import { Outlet } from 'react-router-dom';
import Sidebar from './Sidebar';
import Header from './Header';

const MainLayout = () => {
  return (
    <div className="app-container">
      <Sidebar />
      <div className="main-wrapper">
        <Header />
        <div className="main-content">
          <Outlet /> 
        </div>
      </div>
    </div>
  );
};

export default MainLayout;