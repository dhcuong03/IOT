// src/pages/Devices.jsx
import React, { useState, useEffect } from 'react';
import { Search, Plus, Edit, Trash2, X, Cpu } from 'lucide-react';
import axios from 'axios';

const Devices = () => {
  const [devices, setDevices] = useState([]);
  const [searchQuery, setSearchQuery] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const [editId, setEditId] = useState(null);
  
  // ĐÃ SỬA LẠI FORM ĐỂ KHỚP 100% VỚI SWAGGER C#
  const [formData, setFormData] = useState({
    deviceCode: '',
    machineId: '', // Thêm trường này
    protocol: 'MQTT',
    ipAddress: '',
    port: '',
    status: 'DISCONNECTED' // Thêm trường này
  });

  const API_BASE_URL = 'http://localhost:5015/api/devices'; 

  const fetchDevices = async () => {
    try {
      const res = await axios.get(API_BASE_URL);
      setDevices(res.data);
    } catch (error) {
      console.error("Lỗi khi tải danh sách thiết bị:", error);
    }
  };

  useEffect(() => {
    fetchDevices();
  }, []);

  const handleOpenAdd = () => {
    setFormData({ deviceCode: '', machineId: '', protocol: 'MQTT', ipAddress: '', port: '', status: 'DISCONNECTED' });
    setIsEditMode(false);
    setIsModalOpen(true);
  };

  const handleOpenEdit = (device) => {
    setFormData({
      deviceCode: device.deviceCode,
      machineId: device.machineId,
      protocol: device.protocol,
      ipAddress: device.ipAddress,
      port: device.port,
      status: device.status
    });
    setEditId(device.id || device.deviceId); 
    setIsEditMode(true);
    setIsModalOpen(true);
  };

  const handleSave = async (e) => {
    e.preventDefault();
    
    // Gói dữ liệu ép kiểu số cho chuẩn Swagger
    const payload = {
      deviceCode: formData.deviceCode,
      machineId: parseInt(formData.machineId) || 0, // Ép kiểu int
      protocol: formData.protocol,
      ipAddress: formData.ipAddress,
      port: parseInt(formData.port) || 0,         // Ép kiểu int
      status: formData.status
    };

    try {
      if (isEditMode) {
        await axios.put(`${API_BASE_URL}/${editId}`, payload);
        alert("Cập nhật thiết bị thành công!");
      } else {
        await axios.post(API_BASE_URL, payload);
        alert("Thêm thiết bị mới thành công!");
      }
      setIsModalOpen(false);
      fetchDevices(); 
    } catch (error) {
      console.error("Lỗi:", error.response?.data);
      alert(`Lỗi Backend: ${JSON.stringify(error.response?.data || "Bad Request")}`);
    }
  };

  const handleDelete = async (id, code) => {
    if (window.confirm(`Bạn có chắc chắn muốn xóa thiết bị "${code}" không?`)) {
      try {
        await axios.delete(`${API_BASE_URL}/${id}`);
        fetchDevices(); 
      } catch (error) {
        console.error("Lỗi khi xóa:", error);
      }
    }
  };

  const renderBadge = (status) => {
    const isOnline = status === 'CONNECTED';
    return (
      <div className={`inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold ${isOnline ? 'bg-[#e5f9e5] text-[#05cd99]' : 'bg-[#f4f7fe] text-[#8f9bba]'}`}>
        <span className={`w-2 h-2 rounded-full ${isOnline ? 'bg-[#05cd99]' : 'bg-[#8f9bba]'}`}></span>
        {status || 'UNKNOWN'}
      </div>
    );
  };

  const filteredDevices = devices.filter(d => 
    d.deviceCode?.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <div className="flex flex-col gap-5">
      {/* HEADER */}
      <div className="flex justify-between items-center bg-white p-5 rounded-xl shadow-sm border border-gray-100">
        <div className="flex items-center gap-3">
          <div className="p-2 bg-[#f4f7fe] rounded-lg text-[#4318ff]"><Cpu size={24} /></div>
          <h2 className="text-[#2b3674] text-xl font-bold">Quản lý Thiết bị (IoT Sensors)</h2>
        </div>
        
        <div className="flex gap-4">
          <div className="relative flex items-center">
            <Search className="absolute left-3 text-[#8f9bba]" size={18} />
            <input 
              type="text" 
              placeholder="Tìm mã thiết bị..." 
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="pl-10 pr-4 py-2 border border-gray-200 rounded-md outline-none focus:border-[#4318ff] transition text-sm w-[250px]"
            />
          </div>
          <button onClick={handleOpenAdd} className="flex items-center gap-2 bg-[#4318ff] hover:bg-[#3311cc] text-white px-5 py-2 rounded-md font-semibold transition">
            <Plus size={18} /> Thêm thiết bị mới
          </button>
        </div>
      </div>

      {/* BẢNG DỮ LIỆU */}
      <div className="bg-white rounded-xl p-5 shadow-sm border border-gray-100">
        <table className="w-full border-collapse">
          <thead>
            <tr>
              <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-semibold text-sm">Mã Thiết Bị</th>
              <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-semibold text-sm">Mã Máy (Machine ID)</th>
              <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-semibold text-sm">Giao Thức</th>
              <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-semibold text-sm">IP / Port</th>
              <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-semibold text-sm">Trạng thái</th>
              <th className="text-right p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-semibold text-sm">Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {filteredDevices.length === 0 ? (
              <tr><td colSpan="6" className="p-5 text-center text-[#8f9bba]">Không tìm thấy thiết bị nào.</td></tr>
            ) : (
              filteredDevices.map((dev) => (
                <tr key={dev.id || dev.deviceId} className="hover:bg-[#f4f7fe] transition border-b border-[#f4f7fe]">
                  <td className="p-3 font-bold text-[#4318ff]">{dev.deviceCode}</td>
                  <td className="p-3 font-medium text-[#2b3674]">{dev.machineId}</td>
                  <td className="p-3 text-[#8f9bba] font-semibold">{dev.protocol}</td>
                  <td className="p-3 text-[#8f9bba] font-mono">{dev.ipAddress}:{dev.port}</td>
                  <td className="p-3">{renderBadge(dev.status)}</td>
                  <td className="p-3">
                    <div className="flex justify-end gap-3">
                      <button onClick={() => handleOpenEdit(dev)} className="p-1.5 text-blue-500 hover:bg-blue-50 rounded-md transition"><Edit size={18} /></button>
                      <button onClick={() => handleDelete(dev.id || dev.deviceId, dev.deviceCode)} className="p-1.5 text-red-500 hover:bg-red-50 rounded-md transition"><Trash2 size={18} /></button>
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
              <h3 className="font-bold text-[#2b3674] text-lg">{isEditMode ? 'Chỉnh sửa Thiết bị' : 'Thêm Thiết bị mới'}</h3>
              <button onClick={() => setIsModalOpen(false)} className="text-[#8f9bba] hover:text-red-500"><X size={20} /></button>
            </div>
            <form onSubmit={handleSave} className="p-6 flex flex-col gap-4">
              <div className="grid grid-cols-2 gap-4">
                <div className="col-span-2">
                  <label className="block text-sm font-semibold text-[#2b3674] mb-1">Mã Thiết bị *</label>
                  <input type="text" required value={formData.deviceCode} onChange={(e) => setFormData({...formData, deviceCode: e.target.value})} className="w-full px-3 py-2 border border-gray-300 rounded-md outline-none focus:border-[#4318ff]" placeholder="VD: SENSOR-01" />
                </div>
                <div>
                  <label className="block text-sm font-semibold text-[#2b3674] mb-1">Mã Máy (Machine ID)</label>
                  <input type="number" required value={formData.machineId} onChange={(e) => setFormData({...formData, machineId: e.target.value})} className="w-full px-3 py-2 border border-gray-300 rounded-md outline-none focus:border-[#4318ff]" placeholder="VD: 1" />
                </div>
                <div>
                  <label className="block text-sm font-semibold text-[#2b3674] mb-1">Trạng thái</label>
                  <select value={formData.status} onChange={(e) => setFormData({...formData, status: e.target.value})} className="w-full px-3 py-2 border border-gray-300 rounded-md outline-none focus:border-[#4318ff]">
                    <option value="CONNECTED">CONNECTED</option>
                    <option value="DISCONNECTED">DISCONNECTED</option>
                    <option value="ERROR">ERROR</option>
                  </select>
                </div>
                <div className="col-span-2">
                  <label className="block text-sm font-semibold text-[#2b3674] mb-1">Giao thức kết nối</label>
                  <select value={formData.protocol} onChange={(e) => setFormData({...formData, protocol: e.target.value})} className="w-full px-3 py-2 border border-gray-300 rounded-md outline-none focus:border-[#4318ff]">
                    <option value="MQTT">MQTT</option>
                    <option value="HTTP">HTTP/REST</option>
                    <option value="TCP">TCP/IP</option>
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-semibold text-[#2b3674] mb-1">Địa chỉ IP / Host</label>
                  <input type="text" value={formData.ipAddress} onChange={(e) => setFormData({...formData, ipAddress: e.target.value})} className="w-full px-3 py-2 border border-gray-300 rounded-md outline-none focus:border-[#4318ff]" placeholder="192.168.1.100" />
                </div>
                <div>
                  <label className="block text-sm font-semibold text-[#2b3674] mb-1">Cổng (Port)</label>
                  <input type="number" value={formData.port} onChange={(e) => setFormData({...formData, port: e.target.value})} className="w-full px-3 py-2 border border-gray-300 rounded-md outline-none focus:border-[#4318ff]" placeholder="VD: 1883" />
                </div>
              </div>
              <div className="flex justify-end gap-3 mt-4">
                <button type="button" onClick={() => setIsModalOpen(false)} className="px-4 py-2 border border-gray-300 text-gray-600 rounded-md font-semibold hover:bg-gray-50">Hủy</button>
                <button type="submit" className="px-4 py-2 bg-[#4318ff] text-white rounded-md font-semibold hover:bg-[#3311cc]">{isEditMode ? 'Cập nhật' : 'Thêm mới'}</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Devices;