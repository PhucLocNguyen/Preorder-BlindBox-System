import api from '../instance';
import { axiosConfigHeader } from '../axiosConfigHeader';

const GetTheActiveVoucherCampaign = async () => {
   try {
      const respone = await api.get('/VoucherCampaign/GetAllVoucherCampaignBaseUser', axiosConfigHeader);
      return respone.data;
   } catch (error) {
      console.log('>>> Api Get Active voucher campaign Error: ', error)
   }
}

const CollectActiveVoucherCampaign = async (voucherCampaignId) => {
   try {
      const respone = await api.post('/UserVouchers', { voucherCampaignId }, axiosConfigHeader);
      return respone.data;
   } catch (error) {
      console.log('>>> Api Collect Active voucher campaign Error: ', error)
   }
}
const CreateVoucher = async (payload) => {
   try {
      const response = await api.post('/VoucherCampaign', payload, axiosConfigHeader);
      toast.success("Create successful!");
      return response.data;
   } catch (error) {
      console.log('>>> Api create voucher Error: ', error);
      toast.error("Create failed!");
   }
};

const UpdateVoucher = async (id, payload) => {
   try {
      const response = await api.put(`/VoucherCampaign/${id}`, payload, axiosConfigHeader);
      toast.success("Update successful!");
      return response.data;
   } catch (error) {
      console.log('>>> Api update  preorder campaign Error: ', error);
      toast.error("Update failed!");
   }
};
export { GetTheActiveVoucherCampaign, CollectActiveVoucherCampaign, CreateVoucher, UpdateVoucher }