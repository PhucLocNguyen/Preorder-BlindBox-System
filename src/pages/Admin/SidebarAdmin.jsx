import React from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
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

const { Sider } = Layout;

const SideBarAdmin = () => {
    const location = useLocation();
    const navigate = useNavigate();

    const menuItems = [
        { key: "/admin/dashboard", label: "Dashboard", icon: <DashboardOutlined /> },
        { key: "/admin/staffmanagement", label: "Staff", icon: <UserOutlined /> },
        { key: "/admin/history-transactions", label: "History Transactions", icon: <HistoryOutlined /> },
        { key: "/admin/pre-ordercampaign", label: "Preorder Campaign", icon: <ShoppingCartOutlined /> },
        { key: "/admin/voucher", label: "Voucher", icon: <TagOutlined /> },
        { key: "/admin/banner-management", label: "Banner Management", icon: <PictureOutlined /> },
        { key: "/admin/preordercampaignApproval", label: "Campaign approval", icon: <CheckSquareOutlined /> }

    ];

    return (
        <Sider width={220} className="h-full shadow-lg bg-white">
            <div className="flex items-center justify-center py-6 text-xl font-bold">
                Admin Management
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
                        src="https://placehold.co/40x40"
                        alt="Profile"
                        className="rounded-full w-10 h-10"
                    />
                    <div className="ml-3">
                        <div className="text-sm font-semibold">Admin</div>
                        <div className="text-xs text-gray-500">admin@fpt.edu.vn</div>
                    </div>
                </div>
                <button
                    onClick={() => navigate("/login")}
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
