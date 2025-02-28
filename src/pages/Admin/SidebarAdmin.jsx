import React from "react";
import LogoutButton from "../../assets/Logout/logoutbutton.jpg";
import { Link, useLocation } from "react-router-dom";
import { UserOutlined } from "@ant-design/icons";
import { Row, Col } from "antd";
import VoucherIcon from "../../assets/Admin/VoucherIcon.png";
import DashboardIcon from "../../assets/Admin/DashboardIcon.jpg";
import PreOrderIcon from "../../assets/Admin/Pre-orderIcon.png";
import HistoryTransactions from "../../assets/Admin/HistoryTransactions.png";
import { useNavigate } from "react-router-dom";
const SideBarAdmin = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const Logout = () => { navigate("/login") }
    return (
        <div className="bg-white w-64 h-full shadow-lg p-4 flex flex-col">
            {/* Logo */}
            <div className="flex items-center mb-8">
                <Link to="/admin" className="flex items-center w-full">
                    <i className="fas fa-home text-2xl text-black"></i>
                    <span className="ml-2 text-xl font-bold">BRESS</span>
                </Link>
            </div>

            {/* Sidebar Menu */}
            <ul className="space-y-4">
                {/* Admin Management */}

                <li className={`flex items-center px-4 py-3 rounded-lg transition duration-300 ${location.pathname === "/admin/usermanagement" || location.pathname === "/admin"
                    ? "bg-blue-500 text-white"
                    : "text-gray-700 hover:bg-blue-100 hover:text-blue-600"
                    }`}>
                    <Link to="/admin/usermanagement" className="flex items-center w-full">
                        <Row gutter={12} align="middle">

                            <Col span={10} className="flex justify-center">
                                <UserOutlined style={{ fontSize: '30px' }} />
                            </Col>
                            <Col span={14}>
                                <i className="fas fa-tachometer-alt"></i>
                                <span className="font-medium text-lg">Staff</span>
                            </Col>

                        </Row>
                    </Link>

                </li>


                <li className={`flex items-center px-4 py-3 rounded-lg transition duration-300 ${location.pathname === "/admin/history-transactions"
                    ? "bg-blue-500 text-white"
                    : "text-gray-700 hover:bg-blue-100 hover:text-blue-600"
                    }`}>
                    <Link to="/admin/history-transactions" className="flex items-center w-full">
                        <Row gutter={12} align="middle">
                            <Col span={5} className="flex justify-center">
                                <img src={HistoryTransactions} alt="History Transactions Icon" className="w-8 h-8 no-border" />
                            </Col>
                            <Col span={16}>
                                <i className="fas fa-tachometer-alt"></i>
                                <span className="font-medium text-lg">HistoryTransactions</span>
                            </Col>
                        </Row>
                    </Link>
                </li>


                {/* Pre-Order Campaign */}

                <li className={`flex items-center px-4 py-3 rounded-lg transition duration-300 ${location.pathname === "/admin/pre-ordercampaign"
                    ? "bg-blue-500 text-white"
                    : "text-gray-700 hover:bg-blue-100 hover:text-blue-600"
                    }`}>
                    <Link to="/admin/pre-ordercampaign" className="flex items-center w-full">
                        <Row gutter={12} align="middle">
                            <Col span={5} className="flex justify-center">
                                <img src={PreOrderIcon} alt="Pre-Order Icon" className="w-8 h-8" />
                            </Col>
                            <Col span={18}>
                                <i className="fas fa-tachometer-alt"></i>
                                <span className="font-medium text-lg">PreorderCampaign</span>
                            </Col>
                        </Row>
                    </Link>
                </li>


                {/* Voucher Management */}

                <li className={`flex items-center px-4 py-3 rounded-lg transition duration-300 ${location.pathname === "/admin/voucher"
                    ? "bg-blue-500 text-white"
                    : "text-gray-700 hover:bg-blue-100 hover:text-blue-600"
                    }`}>
                    <Link to="/admin/voucher" className="flex items-center w-full">
                        <Row gutter={12} align="middle">
                            <Col span={8} className="flex justify-center">
                                <img src={VoucherIcon} alt="Voucher Icon" className="w-10 h-8" />
                            </Col>
                            <Col span={16}>
                                <i className="fas fa-tachometer-alt"></i>
                                <span className="font-medium text-lg">Voucher</span>
                            </Col>
                        </Row>
                    </Link>
                </li>


                {/* Dashboard */}

                <li className={`flex items-center px-4 py-3 rounded-lg transition duration-300 ${location.pathname === "/admin/dashboard"
                    ? "bg-blue-500 text-white"
                    : "text-gray-700 hover:bg-blue-100 hover:text-blue-600"
                    }`}>
                    <Link to="/admin/dashboard" className="w-full">
                        <Row gutter={12} align="middle">
                            <Col span={6} className="flex justify-center">
                                <img src={DashboardIcon} alt="Dashboard Icon" className="w-10 h-8" />
                            </Col>
                            <Col span={16}>
                                <i className="fas fa-tachometer-alt"></i>
                                <span className="font-medium text-lg">Dashboard</span>
                            </Col>
                        </Row>
                    </Link>
                </li>

            </ul>

            {/* Profile & Logout */}
            <div className="mt-auto flex items-center justify-between w-full p-2">
                <div className="flex items-center">
                    <img
                        src="https://placehold.co/40x40"
                        alt="Profile"
                        className="rounded-full w-10 h-10"
                    />
                    <div className="ml-2">
                        <div className="text-sm font-semibold">Graham Cooper</div>
                        <div className="text-xs text-gray-500">graham1234@bress.com</div>
                    </div>
                </div>

                {/* Logout Button */}
                <button
                    onClick={() => Logout(alert("Logout"))}
                    className="ml-4 p-2 text-gray-500 hover:text-red-500 transition"
                    title="Logout"
                >
                    <img src={LogoutButton} alt="Logout Icon" className="w-6 h-6" />
                </button>
            </div>
        </div>
    );
};

export default SideBarAdmin;
