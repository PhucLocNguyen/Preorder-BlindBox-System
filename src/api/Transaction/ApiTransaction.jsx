import { toast } from "react-toastify";
import api from "../instance";
import { axiosConfigHeader } from "../axiosConfigHeader";
import { message } from "antd";

const GetTheWithdrawTransactionRequest = async (pageSize, pageIndex) => {
    try {
        const response = await api.get("/Transaction/withdrawals", {
            ...axiosConfigHeader,
            params: { pageSize, pageIndex },
        });
        const data = response.data;
        const paginationHeader = response.headers["x-pagination"];
        const pagination = paginationHeader ? JSON.parse(paginationHeader) : null;

        return { data, pagination };
    } catch (error) {
        console.log(">>> Api Get Withdraw Transaction Request Error: ", error);
        toast.error("Get Withdraw Transaction list failed!");

        return { data: [], pagination: null };
    }
};
const GetWithdrawTransactionDetailRequest = async(transactionId)=>{
    try {
        const response = await api.get(`/Transaction/withdrawals/${transactionId}`, axiosConfigHeader);
        return response.data;
    } catch (error) {
        console.log('>>> Api get detail withdraw transaction by id Error: ', error);
        toast.error("Get detail withdraw transaction by id failed!");
    }
}
const ApproveWithdrawTransactionRequest = async(transactionId)=>{
    try {
        const response = await api.post(`/Transaction/withdrawals/${transactionId}/approval`, axiosConfigHeader);
        message.success("Chấp nhận rút tiền thành công");
        return response.data;
    } catch (error) {
        console.log('>>> Api approve withdraw transaction by id Error: ', error);
        toast.error("Approve withdraw transaction by id failed!");
    } 
}
export {GetTheWithdrawTransactionRequest, GetWithdrawTransactionDetailRequest,ApproveWithdrawTransactionRequest};