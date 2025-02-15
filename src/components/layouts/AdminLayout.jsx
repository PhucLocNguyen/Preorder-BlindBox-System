import React from "react";
import { Outlet } from "react-router";
import SidebarAdmin from "../../pages/Admin/SidebarAdmin";

function AdminLayout({ children }) {
  return (
    <div className="relative">
      <h2>Admin layout</h2>
      <>{children}</>
      <div className="h-screen flex flex-col overflow-hidden">
        <div className="flex flex-1 overflow-hidden gap-4 px-4">
          <div className="h-full">
            <SidebarAdmin />
          </div>


          <div className="flex-1 flex flex-col h-full">
            <Outlet />
          </div>
        </div>
      </div>
    </div>
  );
}

export default AdminLayout;