import { axiosConfigHeader } from "../axiosConfigHeader"
import api from "../instance"

const GetUserVoucherById = async (userVoucherId) => {
    try {
        var result = await api.get(`/UserVouchers/${userVoucherId}`, axiosConfigHeader)
        if (result.status === 200) return result.data
    } catch (error) {
        console.log('>>> Api Get User Voucher By ID Error ', error);
        return [];
    }
}
export { GetUserVoucherById }