import React from 'react';
import { useParams } from 'react-router-dom';
const UserManagementDetails = () => {
    const { id } = useParams();
    return (
        <div>{id}</div>
    )
}

export default UserManagementDetails