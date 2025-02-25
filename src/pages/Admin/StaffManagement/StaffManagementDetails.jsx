import React from 'react';
import { useParams } from 'react-router-dom';
const StaffManagementDetails = () => {
    const { id } = useParams();
    return (
        <div>{id}</div>
    )
}

export default StaffManagementDetails