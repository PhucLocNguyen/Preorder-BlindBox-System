import React from "react";
import { Form, Input, Button, Row, Col, Card, Select } from "antd";

const { Option } = Select;

const PreorderMilestone = () => {
    return (
        <div className="p-5 bg-gray-100 min-h-screen flex justify-center items-center">
            <Card className="bg-white p-5 rounded-lg shadow-md w-full max-w-2xl">
                <h2 className="text-xl font-semibold mb-4 text-center">
                    Add MileStone for Pre-orderCampaign
                </h2>

                <Form layout="vertical">
                    <Row gutter={16}>
                        <Col span={12}>
                            <Form.Item label="Pre-orderCampaignID *">
                                <Input type="number" min={0} placeholder="Enter Pre-orderCampaignID" />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item label="Milestone Number *">
                                <Select placeholder="Choose Milestone Number">
                                    <Option value="1">1</Option>
                                    <Option value="2">2</Option>
                                    <Option value="3">3</Option>
                                </Select>
                            </Form.Item>
                        </Col>
                    </Row>

                    <Row gutter={16}>
                        <Col span={12}>
                            <Form.Item label="Quantity *">
                                <Input type="number" min={0} placeholder="Enter Quantity" />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item label="Price *">
                                <Input placeholder="Enter Price" />
                            </Form.Item>
                        </Col>
                    </Row>
                </Form>

                <div className="flex justify-center space-x-4 mt-6">
                    <Button type="primary">Add</Button>
                    <Button>Save product</Button>
                    <Button type="default">Schedule</Button>
                </div>
            </Card>
        </div>
    );
};

export default PreorderMilestone;
