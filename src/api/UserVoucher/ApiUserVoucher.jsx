import api from '../instance';
import {axiosConfigHeader} from '../axiosConfigHeader';

const GetAllUserVoucher = async () => {
    try {
       const respone = await api.get('/UserVouchers', axiosConfigHeader);
       return respone.data;
    } catch (error) {
       console.log('>>> Api Get All User Voucher Error: ', error)
    }
 }

export {GetAllUserVoucher}