import api from "../instance";
import { axiosConfigHeader, axiosConfigSendFileHeader } from "../axiosConfigHeader";
import { toast } from "react-toastify";

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
        const response = await api.post('/PreorderCampaign/CreatePreorderCampaign', payload, axiosConfigHeader);
        toast.success("Create successful!");
        return response.data;
    } catch (error) {
        console.log('>>> Api create preorder campaign Error: ', error);
        toast.error("Create failed!");
    }
};

const EditPreorderCampaign = async (payload, id) => {
    try {
        const response = await api.put(`/PreorderCampaign/UpdatePreorderCampaign/${id}`, payload, axiosConfigHeader);
        toast.success("Update successful!");
        return response.data;
    } catch (error) {
        console.log('>>> Api edit preorder campaign Error: ', error);
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

export { GetTheActivePreorderCampaign, CreatePreorderCampaign, EditPreorderCampaign, GetActivePreorderCampaignById };
