// src/pages/TelemetryMonitor.jsx
import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, Thermometer, Clock, AlertTriangle, Cpu } from 'lucide-react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

// --- COMPONENT TỰ VIẾT: ĐỒNG HỒ GAUGE SIÊU NHẸ & KHÔNG BAO GIỜ LỖI ---
const CustomGauge = ({ value, threshold = 80, max = 100 }) => {
  const radius = 80;
  const circumference = Math.PI * radius; // Nửa chu vi hình tròn
  const percent = Math.min(Math.max(value, 0), max) / max;
  const strokeDashoffset = circumference - percent * circumference;

  // Đổi màu theo nhiệt độ
  let color = "#05cd99"; // Xanh lá (Bình thường)
  if (value >= threshold) color = "#ee5d50"; // Đỏ (Báo động)
  else if (value >= threshold - 10) color = "#ffb547"; // Cam (Cảnh báo)

  return (
    <div className="relative w-full max-w-[280px] mx-auto flex flex-col items-center">
      <svg viewBox="0 0 200 120" className="w-full drop-shadow-md">
        {/* Vòng cung nền (Màu xám nhạt) */}
        <path
          d="M 20 100 A 80 80 0 0 1 180 100"
          fill="none"
          stroke="#f4f7fe"
          strokeWidth="18"
          strokeLinecap="round"
        />
        {/* Vòng cung dữ liệu (Có màu & hiệu ứng chuyển động) */}
        <path
          d="M 20 100 A 80 80 0 0 1 180 100"
          fill="none"
          stroke={color}
          strokeWidth="18"
          strokeLinecap="round"
          strokeDasharray={circumference}
          strokeDashoffset={strokeDashoffset}
          className="transition-all duration-700 ease-out"
        />
        {/* Hiển thị số ở giữa */}
        <text x="100" y="90" textAnchor="middle" fontSize="32" fontWeight="bold" fill={color}>
          {value.toFixed(1)}°C
        </text>
      </svg>
    </div>
  );
};

