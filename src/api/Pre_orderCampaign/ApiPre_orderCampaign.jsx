import api from "../instance";
import { axiosConfigHeader, axiosConfigSendFileHeader } from "../axiosConfigHeader";
import { toast } from "react-toastify";
import { notification } from "antd";

const GetTheActivePreorderCampaign = async (pageSize, pageIndex) => {
    try {
        const response = await api.get("/PreorderCampaign", {
            ...axiosConfigHeader,
            params: { pageSize, pageIndex },
        });
        const data = response.data;
        const paginationHeader = response.headers["x-pagination"];
        console.log("Pagination Header:", paginationHeader);

        const pagination = paginationHeader ? JSON.parse(paginationHeader) : null;

        return { data, pagination };
    } catch (error) {
        console.log(">>> Api Get Active Preorder Campaign Error: ", error);
        toast.error("Get active preorder campaign failed!");

        return { data: [], pagination: null };
    }
};

const CreatePreorderCampaign = async (payload) => {
    try {
        const response = await api.post('/PreorderCampaign/CreatePreorderCampaignWithMilestone', JSON.stringify(payload), axiosConfigHeader);
        toast.success("Create successful!");
        return response.data;
    } catch (error) {
        console.log('>>> Api create preorder campaign Error: ', error);
        toast.error("Create failed!");
    }
};

const UpdatePreorderCampaign = async (id, payload) => {
    try {
        const response = await api.put(`/PreorderCampaign/UpdatePreorderCampaign/${id}`, payload, axiosConfigHeader);
        toast.success("Update successful!");
        return response.data;
    } catch (error) {
        console.log('>>> Api update  preorder campaign Error: ', error);
        toast.error("Update failed!");
    }
};

const GetActivePreorderCampaignById = async (id) => {
    try {
        const response = await api.get(`/PreorderCampaign/${id}`, axiosConfigHeader);
        return response.data;
    } catch (error) {
        console.log('>>> Api get active preorder campaign by id Error: ', error);
        toast.error("Get active preorder campaign by id failed!");
    }
};

const GetActivePreorderCampaignBySlug = async (slug) => {
    try {
        const respone = await api.get(`/PreorderCampaign/campaign/${slug}`, axiosConfigHeader);
        return respone.data;
    } catch (error) {
        console.log('>>> Api get active preorder campaign by id Error: ', error)
        toast.error("Get active preorder campaign by slug failed!");
    }
}

const DeletePendingCampaign = async(id)=>{
    try {

        const response = await api.delete(`/PreorderCampaign/${id}`, axiosConfigHeader);

        if (response.status === 200) {
            notification.success({
                message: "Success",
                description: "The campaign has been successfully deleted.",
            });
            return response.status;
        } else {
            throw new Error("Failed to delete campaign.");
        }
    } catch (error) {
        console.error("Error deleting Pre_orderCampaign:", error);
        notification.error({
            message: "Error",
            description: "Failed to delete the campaign.",
        });
    }
}

export {
    GetTheActivePreorderCampaign, CreatePreorderCampaign,
    UpdatePreorderCampaign, GetActivePreorderCampaignBySlug, GetActivePreorderCampaignById,DeletePendingCampaign

};

