import { NavLink } from 'react-router-dom';
import { LayoutDashboard, Server, Cpu, AlertTriangle, Factory } from 'lucide-react';

const Sidebar = () => {
  return (
    <div className="sidebar">
      {/* Logo */}
      <div className="sidebar-logo">
        <Factory size={28} color="white" />
        <h2>MES System</h2>
      </div>
      
      {/* Menu chính */}
      <nav className="sidebar-menu">
        <NavLink to="/dashboard" className={({ isActive }) => isActive ? "menu-item active" : "menu-item"}>
          <LayoutDashboard size={20} /> Tổng quan (Dashboard)
        </NavLink>
        
        <NavLink to="/machines" className={({ isActive }) => isActive ? "menu-item active" : "menu-item"}>
          <Server size={20} /> Quản lý Máy móc
        </NavLink>

        <NavLink to="/devices" className={({ isActive }) => isActive ? "menu-item active" : "menu-item"}>
          <Cpu size={20} /> Quản lý Thiết bị (IoT)
        </NavLink>

        <NavLink to="/alerts" className={({ isActive }) => isActive ? "menu-item active" : "menu-item"}>
          <AlertTriangle size={20} /> Cảnh báo
        </NavLink>
      </nav>
    </div>
  );
};

export default Sidebar;