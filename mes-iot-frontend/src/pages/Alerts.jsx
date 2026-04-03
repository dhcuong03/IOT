// src/pages/Alerts.jsx
import React, { useState, useEffect } from 'react';
import { Search, AlertTriangle, CheckCircle, Trash2, Bell, Clock } from 'lucide-react';
import axios from 'axios';

const Alerts = () => {
  const [alerts, setAlerts] = useState([]);
  const [searchQuery, setSearchQuery] = useState('');

  const API_BASE_URL = 'http://localhost:5015/api/alerts'; // URL C# Backend

  // Hàm tải dữ liệu cảnh báo từ DB
  const fetchAlerts = async () => {
    try {
      const res = await axios.get(API_BASE_URL);
      setAlerts(res.data);
    } catch (error) {
      console.error("Lỗi khi tải danh sách cảnh báo:", error);
      // Dữ liệu mẫu (Mock) phòng trường hợp Backend chưa có dữ liệu hoặc lỗi
      setAlerts([
        { id: 1, deviceCode: 'CNC-01', alertType: 'TEMPERATURE', message: 'Nhiệt độ vượt ngưỡng 85°C', severity: 'CRITICAL', timestamp: new Date().toISOString(), status: 'UNREAD' },
        { id: 2, deviceCode: 'SENSOR-02', alertType: 'CONNECTION', message: 'Mất kết nối với thiết bị', severity: 'WARNING', timestamp: new Date(Date.now() - 3600000).toISOString(), status: 'UNREAD' },
      ]);
    }
  };

  useEffect(() => {
    fetchAlerts();
  }, []);

  // Xử lý Xóa cảnh báo (Hoặc đánh dấu đã đọc)
  const handleDelete = async (id) => {
    if (window.confirm("Bạn có muốn xóa cảnh báo này khỏi lịch sử không?")) {
      try {
        await axios.delete(`${API_BASE_URL}/${id}`);
        fetchAlerts();
      } catch (error) {
        console.error("Lỗi khi xóa:", error);
        // Cập nhật UI tạm thời nếu C# chưa có hàm Delete
        setAlerts(alerts.filter(a => a.id !== id)); 
      }
    }
  };

  // Lọc tìm kiếm
  const filteredAlerts = alerts.filter(a => 
    a.message?.toLowerCase().includes(searchQuery.toLowerCase()) || 
    a.deviceCode?.toLowerCase().includes(searchQuery.toLowerCase())
  );

  // Render nhãn Mức độ nghiêm trọng
  const renderSeverity = (severity) => {
    switch (severity?.toUpperCase()) {
      case 'CRITICAL':
        return <span className="px-3 py-1 bg-[#ffebeb] text-[#ee5d50] text-xs font-bold rounded-full flex items-center gap-1 w-max"><AlertTriangle size={12}/> NGHIÊM TRỌNG</span>;
      case 'WARNING':
        return <span className="px-3 py-1 bg-[#fff6e5] text-[#ffb547] text-xs font-bold rounded-full flex items-center gap-1 w-max"><AlertTriangle size={12}/> CẢNH BÁO</span>;
      default:
        return <span className="px-3 py-1 bg-[#e5f9e5] text-[#05cd99] text-xs font-bold rounded-full flex items-center gap-1 w-max"><Bell size={12}/> THÔNG TIN</span>;
    }
  };

  // Format ngày giờ đẹp
  const formatDate = (dateString) => {
    if (!dateString) return '';
    const date = new Date(dateString);
    return `${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')} - ${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
  };

  return (
    <div className="flex flex-col gap-5">
      {/* HEADER */}
      <div className="flex justify-between items-center bg-white p-5 rounded-xl shadow-sm border border-gray-100">
        <div className="flex items-center gap-3">
          <div className="p-2 bg-[#ffebeb] rounded-lg text-[#ee5d50]"><Bell size={24} /></div>
          <div>
            <h2 className="text-[#2b3674] text-xl font-bold">Lịch sử Cảnh báo</h2>
            <p className="text-[#8f9bba] text-sm font-medium">Theo dõi các sự cố bất thường của hệ thống</p>
          </div>
        </div>
        
        <div className="relative flex items-center">
          <Search className="absolute left-3 text-[#8f9bba]" size={18} />
          <input 
            type="text" 
            placeholder="Tìm theo thiết bị, nội dung..." 
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="pl-10 pr-4 py-2 border border-gray-200 rounded-md outline-none focus:border-[#4318ff] transition text-sm w-[300px]"
          />
        </div>
      </div>

      {/* BẢNG DỮ LIỆU */}
      <div className="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
        <table className="w-full border-collapse">
          <thead>
            <tr className="bg-[#f4f7fe]">
              <th className="text-left p-4 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm w-48">Thời gian</th>
              <th className="text-left p-4 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm w-40">Mã Thiết bị</th>
              <th className="text-left p-4 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm">Nội dung sự cố</th>
              <th className="text-left p-4 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm w-40">Mức độ</th>
              <th className="text-right p-4 border-b border-[#e0e5f2] text-[#8f9bba] font-bold text-sm w-24">Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {filteredAlerts.length === 0 ? (
              <tr><td colSpan="5" className="p-10 text-center text-[#8f9bba] font-medium">Hệ thống đang hoạt động an toàn. Không có cảnh báo nào.</td></tr>
            ) : (
              filteredAlerts.map((alert) => (
                <tr key={alert.id} className="hover:bg-[#f4f7fe] transition group border-b border-[#f4f7fe]">
                  <td className="p-4 text-[#8f9bba] font-medium flex items-center gap-2">
                    <Clock size={16} /> {formatDate(alert.timestamp)}
                  </td>
                  <td className="p-4 font-bold text-[#4318ff]">{alert.deviceCode}</td>
                  <td className="p-4 font-semibold text-[#2b3674]">{alert.message}</td>
                  <td className="p-4">{renderSeverity(alert.severity)}</td>
                  <td className="p-4">
                    <div className="flex justify-end gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                      <button onClick={() => handleDelete(alert.id)} className="p-2 text-[#8f9bba] hover:text-[#ee5d50] hover:bg-[#ffebeb] rounded-md transition" title="Xóa cảnh báo">
                        <Trash2 size={18} />
                      </button>
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default Alerts;