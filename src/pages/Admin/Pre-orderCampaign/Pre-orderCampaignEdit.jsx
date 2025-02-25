import React, { useEffect, useState } from 'react';
import { useParams } from "react-router";
import {
    UpdatePreorderCampaign,
    GetActivePreorderCampaignById,
} from "../../../api/Pre_orderCampaign/ApiPre_orderCampaign";
import { Form, Input, Button, Row, Col, Card, DatePicker, Select, notification } from 'antd';



const Pre_orderCampaignEdit = ({ onSuccess }) => {
    const [form] = Form.useForm();
    const { id } = useParams();
    const [detailPre_orderCampaign, setDetailPre_orderCampaign] = useState({});
    const getDetailPre_orderCampaign_byId = async () => {
        var data = await GetActivePreorderCampaignById(id);
        setDetailPre_orderCampaign(data);
    };

    useEffect(() => {
        getDetailPre_orderCampaign_byId();
    }, []);

    const handleSubmit = async (values) => {
        try {
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

            const result = await UpdatePreorderCampaign(id, payload, config);
            console.log(result);
            console.log("Submitted values:", values);
            notification.success({
                message: 'Success',
                description: 'The campaign has been successfully updated.',
            });
            onSuccess();
        } catch (error) {
            toast.error('Error updating  Preorder Campaign:', error);
            notification.error({
                message: 'Error',
                description: 'There was an error editing the campaign.',
            });
        }
    };

    return (
        <Card className="p-5 bg-white rounded-lg shadow-md" style={{ maxWidth: '600px', margin: '0 auto' }}>
            <h2 className="text-xl font-bold mb-4 text-center">Update Campaign</h2>
            <Form form={form} layout="vertical" onFinish={handleSubmit}
                initialValues={{
                    startDate: detailPre_orderCampaign.startDate,
                    endDate: detailPre_orderCampaign.endDate,
                    type: detailPre_orderCampaign.type,
                }}
            >

                <Row gutter={16}>
                    <Col span={8}>
                        <Form.Item label="Type" name="type" rules={[{ required: true, message: 'Please select a type!' }]}>
                            <Select
                                placeholder="Select a type"
                            >
                                <Option value={0}>TimedPricing</Option>
                                <Option value={1}>BulkOrder</Option>
                            </Select>
                        </Form.Item>
                    </Col>

                    <Col span={8}>
                        <Form.Item label="Start Date" name="startDate" rules={[{ required: true, message: 'Please select a start date!' }]}>
                            <DatePicker style={{ width: '100%' }} showTime format="YYYY-MM-DD HH:mm:ss" />
                        </Form.Item>
                    </Col>

                    <Col span={8}>
                        <Form.Item label="End Date" name="endDate" rules={[{ required: true, message: 'Please select an end date!' }]}>
                            <DatePicker style={{ width: '100%' }} showTime format="YYYY-MM-DD HH:mm:ss" />
                        </Form.Item>
                    </Col>

                </Row>

                <div className="flex justify-center gap-2 mt-4">
                    <Button size="large" onClick={onSuccess}>
                        Cancel
                    </Button>
                    <Button type="primary" htmlType="submit" size="large">
                        Update
                    </Button>
                </div>
            </Form>
        </Card>
    );
}

export default Pre_orderCampaignEdit;
