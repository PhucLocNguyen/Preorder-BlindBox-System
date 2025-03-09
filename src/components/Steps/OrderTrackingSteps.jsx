import React from "react";
import { Steps } from "antd";

const { Step } = Steps;

// Giả sử bạn có trạng thái của đơn hàng dưới dạng chuỗi hoặc số,
// ở đây chúng ta sẽ ánh xạ trạng thái thành chỉ số của bước hiện tại.
// placed, proccessing, delivering, delivered, Cancel

const OrderTrackingSteps = ({
  status,
  preorderCampaignType = "TimedPricing",
}) => {
  // Xác định bước hiện tại dựa trên trạng thái đơn hàng
  if (preorderCampaignType === "TimedPricing") {
    const statusToStepIndex = {
      placed: 0,
      proccessing: 1,
      delivering: 2,
      delivered: 3,
    };
    const currentStep = statusToStepIndex[status] || 0;
    return (
        <Steps current={currentStep}>
          <Step title="Ordered" description="Order has been placed." />
          <Step title="Processed" description="Order is being processed." />
          <Step title="Delivering" description="Order is delivering." />
          <Step title="Delivered" description="Order has been delivered." />
        </Steps>
    );
  } else {
    if (status === "cancel") {
      const statusToStepIndex = {
        placed: 0,
        processing: 1,
        cancel: 2,
      };
      const currentStep = statusToStepIndex[status] || 0;
      return (
        <Card title="Tình trạng của đơn hàng" style={{ marginTop: 24 }}>
          <Steps current={currentStep}>
            <Step title="Ordered" description="Order has been placed." />
            <Step title="Processed" description="Order is being processed." />
            <Step title="Rejected" description="Order has be cancel." />
          </Steps>
        </Card>
      );
    } else {
      const statusToStepIndex = {
        placed: 0,
        processing: 1,
        delivering: 2,
        delivered: 3,
      };
      const currentStep = statusToStepIndex[status] || 0;
      return (
        <Card title="Tình trạng của đơn hàng" style={{ marginTop: 24 }}>
          <Steps current={currentStep}>
            <Step title="Ordered" description="Order has been placed." />
            <Step title="Processed" description="Order is being processed." />
            <Step title="Processed" description="Order is being processed." />
            <Step title="Delivering" description="Order is delivering." />
            <Step title="Delivered" description="Order has been delivered." />
          </Steps>
        </Card>
      );
    }
  }
};

export default OrderTrackingSteps;
