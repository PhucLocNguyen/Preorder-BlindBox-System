
import React, { useEffect } from 'react';
import { Form, Input, Button, Row, Col, Card, DatePicker, Select } from 'antd';

const Pre_orderCampaignCreate = ({ onSuccess, selectedProduct }) => {
    const [form] = Form.useForm();

    useEffect(() => {
        console.log("Received selectedProduct in Create Modal:", selectedProduct);
        form.setFieldsValue({ 'blind-boxID': selectedProduct });
    }, [selectedProduct, form]);

    const onFinish = (values) => {
        console.log("Submitted values:", values);
        console.log("Selected Product Key:", selectedProduct);
        onSuccess();
    };

    return (
        <Card className="p-5 bg-white rounded-lg shadow-md" style={{ maxWidth: '600px', margin: '0 auto' }}>
            <h2 className="text-xl font-bold mb-4 text-center">Add New Campaign</h2>
            <Form form={form} layout="vertical" onFinish={onFinish}>
                <Row gutter={16}>
                    <Col span={6}>
                        <Form.Item
                            label="Blind-box ID"
                            name="blind-boxID"
                            rules={[{ required: true, message: 'Please enter Blind-box ID!' }]}
                        >
                            <Input readOnly />
                        </Form.Item>
                    </Col>
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
                    <Col span={10}>
                        <Form.Item label="Status" name="status" rules={[{ required: true, message: 'Please select a status!' }]}>
                            <Select
                                placeholder="Select a status"
                                options={[
                                    { value: '0', label: 'Pending' },
                                    { value: '1', label: 'Active' },
                                    { value: '2', label: 'Completed' },
                                    { value: '3', label: 'Canceled' },
                                ]}
                            />
                        </Form.Item>
                    </Col>
                </Row>
                <Row gutter={16}>
                    <Col span={12}>
                        <Form.Item label="Start Date" name="StartedDate" rules={[{ required: true, message: 'Please select a start date!' }]}>
                            <DatePicker style={{ width: '100%' }} />
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item label="End Date" name="EndedDate" rules={[{ required: true, message: 'Please select an end date!' }]}>
                            <DatePicker style={{ width: '100%' }} />
                        </Form.Item>
                    </Col>
                </Row>
                <Row gutter={16}>
                    <Col span={12}>
                        <Form.Item label="Create Date" name="CreatedDate">
                            <DatePicker style={{ width: '100%' }} />
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item label="Update  Date" name="UpdatedDate">
                            <DatePicker style={{ width: '100%' }} />
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
