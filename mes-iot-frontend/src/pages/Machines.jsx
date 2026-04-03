// src/pages/Machines.jsx
import React, { useState, useEffect } from 'react';
import { Search, Plus, Edit, Trash2, X } from 'lucide-react';
import axios from 'axios';

const Machines = () => {
  const [machines, setMachines] = useState([]);
  const [searchQuery, setSearchQuery] = useState('');
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const [editId, setEditId] = useState(null);
  
  // Đã sửa mặc định thành ONLINE cho khớp DB Oracle
  const [formData, setFormData] = useState({
    machineCode: '',
    machineName: '',
    machineType: 'CNC', 
    location: '',
    status: 'ONLINE'    
  });

  const API_BASE_URL = 'http://localhost:5015/api/machines'; 

  const fetchMachines = async () => {
    try {
      const res = await axios.get(API_BASE_URL);
      setMachines(res.data);
    } catch (error) {
      console.error("Lỗi khi tải danh sách máy:", error);
    }
  };

  useEffect(() => {
    fetchMachines();
  }, []);

  const handleOpenAdd = () => {
    // Đã sửa mặc định thành ONLINE
    setFormData({ machineCode: '', machineName: '', machineType: 'CNC', location: '', status: 'ONLINE' });
    setIsEditMode(false);
    setIsModalOpen(true);
  };

  const handleOpenEdit = (machine) => {
    setFormData({
      machineCode: machine.machineCode,
      machineName: machine.machineName,
      machineType: machine.machineType || '',
      location: machine.location || '',
      status: machine.status || 'ONLINE' // Đã sửa mặc định thành ONLINE
    });
    setEditId(machine.id || machine.machineId); 
    setIsEditMode(true);
    setIsModalOpen(true);
  };

  const handleSave = async (e) => {
    e.preventDefault();
    
    const payload = {
      machineCode: formData.machineCode,
      machineName: formData.machineName,
      machineType: formData.machineType,
      location: formData.location,
      status: formData.status
    };

    try {
      if (isEditMode) {
        await axios.put(`${API_BASE_URL}/${editId}`, payload);
        alert("Cập nhật thành công!");
      } else {
        await axios.post(API_BASE_URL, payload);
        alert("Thêm máy mới thành công!");
      }
      setIsModalOpen(false); 
      fetchMachines();       
    } catch (error) {
      console.error("Chi tiết lỗi:", error.response?.data);
      alert(`Lỗi Backend: ${JSON.stringify(error.response?.data || "Bad Request")}`);
    }
  };

  const handleDelete = async (id, name) => {
    if (window.confirm(`Bạn có chắc chắn muốn xóa máy "${name}" không?`)) {
      try {
        await axios.delete(`${API_BASE_URL}/${id}`);
        fetchMachines(); 
      } catch (error) {
        console.error("Lỗi khi xóa:", error);
        alert("Lỗi khi xóa máy.");
      }
    }
  };

  const filteredMachines = machines.filter(m => 
    m.machineName?.toLowerCase().includes(searchQuery.toLowerCase()) || 
    m.machineCode?.toLowerCase().includes(searchQuery.toLowerCase())
  );

  // ĐÃ FIX LỖI MÀU SẮC TRẠNG THÁI (Bắt chuẩn chữ ONLINE)
  const renderBadge = (status) => {
    const isOnline = status === 'ONLINE' || status === 'ACTIVE'; // Chấp nhận cả 2 phòng hờ
    return (
      <div className={`inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold ${isOnline ? 'bg-[#e5f9e5] text-[#05cd99]' : 'bg-[#fffaf9] text-[#ee5d50]'}`}>
        <span className={`w-2 h-2 rounded-full ${isOnline ? 'bg-[#05cd99]' : 'bg-[#ee5d50]'}`}></span>
        {status || 'OFFLINE'}
      </div>
    );
  };

  return (
    <div className="flex flex-col gap-5">
      <div className="flex justify-between items-center bg-white p-5 rounded-xl shadow-sm border border-gray-100">
        <h2 className="text-[#2b3674] text-xl font-bold">Quản lý Máy móc</h2>
        
        <div className="flex gap-4">
          <div className="relative flex items-center">
            <Search className="absolute left-3 text-[#8f9bba]" size={18} />
            <input 
              type="text" 
              placeholder="Tìm tên hoặc mã máy..." 
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="pl-10 pr-4 py-2 border border-gray-200 rounded-md outline-none focus:border-[#4318ff] transition text-sm w-[250px]"
            />
          </div>
          <button onClick={handleOpenAdd} className="flex items-center gap-2 bg-[#4318ff] hover:bg-[#3311cc] text-white px-5 py-2 rounded-md font-semibold transition">
            <Plus size={18} /> Thêm máy mới
          </button>
        </div>
      </div>

      {/* BẢNG DỮ LIỆU */}
      <div className="bg-white rounded-xl p-5 shadow-sm border border-gray-100">
        <table className="w-full border-collapse">
          <thead>
            <tr>
              <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-semibold text-sm">Mã Máy</th>
              <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-semibold text-sm">Tên Máy</th>
              <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-semibold text-sm">Loại Máy</th>
              <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-semibold text-sm">Vị trí</th>
              <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-semibold text-sm">Trạng thái</th>
              <th className="text-right p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-semibold text-sm w-24">Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {filteredMachines.length === 0 ? (
              <tr><td colSpan="6" className="p-5 text-center text-[#8f9bba]">Không tìm thấy dữ liệu máy móc.</td></tr>
            ) : (
              filteredMachines.map((machine) => (
                <tr key={machine.id || machine.machineId} className="hover:bg-[#f4f7fe] transition">
                  <td className="p-3 border-b border-[#f4f7fe] font-bold text-[#4318ff]">{machine.machineCode}</td>
                  <td className="p-3 border-b border-[#f4f7fe] font-medium text-[#2b3674]">{machine.machineName}</td>
                  <td className="p-3 border-b border-[#f4f7fe] text-[#8f9bba] font-semibold">{machine.machineType}</td>
                  <td className="p-3 border-b border-[#f4f7fe] text-[#8f9bba]">{machine.location || 'Chưa cập nhật'}</td>
                  
                  {/* CỘT TRẠNG THÁI HIỂN THỊ MÀU ĐÚNG CHUẨN */}
                  <td className="p-3 border-b border-[#f4f7fe]">{renderBadge(machine.status)}</td>
                  
                  <td className="p-3 border-b border-[#f4f7fe]">
                    <div className="flex justify-end gap-3">
                      <button onClick={() => handleOpenEdit(machine)} className="p-1.5 text-blue-500 hover:bg-blue-50 rounded-md transition" title="Sửa"><Edit size={18} /></button>
                      <button onClick={() => handleDelete(machine.id || machine.machineId, machine.machineName)} className="p-1.5 text-red-500 hover:bg-red-50 rounded-md transition" title="Xóa"><Trash2 size={18} /></button>
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* MODAL THÊM / SỬA */}
      {isModalOpen && (
        <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
          <div className="bg-white rounded-xl shadow-xl w-[500px] overflow-hidden">
            <div className="flex justify-between items-center bg-[#f4f7fe] px-6 py-4 border-b border-[#e0e5f2]">
              <h3 className="font-bold text-[#2b3674] text-lg">{isEditMode ? 'Chỉnh sửa thông tin máy' : 'Thêm máy móc mới'}</h3>
              <button onClick={() => setIsModalOpen(false)} className="text-[#8f9bba] hover:text-red-500"><X size={20} /></button>
            </div>

            <form onSubmit={handleSave} className="p-6 flex flex-col gap-4">
              <div className="grid grid-cols-2 gap-4">
                <div className="col-span-2">
                  <label className="block text-sm font-semibold text-[#2b3674] mb-1">Mã Máy (Machine Code) *</label>
                  <input type="text" required value={formData.machineCode} onChange={(e) => setFormData({...formData, machineCode: e.target.value})} className="w-full px-3 py-2 border border-gray-300 rounded-md outline-none focus:border-[#4318ff]" placeholder="VD: CNC-01" />
                </div>
                <div className="col-span-2">
                  <label className="block text-sm font-semibold text-[#2b3674] mb-1">Tên Máy (Machine Name) *</label>
                  <input type="text" required value={formData.machineName} onChange={(e) => setFormData({...formData, machineName: e.target.value})} className="w-full px-3 py-2 border border-gray-300 rounded-md outline-none focus:border-[#4318ff]" placeholder="VD: Máy phay CNC" />
                </div>

                <div>
                  <label className="block text-sm font-semibold text-[#2b3674] mb-1">Loại Máy</label>
                  <input type="text" value={formData.machineType} onChange={(e) => setFormData({...formData, machineType: e.target.value})} className="w-full px-3 py-2 border border-gray-300 rounded-md outline-none focus:border-[#4318ff]" placeholder="VD: CNC, LASER..." />
                </div>
                <div>
                  <label className="block text-sm font-semibold text-[#2b3674] mb-1">Trạng thái</label>
                  <select value={formData.status} onChange={(e) => setFormData({...formData, status: e.target.value})} className="w-full px-3 py-2 border border-gray-300 rounded-md outline-none focus:border-[#4318ff]">
                    {/* ĐÃ SỬA CÁC OPTION CHO KHỚP ORACLE */}
                    <option value="ONLINE">ONLINE (Hoạt động)</option>
                    <option value="OFFLINE">OFFLINE (Dừng)</option>
                    <option value="MAINTENANCE">MAINTENANCE (Bảo trì)</option>
                  </select>
                </div>

                <div className="col-span-2">
                  <label className="block text-sm font-semibold text-[#2b3674] mb-1">Vị trí (Location)</label>
                  <input type="text" value={formData.location} onChange={(e) => setFormData({...formData, location: e.target.value})} className="w-full px-3 py-2 border border-gray-300 rounded-md outline-none focus:border-[#4318ff]" placeholder="VD: Line A - Xưởng 1" />
                </div>
              </div>

              <div className="flex justify-end gap-3 mt-4">
                <button type="button" onClick={() => setIsModalOpen(false)} className="px-4 py-2 border border-gray-300 text-gray-600 rounded-md font-semibold hover:bg-gray-50">Hủy bỏ</button>
                <button type="submit" className="px-4 py-2 bg-[#4318ff] text-white rounded-md font-semibold hover:bg-[#3311cc]">{isEditMode ? 'Lưu thay đổi' : 'Thêm mới'}</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Machines;