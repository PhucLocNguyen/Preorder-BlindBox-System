import api from '../instance';
import {axiosConfigHeader} from '../axiosConfigHeader';
import { param } from 'framer-motion/client';

const GetAllOrder = async () => {
    try {
        const response = await api.get(`/Order`, axiosConfigHeader);
        if (response.status === 200) {
            return response.data;
        }
    } catch (error) {
        console.log('>>> Api Get all orders Error: ', error)
    }
}

export { GetAllOrder }