// --- COMPONENT CHÍNH ---
const TelemetryMonitor = () => {
  const { deviceId } = useParams();
  const navigate = useNavigate();
  
  // States
  const [currentTemp, setCurrentTemp] = useState(0);
  const [historicalData, setHistoricalData] = useState([]);
  const [deviceAlerts, setDeviceAlerts] = useState([]);
  const [deviceInfo, setDeviceInfo] = useState(null);

  const TEMP_MAX_THRESHOLD = 80.0;

  // Mô phỏng gọi API và Real-time data
  useEffect(() => {
    // 1. Lấy thông tin thiết bị
    setDeviceInfo({ id: deviceId, code: 'TEMP-SENSOR-01', location: 'Line A - Xưởng 1', type: 'TEMPERATURE' });

    // 2. Lấy dữ liệu lịch sử (10 điểm gần nhất)
    const now = new Date();
    const initialData = [];
    for (let i = 9; i >= 0; i--) {
      const time = new Date(now.getTime() - i * 60 * 1000);
      initialData.push({
        time: `${time.getHours().toString().padStart(2, '0')}:${time.getMinutes().toString().padStart(2, '0')}`,
        temp: Math.floor(40 + Math.random() * 30), // Nhiệt độ bình thường
      });
    }
    setHistoricalData(initialData);

    // 3. Real-time polling: Cứ 3 giây sinh 1 điểm dữ liệu mới
    const intervalId = setInterval(() => {
      const newTime = new Date();
      const timeString = `${newTime.getHours().toString().padStart(2, '0')}:${newTime.getMinutes().toString().padStart(2, '0')}:${newTime.getSeconds().toString().padStart(2, '0')}`;
      
      // Tạo nhiệt độ ngẫu nhiên (Có xác suất vọt lên trên 80 độ để test cảnh báo)
      const isSpike = Math.random() > 0.8; 
      const newTemp = isSpike ? 80 + Math.random() * 15 : 40 + Math.random() * 35;
      
      setCurrentTemp(newTemp);

      // Cập nhật biểu đồ
      setHistoricalData(prev => {
        const newData = [...prev, { time: timeString, temp: parseFloat(newTemp.toFixed(1)) }];
        if (newData.length > 20) newData.shift(); // Giữ lại tối đa 20 điểm
        return newData;
      });

      // Ghi log nếu vượt ngưỡng
      if (newTemp > TEMP_MAX_THRESHOLD) {
        setDeviceAlerts(prev => {
          const alertMessage = `${timeString} - Cảnh báo khẩn: Nhiệt độ vượt ngưỡng ${newTemp.toFixed(1)}°C`;
          return [...prev, { id: newTime.getTime(), message: alertMessage }];
        });
      }
    }, 3000);

    return () => clearInterval(intervalId);
  }, [deviceId]);

  if (!deviceInfo) return <div className="p-10 font-bold text-[#4318ff]">Đang tải dữ liệu thiết bị...</div>;

  const isOverThreshold = currentTemp > TEMP_MAX_THRESHOLD;

  return (
    <div className="flex flex-col gap-6">
      {/* HEADER */}
      <div className="flex items-center justify-between bg-white p-5 rounded-xl shadow-sm border border-gray-100">
        <div className="flex items-center gap-4">
          <button onClick={() => navigate(-1)} className="p-2 text-[#8f9bba] hover:text-[#4318ff] hover:bg-[#f4f7fe] rounded-lg transition-colors">
            <ArrowLeft size={22} />
          </button>
          <div>
            <div className="text-xs font-bold text-[#8f9bba] uppercase flex items-center gap-2 tracking-wider mb-1">
              <Cpu size={14} /> Giám sát Thiết bị IoT
            </div>
            <h1 className="text-xl font-bold text-[#2b3674]">{deviceInfo.code}</h1>
            <p className="text-[#8f9bba] text-sm font-medium mt-0.5">Vị trí: {deviceInfo.location}</p>
          </div>
        </div>
        <div className="flex items-center gap-2 bg-[#f4f7fe] px-4 py-2 rounded-lg border border-[#e0e5f2]">
          <Clock size={16} className="text-[#4318ff]" />
          <span className="text-sm font-bold text-[#2b3674]">Realtime Sync</span>
        </div>
      </div>

      {/* MAIN CONTENT: Lưới 2 cột */}
      <div className="flex gap-6 items-stretch">
        
        {/* CỘT TRÁI: Gồm Nửa trên (Gauge) và Nửa dưới (Line Chart) */}
        <div className="flex-1 flex flex-col gap-6 w-2/3">
          
          {/* NỬA TRÊN: ĐỒNG HỒ ĐO (GAUGE) */}
          <div className={`bg-white rounded-xl p-6 shadow-sm border-2 transition-colors duration-500 ${isOverThreshold ? 'border-[#ee5d50] bg-[#fffaf9]' : 'border-transparent'}`}>
            <div className="flex justify-between items-start mb-2">
              <h3 className="text-[#2b3674] font-bold text-lg">Real-time Temperature</h3>
              <div className={`flex items-center gap-2 px-4 py-1.5 rounded-full font-bold text-sm ${isOverThreshold ? 'bg-[#ee5d50] text-white' : 'bg-[#e5f9e5] text-[#05cd99]'}`}>
                <Thermometer size={18} />
                {isOverThreshold ? 'CẢNH BÁO' : 'BÌNH THƯỜNG'}
              </div>
            </div>
            
            {/* Gọi Component CustomGauge tự code */}
            <CustomGauge value={currentTemp} threshold={TEMP_MAX_THRESHOLD} max={100} />
          </div>

          {/* NỬA DƯỚI: BIỂU ĐỒ LỊCH SỬ (LINE CHART) */}
          <div className="bg-white rounded-xl p-6 shadow-sm border border-gray-100 flex-1 min-h-[350px] flex flex-col">
            <h3 className="text-[#2b3674] font-bold mb-6 text-lg">Biểu đồ Lịch sử (20 lần đọc gần nhất)</h3>
            <div className="flex-1 w-full">
              <ResponsiveContainer width="100%" height="100%">
                <LineChart data={historicalData} margin={{ top: 5, right: 20, left: 0, bottom: 5 }}>
                  <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#e0e5f2" />
                  <XAxis dataKey="time" tick={{fontSize: 12, fill: '#8f9bba'}} axisLine={false} tickLine={false} />
                  <YAxis unit="°C" tick={{fontSize: 12, fill: '#8f9bba'}} axisLine={false} tickLine={false} />
                  <Tooltip 
                    contentStyle={{ borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1)' }}
                    itemStyle={{ color: '#4318ff', fontWeight: 'bold' }}
                  />
                  <Legend iconType="circle" wrapperStyle={{ fontSize: '14px', fontWeight: '500', color: '#2b3674' }} />
                  <Line 
                    type="monotone" 
                    name="Nhiệt độ (°C)" 
                    dataKey="temp" 
                    stroke={isOverThreshold ? '#ee5d50' : '#4318ff'} 
                    strokeWidth={4} 
                    dot={false} 
                    activeDot={{ r: 8, strokeWidth: 0 }} 
                  />
                </LineChart>
              </ResponsiveContainer>
            </div>
          </div>
        </div>

        {/* CỘT PHẢI: NHẬT KÝ CẢNH BÁO (ALERT LOG) */}
        <div className="w-1/3 bg-white rounded-xl shadow-sm border border-gray-100 flex flex-col overflow-hidden max-h-[calc(100vh-150px)]">
          <div className="flex items-center gap-3 p-5 border-b border-[#e0e5f2] bg-[#f4f7fe]">
            <AlertTriangle className="text-[#ee5d50]" size={20} />
            <h3 className="text-[#2b3674] font-bold text-lg">Nhật ký Cảnh báo</h3>
          </div>
          
          <div className="flex-1 p-5 overflow-y-auto flex flex-col gap-3">
            {deviceAlerts.length === 0 ? (
              <div className="text-center text-[#8f9bba] text-sm mt-10 font-medium">
                Hệ thống đang hoạt động ổn định.<br/>Chưa ghi nhận cảnh báo nào.
              </div>
            ) : (
              // In mảng ngược để cảnh báo mới nhất nằm trên cùng
              deviceAlerts.slice().reverse().map(alert => (
                <div key={alert.id} className="p-3 bg-[#fffaf9] border border-[#ffcfc9] rounded-lg text-[#ee5d50] text-sm font-medium flex items-start gap-2 shadow-sm animate-fade-in">
                  <span className="w-2 h-2 rounded-full bg-[#ee5d50] mt-1.5 flex-shrink-0"></span>
                  <p className="leading-snug">{alert.message}</p>
                </div>
              ))
            )}
          </div>
        </div>

      </div>
    </div>
  );
};

export default TelemetryMonitor;