import SignalRService from "./SignalRService";

class PreorderCampaignDetailService extends SignalRService {
    constructor() {
        super(`${import.meta.env.VITE_PREORDERBLINDBOX_API_Hub_URL}/OrderInCampaign`);
    }

    addOrderUpdateListener(callback) {
        this.hubConnection.on("ReceiveOrderUpdate", (orderInfo) => {
            console.log("Tổng đơn mới của chiến dịch là ", orderInfo);
            callback(orderInfo); 
        });
    }
}

export default new PreorderCampaignDetailService();
