import { Bell, UserCircle } from 'lucide-react';

const Header = () => {
  return (
    <div className="top-header">
      <div className="header-title">
        <h3>Machine Monitoring Dashboard</h3>
      </div>
      <div className="header-actions">
        {/* Chuông báo */}
        <button className="icon-btn">
          <Bell size={22} />
          <span className="badge">3</span>
        </button>
        {/* Avatar người dùng */}
        <div className="user-profile">
          <UserCircle size={28} />
          <span>Hùng Cường</span>
        </div>
      </div>
    </div>
  );
};

export default Header;