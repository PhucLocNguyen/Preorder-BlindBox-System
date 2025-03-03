import api from '../instance';
import { axiosConfigHeader } from '../axiosConfigHeader';
import { toast } from "react-toastify";

const CreateStaffAccount = async (payload) => {
    try {
        const response = await api.post('/Authen/RegisterStaff', payload, axiosConfigHeader);
        return response.data;
    } catch (error) {
        console.log('>>> Api create voucher Error: ', error);
    }
};

const GetAllStaff = async () => {
    try {
        const response = await api.get('User/GetAllStaff', axiosConfigHeader);
        return response.data;
    } catch (error) {
        console.log('>>> Api get active all staff Error: ', error);
        toast.error("Get active all staff failed!");
    }
};

const DeleteStaff = async (staffId) => {
    try {
        const respone = await api.delete(`/User/${staffId}`, axiosConfigHeader);
        return respone.data;
    } catch (error) {
        console.log('>>> Api Delete voucher campaign Error: ', error)
    }
}



export {
    CreateStaffAccount, GetAllStaff, DeleteStaff
}