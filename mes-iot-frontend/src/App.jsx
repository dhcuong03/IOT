import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import MainLayout from './layouts/MainLayout';
import Dashboard from './pages/Dashboard'; 
import Machines from './pages/Machines';
import MachineDetails from './pages/MachineDetails';
import Devices from './pages/Devices';
import TelemetryMonitor from './pages/TelemetryMonitor';

// 1. IMPORT TRANG CẢNH BÁO XỊN VÀO ĐÂY
import Alerts from './pages/Alerts';

// Đã xóa bỏ cái const Alerts = ... nháp đi rồi nhé!

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<MainLayout />}>
          <Route index element={<Navigate to="/dashboard" replace />} />
          <Route path="dashboard" element={<Dashboard />} />
          <Route path="machines" element={<Machines />} />
          <Route path="machines/:id" element={<MachineDetails />} />
          <Route path="devices/:deviceId/monitor" element={<TelemetryMonitor />} />
          <Route path="devices" element={<Devices />} />
          
          {/* 2. ROUTE TRỎ ĐẾN COMPONENT ALERTS THẬT */}
          <Route path="alerts" element={<Alerts />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;