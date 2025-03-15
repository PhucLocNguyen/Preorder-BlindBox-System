import React from "react";
import { Link, useLocation } from "react-router-dom";
import { Layout, Menu } from "antd";
import {
    DashboardOutlined,
    UserOutlined,
    HistoryOutlined,
    ShoppingCartOutlined,
    TagOutlined,
    LogoutOutlined,
    PictureOutlined,
    CheckSquareOutlined
} from "@ant-design/icons";
import AdminIcon from "../../assets/Admin/AdminIcon.png"
import useLogout from "../../hooks/useLogout";

const { Sider } = Layout;

const SideBarAdmin = () => {
    const location = useLocation();
    const logout = useLogout()

    const menuItems = [
        { key: "/admin/dashboard", label: "Thống kê", icon: <DashboardOutlined /> },
        { key: "/admin/staffmanagement", label: "Quản lý nhân viên", icon: <UserOutlined /> },
        { key: "/admin/history-transactions", label: "History Transactions", icon: <HistoryOutlined /> },
        { key: "/admin/pre-ordercampaign", label: "Quản lý các chiến dịch", icon: <ShoppingCartOutlined /> },
        { key: "/admin/voucher", label: "Mã giảm giá", icon: <TagOutlined /> },
        { key: "/admin/banner-management", label: "Quản lý Banner", icon: <PictureOutlined /> },
        { key: "/admin/preordercampaignApproval", label: "Xét duyệt chiến dịch", icon: <CheckSquareOutlined /> }

    ];

    return (
        <Sider width={220} className="h-full shadow-lg bg-white">
            <div className="flex items-center justify-center py-6 text-xl font-bold">
                Admin
            </div>

            <Menu
                mode="vertical"
                selectedKeys={[location.pathname]}
                className="border-none"
            >
                {menuItems.map(({ key, label, icon }) => (
                    <Menu.Item key={key} icon={icon}>
                        <Link to={key}>{label}</Link>
                    </Menu.Item>
                ))}
            </Menu>

            {/* Profile & Logout */}
            <div className="absolute bottom-4 left-4 right-4 flex items-center justify-between p-3 bg-gray-100 rounded-lg">
                <div className="flex items-center">
                    <img
                        src={AdminIcon}
                        alt="Profile"
                        className="rounded-full w-10 h-10"
                    />
                    <div className="ml-3">
                        <div className="text-sm font-semibold">Admin</div>
                        <div className="text-xs text-gray-500">admin@fpt.edu.vn</div>
                    </div>
                </div>
                <button
                    onClick={() => logout()}
                    className="p-2 text-gray-500 hover:text-red-500 transition"
                    title="Logout"
                >
                    <LogoutOutlined className="text-lg" />
                </button>
            </div>
        </Sider>
    );
};

export default SideBarAdmin;
