import api from "../instance";

const GetAllBanners = async () => {
    try {
        const response = await api.get('/Banner');
        if (response.status === 200) {
            return response.data;
        }
    } catch (error) {
        console.log(">>> API Get All Banners Error: ", error);
        return [];
    }
}

export { GetAllBanners };
