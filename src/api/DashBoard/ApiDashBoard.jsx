import api from "../instance";
import { axiosConfigHeader, axiosConfigSendFileHeader } from "../axiosConfigHeader";
import { toast } from "react-toastify";
const GetRevenueByTime = async (fromDate, toDate) => {
    try {
        const response = await api.get("/DashBoard/GetRevenueByTime", {
            ...axiosConfigHeader,
            params: { fromDate, toDate },
        });

        const data = response.data;
        console.log("Raw API Response:", data); // In dữ liệu trả về

        return data;
    } catch (error) {
        console.log(">>> API Get Revenue By Time Error: ", error);
        toast.error("Get revenue by time failed!");
        return [];
    }
};


const GetTopThreeCampaign = async (fromDate, toDate) => {
    try {
        const response = await api.get("/DashBoard/GetTopThreeCampaign", {
            ...axiosConfigHeader, // Giữ nguyên cấu hình header
            params: { fromDate, toDate }, // Truyền fromDate và toDate vào query params
        });

        const data = response.data;
        console.log("Raw API Response:", data); // In dữ liệu trả về

        return data;
    } catch (error) {
        console.log(">>> API Get Top Three Campaign Error: ", error);
        toast.error("Get top three campaign failed!");
        return [];
    }
};

export { GetRevenueByTime, GetTopThreeCampaign };