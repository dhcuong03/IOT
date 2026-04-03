// src/pages/MachineDetails.jsx
import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, Cpu, Settings, Activity, Edit2, X, Plus } from 'lucide-react';
import axios from 'axios';

const MachineDetails = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  
  const [activeTab, setActiveTab] = useState('devices'); 
  
  const [machine, setMachine] = useState(null);
  const [devices, setDevices] = useState([]);
  const [configs, setConfigs] = useState([]);
  const [selectedDeviceId, setSelectedDeviceId] = useState('');

  // STATES CHO POPUP CẤU HÌNH (Đã nâng cấp)
  const [isConfigModalOpen, setIsConfigModalOpen] = useState(false);
  const [isNewConfig, setIsNewConfig] = useState(false); // <--- Thêm cờ nhận biết là Thêm mới hay Sửa
  const [editConfig, setEditConfig] = useState({ id: null, key: '', value: '' });

  const API_BASE_URL = 'http://localhost:5015/api';

  useEffect(() => {
    const fetchMachineAndDevices = async () => {
      try {
        const machineRes = await axios.get(`${API_BASE_URL}/machines/${id}`);
        setMachine(machineRes.data);

        const devicesRes = await axios.get(`${API_BASE_URL}/devices`);
        const machineDevices = devicesRes.data.filter(dev => 
          dev.machineId && dev.machineId.toString() === id.toString()
        );
        
        setDevices(machineDevices);
        if (machineDevices.length > 0) {
          setSelectedDeviceId(machineDevices[0].id || machineDevices[0].deviceId);
        }
      } catch (error) {
        console.error("Lỗi tải thông tin máy hoặc thiết bị:", error);
      }
    };
    fetchMachineAndDevices();
  }, [id]);

  useEffect(() => {
    if (selectedDeviceId) {
      // Mock data cấu hình (Khi có API thật C# thì thay bằng axios.get)
      setConfigs([
        { id: 1, key: 'TEMP_MAX', value: '80' },
        { id: 2, key: 'TEMP_MIN', value: '15' },
        { id: 3, key: 'REPORT_INTERVAL_MS', value: '5000' }
      ]);
    }
  }, [selectedDeviceId]);

  const renderBadge = (status) => {
    const isOnline = status === 'CONNECTED' || status === 'ONLINE' || status === 'ACTIVE';
    const bgClass = isOnline ? 'bg-[#e5f9e5] text-[#05cd99]' : 'bg-[#fffaf9] text-[#ee5d50]';
    const dotClass = isOnline ? 'bg-[#05cd99]' : 'bg-[#ee5d50]';

    return (
      <div className={`inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold ${bgClass}`}>
        <span className={`w-2 h-2 rounded-full ${dotClass}`}></span>
        {status || 'OFFLINE'}
      </div>
    );
  };

  // HÀM: MỞ POPUP THÊM MỚI
  const handleOpenAddConfig = () => {
    setEditConfig({ id: null, key: '', value: '' }); // Làm trống form
    setIsNewConfig(true); // Đánh dấu đây là chế độ Thêm mới
    setIsConfigModalOpen(true);
  };

  // HÀM: MỞ POPUP SỬA
  const handleOpenEditConfig = (config) => {
    setEditConfig({ id: config.id, key: config.key, value: config.value });
    setIsNewConfig(false); // Đánh dấu đây là chế độ Sửa
    setIsConfigModalOpen(true);
  };

  // HÀM: LƯU CẤU HÌNH (THÊM HOẶC SỬA)
  const handleSaveConfig = async () => {
    try {
      if (isNewConfig) {
        // --- Gọi API POST khi C# có sẵn ---
        // await axios.post(`${API_BASE_URL}/deviceconfigs`, { deviceId: selectedDeviceId, key: editConfig.key, value: editConfig.value });
        
        // Tạm thời update giao diện ngay lập tức
        setConfigs([...configs, { id: Date.now(), key: editConfig.key.toUpperCase(), value: editConfig.value }]);
        alert("Đã thêm thông số cấu hình mới!");
      } else {
        // --- Gọi API PUT khi C# có sẵn ---
        // await axios.put(`${API_BASE_URL}/deviceconfigs/${editConfig.id}`, { value: editConfig.value });
        
        // Tạm thời update giao diện
        setConfigs(configs.map(c => c.id === editConfig.id ? { ...c, value: editConfig.value } : c));
        alert("Đã cập nhật cấu hình thành công!");
      }
      setIsConfigModalOpen(false);
    } catch (error) {
      console.error("Lỗi khi lưu cấu hình:", error);
      alert("Lỗi khi lưu cấu hình!");
    }
  };

  if (!machine) return <div className="p-10 font-bold text-[#4318ff] animate-pulse">Đang tải thông tin chi tiết máy...</div>;

  return (
    <div className="flex flex-col gap-6">
      <div className="bg-white rounded-xl p-6 shadow-sm flex flex-col gap-4 relative overflow-hidden border border-gray-100">
        <button onClick={() => navigate(-1)} className="flex items-center gap-2 text-[#8f9bba] hover:text-[#4318ff] font-semibold w-fit transition">
          <ArrowLeft size={18} /> Quay lại
        </button>
        <div className="flex items-center justify-between mt-2">
          <div className="flex items-center gap-5">
            <div className="w-16 h-16 bg-[#f4f7fe] text-[#4318ff] rounded-xl flex items-center justify-center">
              <Activity size={32} />
            </div>
            <div>
              <h1 className="text-3xl font-bold text-[#2b3674]">{machine.machineName}</h1>
              <p className="text-[#8f9bba] font-medium mt-1">
                Mã: <span className="font-semibold">{machine.machineCode}</span> &bull; Vị trí: {machine.location || 'Chưa cập nhật'}
              </p>
            </div>
          </div>
          <div>{renderBadge(machine.status)}</div>
        </div>
      </div>

      <div className="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden flex flex-col flex-1 min-h-[400px]">
        <div className="flex border-b border-[#e0e5f2]">
          <button onClick={() => setActiveTab('devices')} className={`flex items-center gap-2 px-8 py-4 font-bold text-sm transition-all border-b-2 ${activeTab === 'devices' ? 'border-[#4318ff] text-[#4318ff] bg-[#f4f7fe]' : 'border-transparent text-[#8f9bba] hover:text-[#2b3674]'}`}>
            <Cpu size={18} /> Danh sách Thiết bị (IoT)
          </button>
          <button onClick={() => setActiveTab('configs')} className={`flex items-center gap-2 px-8 py-4 font-bold text-sm transition-all border-b-2 ${activeTab === 'configs' ? 'border-[#4318ff] text-[#4318ff] bg-[#f4f7fe]' : 'border-transparent text-[#8f9bba] hover:text-[#2b3674]'}`}>
            <Settings size={18} /> Cấu hình Thông số
          </button>
        </div>

        <div className="p-6">
          {activeTab === 'devices' && (
            <div>
              <div className="flex justify-between items-center mb-4">
                <h3 className="text-[#2b3674] font-bold">Các cảm biến đang gắn trên máy này</h3>
                <button onClick={() => navigate('/devices')} className="flex items-center gap-2 bg-white border border-[#4318ff] text-[#4318ff] hover:bg-[#4318ff] hover:text-white px-4 py-1.5 rounded-md font-semibold transition text-sm">
                  <Plus size={16} /> Thêm thiết bị
                </button>
              </div>
              <table className="w-full border-collapse">
                <thead>
                  <tr className="bg-[#f4f7fe]">
                    <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm rounded-tl-lg">Mã Thiết Bị</th>
                    <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm">Giao Thức</th>
                    <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm">IP / Endpoint</th>
                    <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm">Port</th>
                    <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm">Trạng Thái</th>
                    <th className="text-right p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm rounded-tr-lg w-28">Thao tác</th>
                  </tr>
                </thead>
                <tbody>
                  {devices.length === 0 ? (
                    <tr><td colSpan="6" className="p-8 text-center text-[#8f9bba] font-medium">Máy này hiện chưa được gắn thiết bị IoT nào.</td></tr>
                  ) : (
                    devices.map(dev => (
                      <tr key={dev.id || dev.deviceId} className="hover:bg-[#f8f9ff] transition border-b border-[#f4f7fe]">
                        <td className="p-3 font-bold text-[#2b3674]">{dev.deviceCode}</td>
                        <td className="p-3 text-[#8f9bba] font-medium">{dev.protocol}</td>
                        <td className="p-3 text-[#8f9bba]">{dev.ipAddress}</td>
                        <td className="p-3 text-[#8f9bba]">{dev.port}</td>
                        <td className="p-3">{renderBadge(dev.status)}</td>
                        <td className="p-3 text-right">
                          <button onClick={() => navigate(`/devices/${dev.id || dev.deviceId}/monitor`)} className="flex items-center gap-1.5 px-3 py-1.5 border border-[#05cd99] text-[#05cd99] rounded-md font-bold hover:bg-[#05cd99] hover:text-white transition text-sm inline-flex">
                            <Activity size={16} /> Giám sát
                          </button>
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          )}

          {activeTab === 'configs' && (
            <div>
              <div className="flex justify-between items-center mb-6 bg-[#f4f7fe] p-4 rounded-lg border border-[#e0e5f2]">
                <div className="flex items-center gap-3">
                  <span className="font-semibold text-[#2b3674]">Chọn thiết bị để cấu hình:</span>
                  <select 
                    value={selectedDeviceId} 
                    onChange={(e) => setSelectedDeviceId(Number(e.target.value))}
                    className="px-3 py-1.5 border border-gray-300 rounded-md outline-none text-[#2b3674] font-medium w-64"
                    disabled={devices.length === 0}
                  >
                    {devices.length === 0 ? (
                      <option value="">Không có thiết bị</option>
                    ) : (
                      devices.map(dev => (
                        <option key={dev.id || dev.deviceId} value={dev.id || dev.deviceId}>{dev.deviceCode} ({dev.protocol})</option>
                      ))
                    )}
                  </select>
                </div>
                
                {/* NÚT THÊM KEY MỚI ĐÃ ĐƯỢC KÍCH HOẠT */}
                <button 
                  onClick={handleOpenAddConfig}
                  className="flex items-center gap-2 bg-[#4318ff] hover:bg-[#3311cc] text-white px-4 py-1.5 rounded-md font-semibold transition text-sm disabled:bg-gray-300"
                  disabled={devices.length === 0}
                >
                  <Plus size={16} /> Thêm Key mới
                </button>
              </div>

              <table className="w-full border-collapse">
                <thead>
                  <tr className="bg-[#f4f7fe]">
                    <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm rounded-tl-lg">Config Key</th>
                    <th className="text-left p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm">Value</th>
                    <th className="text-right p-3 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm rounded-tr-lg">Action</th>
                  </tr>
                </thead>
                <tbody>
                  {devices.length === 0 ? (
                    <tr><td colSpan="3" className="p-8 text-center text-[#8f9bba] font-medium">Vui lòng gắn thiết bị trước khi cài đặt cấu hình.</td></tr>
                  ) : (
                    configs.map(cfg => (
                      <tr key={cfg.id} className="hover:bg-[#f8f9ff] transition border-b border-[#f4f7fe]">
                        <td className="p-3 font-bold text-[#2b3674]">{cfg.key}</td>
                        <td className="p-3 font-mono text-[#4318ff] font-bold bg-blue-50 px-3 py-1 rounded w-fit inline-block mt-2">{cfg.value}</td>
                        <td className="p-3 text-right">
                          <button onClick={() => handleOpenEditConfig(cfg)} className="p-2 text-blue-500 hover:bg-blue-100 rounded-md transition inline-flex" title="Sửa giá trị">
                            <Edit2 size={16} />
                          </button>
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>

      {/* POPUP SỬA/THÊM CẤU HÌNH ĐÃ NÂNG CẤP */}
      {isConfigModalOpen && (
        <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
          <div className="bg-white rounded-xl shadow-xl w-[400px] overflow-hidden">
            <div className="flex justify-between items-center bg-[#f4f7fe] px-6 py-4 border-b border-[#e0e5f2]">
              <h3 className="font-bold text-[#2b3674]">{isNewConfig ? 'Thêm cấu hình mới' : 'Đổi giá trị cấu hình'}</h3>
              <button onClick={() => setIsConfigModalOpen(false)} className="text-[#8f9bba] hover:text-red-500"><X size={20} /></button>
            </div>
            <div className="p-6">
              
              <label className="block text-sm font-semibold text-[#8f9bba] mb-1">Config Key</label>
              <input 
                type="text" 
                value={editConfig.key} 
                onChange={(e) => setEditConfig({...editConfig, key: e.target.value})}
                disabled={!isNewConfig} // Chỉ cho phép nhập nếu là Thêm Mới
                placeholder="VD: VOLTAGE_MAX"
                className={`w-full px-3 py-2 border rounded-md font-mono mb-4 outline-none uppercase ${!isNewConfig ? 'bg-gray-100 text-gray-500 border-gray-300' : 'bg-white text-[#2b3674] border-[#4318ff] focus:ring-2 focus:ring-blue-200'}`} 
              />
              
              <label className="block text-sm font-semibold text-[#2b3674] mb-1">Value (Giá trị)</label>
              <input 
                type="text" 
                value={editConfig.value} 
                onChange={(e) => setEditConfig({...editConfig, value: e.target.value})}
                placeholder="VD: 220"
                className="w-full px-3 py-2 border border-[#4318ff] rounded-md outline-none font-mono text-[#4318ff] font-bold focus:ring-2 focus:ring-blue-200" autoFocus 
              />
              
              <div className="flex justify-end gap-3 mt-6">
                <button onClick={() => setIsConfigModalOpen(false)} className="px-4 py-2 border border-gray-300 text-gray-600 rounded-md font-semibold hover:bg-gray-50">Hủy</button>
                <button onClick={handleSaveConfig} className="px-4 py-2 bg-[#4318ff] hover:bg-[#3311cc] text-white rounded-md font-semibold transition">
                  {isNewConfig ? 'Thêm cấu hình' : 'Lưu thay đổi'}
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default MachineDetails;