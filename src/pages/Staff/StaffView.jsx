
import SlideBarStaff from "./SlideBarStaff";
import { Outlet } from "react-router";
import { useLocation } from "react-router-dom";

const OrderManagement = () => {
    const location = useLocation();
    return (
        <div className="h-screen flex flex-col overflow-hidden">
            <div className="flex flex-1 overflow-hidden gap-4 px-4">
                <div className="h-full">
                    <SlideBarStaff />
                </div>

                {/* Nội dung chính (NavBarStaff) */}
                <div className="flex-1 flex flex-col h-full">
                    <Outlet />
                </div>
            </div>
        </div>
    );
};


export default OrderManagement;