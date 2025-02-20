import React from 'react'
import { useParams } from 'react-router-dom';
const Pre_orderCampaignDetails = () => {
    const { id } = useParams();
    return (
        <div>{id}</div>
    )
}

export default Pre_orderCampaignDetails