import React from 'react'
import { Form, Input, Button, Row, Col, Card, DatePicker, Select, message } from 'antd';
const Pre_orderCampaignEdit = ({ onSuccess }) => {
    const onFinish = (values) => {
        onSuccess();
    };
    return (
        <Card
            className="p-5 bg-white rounded-lg shadow-md"
            style={{ maxWidth: '600px', margin: '0 auto' }}
        >
            <h2 className="text-xl font-bold mb-4 text-center">Update Campaign</h2>
            <Form layout="vertical" onFinish={onFinish}>
                <Row gutter={16}>
                    <Col span={6}>
                        <Form.Item label="Blind-boxID" name="blind-boxID"
                            rules={[{ required: true, message: 'Please enter Blind-box ID!' }]}>
                            <Input />
                        </Form.Item>
                    </Col>
                    <Col span={8}>
                        <Form.Item label="Type" name="type" rules={[{ required: true, message: 'Please enter type' }]}>
                            <Select

                                placeholder="Select a type"
                                filterOption={(input, option) =>
                                    (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
                                }
                                options={[
                                    { value: '0', label: 'TimedPricing' },
                                    { value: '1', label: 'BulkOrder' },

                                ]}
                            />
                        </Form.Item>
                    </Col>
                    <Col span={10}>
                        <Form.Item label="Status" name="status" rules={[{ required: true, message: 'Please enter status!' }]}>
                            <Select

                                placeholder="Select a status"
                                filterOption={(input, option) =>
                                    (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
                                }
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
                        <Form.Item label="Start Date" name="StartedDate">
                            <DatePicker style={{ width: '100%' }} />
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item label="End Date" name="EndedDate">
                            <DatePicker style={{ width: '100%' }} />
                        </Form.Item>
                    </Col>

                </Row>
                <div className="flex justify-center gap-2 mt-4">
                    <Button
                        size="large"
                        onClick={onSuccess} // Đóng Modal khi bấm Cancel
                    >
                        Cancel
                    </Button>
                    <Button
                        type="primary"
                        htmlType="submit"
                        size="large"
                    >
                        Create
                    </Button>
                </div>
            </Form>
        </Card>
    );
}

export default Pre_orderCampaignEdit