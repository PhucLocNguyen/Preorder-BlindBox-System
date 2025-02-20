import api from '../instance';
import {axiosConfigHeader} from '../axiosConfigHeader';

const GetTheActiveVoucherCampaign = async () => {
    try {
       const respone = await api.get('/VoucherCampaign/GetAllVoucherCampaignBaseUser', axiosConfigHeader);
       return respone.data;
    } catch (error) {
       console.log('>>> Api Get Active voucher campaign Error: ', error)
    }
 }
 
 const CollectActiveVoucherCampaign = async(voucherCampaignId)=>{
   try {
      const respone = await api.post('/UserVouchers', {voucherCampaignId}, axiosConfigHeader);
      return respone.data;
   } catch (error) {
      console.log('>>> Api Collect Active voucher campaign Error: ', error)
   }
 }
 export {GetTheActiveVoucherCampaign, CollectActiveVoucherCampaign}