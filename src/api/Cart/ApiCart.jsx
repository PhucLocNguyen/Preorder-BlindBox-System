import api from '../instance';
import { axiosConfigHeader } from '../axiosConfigHeader';

const GetAllCart = async () => {
    try {
        var result = await api.get('/Cart', axiosConfigHeader);
        if (result.status === 200) {
            return result.data;
        }
    } catch (error) {
        console.log('>>> Api Get All Order Error: ', error);
        return [];
    }
    
}

const GetPriceInCart = async (requestCreateCart) => {
    try {
        // Gọi API bằng phương thức GET, truyền các tham số thông qua 'params'
        const result = await api.get('/Cart/GetPriceInCart', {
            ...axiosConfigHeader,
            params: requestCreateCart, // Đây là object chứa các tham số cần gửi lên backend
        });
        if (result.status === 200) {
            return result.data;
        }
    } catch (error) {
        console.error('>>> Api GetPriceInCart Error: ', error);
        return null;
    }
};

// API cập nhật số lượng trong giỏ hàng
const UpdateQuantityInCart = async (requestCreateCart) => {
    try {
      const result = await api.put('/Cart', requestCreateCart, axiosConfigHeader);
      if (result.status === 200) {
        return result.data;
      }
    } catch (error) {
      console.error('>>> API UpdateQuantityInCart Error: ', error);
      throw error;
    }
};

// Hàm gọi API ClearAllCart
const ClearAllCart = async () => {
    try {
      const result = await api.put('/Cart/ClearAllCart', null, axiosConfigHeader);
      if (result.status === 200) {
        return result.data;
      }
    } catch (error) {
      console.error("Error clearing cart:", error);
      throw error;
    }
  };

export { GetAllCart, GetPriceInCart, UpdateQuantityInCart, ClearAllCart }