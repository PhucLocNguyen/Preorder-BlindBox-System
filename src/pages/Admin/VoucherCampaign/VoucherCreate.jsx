import React, { useState } from 'react';
import { Button, Form, Input, Select, DatePicker, Row, Col, Slider } from "antd";

const { Option } = Select;

const VoucherCreate = () => {
    const [loading, setLoading] = useState(false);
    const [form] = Form.useForm();

    const handleSubmit = async (values) => {
        const formData = {
            Name: values.name,
            CreateDate: values.createDate,
            StartDate: values.startDate,
            EndDate: values.endDate,
            Quantity: values.quantity,
            TakenQuantity: values.takenQuantity,
            MaximumUserCanGet: values.maximumUserCanGet,
            PercentDiscount: values.percentDiscount,
            MaximumMoneyDiscount: values.maximumMoneyDiscount,
            MaximumDiscount: values.maximumDiscount,
            Status: values.status
        };

        // Example: Replace this with your API call
        console.log(formData);

        // Reset form after submission
        form.resetFields();
    };

    return (
        <div className="w-full min-h-screen mx-auto mt-5 p-5 bg-white shadow-lg rounded-lg">
            <h2 className="text-xl font-bold mb-4">Create New Voucher</h2>
            <Form form={form} layout="vertical" onFinish={handleSubmit}>
                {/* Name */}
                <Row gutter={16}>
                    <Col span={16}>
                        <Form.Item
                            label="Name"
                            name="name"
                            rules={[{ required: true, message: "Please enter the name!" }]}
                        >
                            <Input placeholder="Enter voucher name" />
                        </Form.Item>
                    </Col>
                    <Col span={8}>
                        {/* Status */}
                        <Form.Item
                            label="Status"
                            name="status"
                            rules={[{ required: true, message: "Please select the status!" }]}
                        >
                            <Select placeholder="Select status">
                                <Option value="Active">Active</Option>
                                <Option value="Inactive">Inactive</Option>
                                <Option value="Expired">Expired</Option>
                            </Select>
                        </Form.Item>
                    </Col>
                </Row>
                {/* Create Date */}
                <Row gutter={16}>
                    <Col span={8}>
                        <Form.Item
                            label="Create Date"
                            name="createDate"
                            rules={[{ required: true, message: "Please select the create date!" }]}
                        >
                            <DatePicker style={{ width: '100%' }} />
                        </Form.Item>
                    </Col>
                    {/* Start Date */}
                    <Col span={8}>
                        <Form.Item
                            label="Start Date"
                            name="startDate"
                            rules={[{ required: true, message: "Please select the start date!" }]}
                        >
                            <DatePicker style={{ width: '100%' }} />
                        </Form.Item>
                    </Col>
                    {/* End Date */}
                    <Col span={8}>
                        <Form.Item
                            label="End Date"
                            name="endDate"
                            rules={[{ required: true, message: "Please select the end date!" }]}
                        >
                            <DatePicker style={{ width: '100%' }} />
                        </Form.Item>
                    </Col>
                </Row>
                {/* Quantity */}
                <Row gutter={16}>
                    <Col span={12}>
                        <Form.Item
                            label="Quantity"
                            name="quantity"
                            rules={[{ required: true, message: "Please enter the quantity!" }]}
                        >
                            <Input type="number" placeholder="Enter quantity" />
                        </Form.Item>
                    </Col>
                    {/* Taken Quantity */}
                    <Col span={12}>
                        <Form.Item
                            label="Taken Quantity"
                            name="takenQuantity"
                            rules={[{ required: true, message: "Please enter the taken quantity!" }]}
                        >
                            <Input type="number" placeholder="Enter taken quantity" />
                        </Form.Item>
                    </Col>
                </Row>
                {/* Maximum User Can Get */}
                <Form.Item
                    label="Maximum User Can Get"
                    name="maximumUserCanGet"
                    rules={[{ required: true, message: "Please enter the maximum user can get!" }]}
                >
                    <Slider min={0} max={10} />
                </Form.Item>

                {/* Percent Discount */}
                <Form.Item
                    label="Percent Discount"
                    name="percentDiscount"
                >
                    <Slider min={0} max={100} />
                </Form.Item>

                {/* Maximum Money Discount */}
                <Form.Item
                    label="Maximum Money Discount"
                    name="maximumMoneyDiscount"
                >
                    <Input type="number" step="0.01" placeholder="Enter maximum money discount" />
                </Form.Item>

                {/* Maximum Discount */}
                <Form.Item
                    label="Maximum Discount"
                    name="maximumDiscount"
                >
                    <Input type="number" step="0.01" placeholder="Enter maximum discount" />
                </Form.Item>



                {/* Submit Button */}
                <Form.Item>
                    <Button
                        type="primary"
                        htmlType="submit"
                        loading={loading}
                        className="w-full"
                    >
                        Create Voucher
                    </Button>
                </Form.Item>
            </Form>
        </div>
    );
};

export default VoucherCreate;
