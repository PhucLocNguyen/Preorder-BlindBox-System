import React from 'react';
import { useParams } from 'react-router-dom';
import {
    Button
} from 'antd';
import {
    PlusOutlined
} from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
const VoucherDetails = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const handleUpdateVoucher = () => {
        navigate(`/admin/voucher/update/${id}`);
    };
    return (
        <div>
            <Button className="bg-blue-500 text-white px-4 py-2 rounded mt-8" icon={<PlusOutlined />} onClick={handleUpdateVoucher}>
                Update  Voucher
            </Button>
            <h1>Voucher Details</h1>
            <p>Voucher ID: {id}</p>
            {/* Add more details about the voucher here */}
        </div>
    );
};

export default VoucherDetails;
