import React, { useEffect, useState } from "react";
import {
    UpdatePreorderCampaign,
    GetActivePreorderCampaignById,
} from "../../../api/Pre_orderCampaign/ApiPre_orderCampaign";
import { Form, Input, Button, Row, Col, Card, DatePicker, Select, notification } from "antd";
import dayjs from "dayjs";
import { useNavigate } from "react-router";
const { Option } = Select;

const Pre_orderCampaignEdit = ({ preorderCampaignId, onSuccess }) => {
    const [form] = Form.useForm();
    const [detailPre_orderCampaign, setDetailPre_orderCampaign] = useState({});
    const [isEditable, setIsEditable] = useState(true);
    useEffect(() => {
        const fetchCampaignData = async () => {
            try {
                if (!preorderCampaignId) return;

                const data = await GetActivePreorderCampaignById(preorderCampaignId);
                console.log("Dữ liệu từ API:", data);
                onSuccess();
                if (!(data.status === "Pending")) {
                    notification.error({
                        message: "Cannot Update",
                        description: "Cannot update Pre-Order Campaign that is deleted, active, or completed.",
                    });
                    setIsEditable(false);
                    return;
                } else {
                    setIsEditable(true);
                }
                setDetailPre_orderCampaign(data);
                form.setFieldsValue({
                    startDate: data.startDate ? dayjs(data.startDate) : null,
                    endDate: data.endDate ? dayjs(data.endDate) : null,
                    type: data.type === "TimedPricing" ? 0 : 1,
                });

            } catch (error) {
                console.error("Error fetching campaign data:", error);
            }
        };

        fetchCampaignData();
    }, [preorderCampaignId]);

    const handleSubmit = async (values) => {
        try {
            const now = dayjs().startOf('day'); // Ngày hiện tại (bỏ qua giờ, phút, giây)
            const startDate = dayjs(values.startDate).startOf('day'); // Ngày bắt đầu (bỏ qua giờ, phút, giây)

            // Nếu startDate là hôm nay hoặc quá khứ thì không cho phép update
            if (!startDate.isAfter(now)) {
                notification.error({
                    message: "Invalid Start Date",
                    description: "Start date must be in the future.",
                })
                setIsEditable(false);
                return;

            } else {
                setIsEditable(true);
            }


            const config = {
                headers: {
                    'Content-Type': 'application/json'
                }
            };

            const payload = {
                startDate: values.startDate.toISOString(),
                endDate: values.endDate.toISOString(),
                type: values.type,
            };
            const result = await UpdatePreorderCampaign(preorderCampaignId, payload, config);
            console.log("API response:", payload);
            console.log(result);
            onSuccess();
        } catch (error) {
            console.error("Error updating Preorder Campaign:", error);
            notification.error({
                message: "Error",
                description: "There was an error editing the campaign.",
            });
        }
    };

    return (
        <Card className="p-5 bg-white rounded-lg shadow-md" style={{ maxWidth: "600px", margin: "0 auto" }}>
            <h2 className="text-xl font-bold mb-4 text-center">Update Campaign</h2>
            <Form form={form} layout="vertical" onFinish={handleSubmit}>
                <Row gutter={16}>
                    <Col span={8}>
                        <Form.Item label="Type" name="type" rules={[{ required: true, message: "Please select a type!" }]}>
                            <Select placeholder="Select a type" disabled={!isEditable}>
                                <Option value={0}>TimedPricing</Option>
                                <Option value={1}>BulkOrder</Option>
                            </Select>
                        </Form.Item>
                    </Col>

                    <Col span={8}>
                        <Form.Item label="Start Date" name="startDate" rules={[{ required: true, message: "Please select a start date!" }]}>
                            <DatePicker style={{ width: "100%" }} showTime format="YYYY-MM-DD HH:mm:ss" disabled={!isEditable} />
                        </Form.Item>
                    </Col>

                    <Col span={8}>
                        <Form.Item label="End Date" name="endDate" rules={[{ required: true, message: "Please select an end date!" }]}>
                            <DatePicker style={{ width: "100%" }} showTime format="YYYY-MM-DD HH:mm:ss" disabled={!isEditable} />
                        </Form.Item>
                    </Col>
                </Row>

                <div className="flex justify-center gap-2 mt-4">
                    <Button size="large" onClick={onSuccess}>
                        Cancel
                    </Button>
                    <Button type="primary" htmlType="submit" size="large" disabled={!isEditable}>
                        Update
                    </Button>
                </div>
            </Form>
        </Card>
    );
};

export default Pre_orderCampaignEdit;
