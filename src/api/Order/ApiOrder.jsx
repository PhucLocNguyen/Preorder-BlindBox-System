import api from '../instance';
import { axiosConfigHeader } from '../axiosConfigHeader';
import { param } from 'framer-motion/client';

const GetAllOrder = async (pageIndex, pageSize, searchKeyWords, orderBy) => {
    try {
        const params = {};
        if (pageIndex !== undefined) {
            params.pageIndex = pageIndex;
        }
        if (pageSize !== undefined) {
            params.pageSize = pageSize;
        }
        if (searchKeyWords !== undefined) {
            params.searchKeyWords = searchKeyWords;
        }
        params.orderBy = orderBy;
        var result = await api.get('/Order', { ...axiosConfigHeader, params: params });
        if (result.status === 200) {
            return result.data;
        }
    } catch (error) {
        console.log('>>> Api Get All Order Error: ', error);
        return [];
    }
}

const GetOrderById = async (orderId) => {
    try {
        var result = await api.get(`/Order/${orderId}`, axiosConfigHeader)
        if (result.status === 200) {
            return result.data;
        }
    } catch (error) {
        console.log('>>> Api Get Order Id Error ', error);
        return null;
    }
}

export { GetAllOrder, GetOrderById }