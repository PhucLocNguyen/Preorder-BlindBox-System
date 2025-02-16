import React from 'react';
import { useParams } from 'react-router-dom';

const VoucherDetails = () => {
    const { id } = useParams();

    return (
        <div>
            <h1>Voucher Details</h1>
            <p>Voucher ID: {id}</p>
            {/* Add more details about the voucher here */}
        </div>
    );
};

export default VoucherDetails;
