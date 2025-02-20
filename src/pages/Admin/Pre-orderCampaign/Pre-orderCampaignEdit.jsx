import React, { useEffect } from 'react';
import { Form, Input, Button, Row, Col, Card, DatePicker, Select, notification } from 'antd';
import { EditPreorderCampaign } from "../../../api/Pre_orderCampaign/ApiPre_orderCampaign";

const Pre_orderCampaignEdit = ({ onSuccess, selectedProduct }) => {
    const [form] = Form.useForm();

    const handleSubmit = async (values) => {


        const payload = {
            startedDate: values.startedDate.toISOString(),
            endedDate: values.endedDate.toISOString(),
            type: values.type,
        };

        var result = await EditPreorderCampaign(selectedProduct, payload);
        console.log(result);
        console.log("Submitted values:", values);
        notification.success({
            message: 'Success',
            description: 'The campaign has been successfully updated.',
        });
        onSuccess();
    };

    return (
        <Card className="p-5 bg-white rounded-lg shadow-md" style={{ maxWidth: '600px', margin: '0 auto' }}>
            <h2 className="text-xl font-bold mb-4 text-center">Update Campaign</h2>
            <Form form={form} layout="vertical" onFinish={handleSubmit}>
                <Row gutter={16}>
                    <Col span={8}>
                        <Form.Item label="Type" name="type" rules={[{ required: true, message: 'Please select a type!' }]}>
                            <Select
                                placeholder="Select a type"
                                options={[
                                    { value: '0', label: 'TimedPricing' },
                                    { value: '1', label: 'BulkOrder' },
                                ]}
                            />
                        </Form.Item>
                    </Col>

                    <Col span={8}>
                        <Form.Item label="Start Date" name="startedDate" rules={[{ required: true, message: 'Please select a start date!' }]}>
                            <DatePicker style={{ width: '100%' }} showTime format="YYYY-MM-DD HH:mm:ss" />
                        </Form.Item>
                    </Col>

                    <Col span={8}>
                        <Form.Item label="End Date" name="endedDate" rules={[{ required: true, message: 'Please select an end date!' }]}>
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
