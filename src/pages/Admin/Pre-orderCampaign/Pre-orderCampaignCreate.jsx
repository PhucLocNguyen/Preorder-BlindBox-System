import { React, useEffect } from 'react';
import { Form, Input, Button, Row, Col, Card, DatePicker, Select, notification } from 'antd';
import { CreatePreorderCampaign } from "../../../api/Pre_orderCampaign/ApiPre_orderCampaign";
import { toast } from 'react-toastify';
const { Option } = Select;

const Pre_orderCampaignCreate = ({ onSuccess }) => {
    const [form] = Form.useForm();

    const handleSubmit = async (values) => {

        const payload = {
            blindBoxId: Number(values.blindBoxId),
            startDate: values.startDate.toISOString(),
            endDate: values.endDate.toISOString(),
            type: values.type
        };

        try {
            const config = {
                headers: {

                    'Content-Type': 'application/json'
                }
            };
            const result = await CreatePreorderCampaign(payload, config);
            console.log("Submitted values:", payload);
            console.log("result", result);

            if (result?.status === 200) {
                notification.success({
                    message: 'Success',
                    description: 'The campaign has been successfully created.',
                });
                onSuccess();
            }

            onSuccess();

        } catch (error) {
            toast.error('Error creating Preorder Campaign:', error);
            notification.error({
                message: 'Error',
                description: 'There was an error creating the campaign.',
            });
        }
    };


    return (
        <Card className="p-5 bg-white rounded-lg shadow-md" style={{ maxWidth: '600px', margin: '0 auto' }}>
            <h2 className="text-xl font-bold mb-4 text-center">Add New Campaign</h2>
            <Form form={form} layout="vertical" onFinish={handleSubmit}>
                <Row gutter={16}>
                    <Col span={12}>
                        <Form.Item
                            label="Blind-box ID"
                            name="blindBoxId"

                            rules={[{ required: true, message: 'Please enter Blind-box ID!' }]}
                        >
                            <Input type="number" min={1} />
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item label="Type" name="type" rules={[{ required: true, message: 'Please select a type!' }]}>
                            <Select
                                placeholder="Select a type"
                            >
                                <Option value={0}>TimedPricing</Option>
                                <Option value={1}>BulkOrder</Option>
                            </Select>


                        </Form.Item>
                    </Col>
                </Row>
                <Row gutter={16}>
                    <Col span={12}>
                        <Form.Item label="Start Date" name="startDate" rules={[{ required: true, message: 'Please select a start date!' }]}>
                            <DatePicker style={{ width: '100%' }} showTime format="YYYY-MM-DD HH:mm:ss" />
                        </Form.Item>
                    </Col>
                    <Col span={12}>
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
                        Create
                    </Button>
                </div>
            </Form>
        </Card>
    );
};

export default Pre_orderCampaignCreate;
