import React from "react";
import LogoutButton from "../../assets/Logout/LogoutButton.jpg";
import { Link, useLocation } from "react-router-dom";


const SideBarStaff = (props) => {
    const location = useLocation();
    return (
        //<SlideBarStaff />
        <div className="bg-white w-64 h-full shadow-lg p-4 flex flex-col">
            <div className="flex items-center mb-8">
                <Link to="/staff" className="flex items-center w-full">
                    <i className="fas fa-home text-2xl text-black"></i>
                    <span className="ml-2 text-xl font-bold">BRESS</span>
                </Link>

            </div>
            <ul className="space-y-4">
                <li className={`flex items-center ${location.pathname === "/staff/products" || location.pathname === "/staff" ? `text-white bg-black rounded-lg p-2` : `text-gray-600`} `}>
                    <Link to="/staff/products" className="flex items-center w-full">
                        <i className="fas fa-tachometer-alt"></i>
                        <span className="ml-2">Products</span>
                    </Link>

                </li>
                <li className={`flex items-center ${location.pathname.startsWith("/staff/orders") ? `text-white bg-black rounded-lg p-2` : `text-gray-600`} `}>
                    <Link to="/staff/orders" className="flex items-center w-full">
                        <i className="fas fa-folder"></i>
                        <span className="ml-2">Orders</span>
                    </Link>

                </li>
                <li className={`flex items-center ${location.pathname === "/staff/notifications" ? `text-white bg-black rounded-lg p-2` : `text-gray-600`} `}>
                    <Link to="/staff/notifications" className="flex items-center w-full">
                        <i className="fas fa-bell"></i>
                        <span className="ml-2">Notifications</span>
                        <span className="ml-auto bg-green-200 text-green-800 text-xs font-semibold px-2 py-1 rounded-full">2</span>
                    </Link>

                </li>
            </ul>
            {/* <div className="mt-auto flex items-center">
                <img
                    src="https://placehold.co/40x40"
                    alt="Profile picture of Emily Jonson"
                    className="rounded-full"
                    width="40"
                    height="40"
                />
                <div className="ml-2">
                    <div className="text-sm font-semibold">Emily Jonson</div>
                    <div className="text-xs text-gray-500">jonson@bress.com</div>
                </div>
            </div> */}
            <div className="mt-auto flex items-center justify-between w-full p-2">
                {/* Profile Info */}
                <div className="flex items-center">
                    <img
                        src="https://placehold.co/40x40"
                        alt="Profile picture of Emily Jonson"
                        className="rounded-full"
                        width="40"
                        height="40"
                    />
                    <div className="ml-2">
                        <div className="text-sm font-semibold">Emily Jonson</div>
                        <div className="text-xs text-gray-500">jonson@bress.com</div>
                    </div>
                </div>

                {/* Logout Icon */}
                <button
                    onClick={() => alert("Logout")}
                    className="ml-4 p-2 text-gray-500 hover:text-red-500 transition"
                    title="Logout"
                >
                    <img src={LogoutButton} alt="Logout Icon" className="w-6 h-6" />
                </button>
            </div>
        </div >
    );
}
export default SideBarStaff;