import api from '../instance';
import axiosConfigHeader from '../AxiosConfigHeader';

const GetTheActiveVoucherCampaign = async ({ formData: payload}) => {
    try {
       const respone = await api.get('/GetActiveVoucherCampaign', payload, axiosConfigHeader);
       return respone.data;
    } catch (error) {
       console.log('>>> Api Get Active voucher campaign Error: ', error)
    }
 }
 