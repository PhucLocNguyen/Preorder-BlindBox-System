import React, { useState } from 'react';
import { Button, Form, Input, DatePicker, Row, Col, Slider, InputNumber, Select, Card } from "antd";
import { UpdateVoucher } from "../../../api/VoucherCampaign/ApiVoucherCampaign";
import { toast } from 'react-toastify';
import { Link, useNavigate } from 'react-router';
import { useParams } from 'react-router';
import { ArrowLeftOutlined } from "@ant-design/icons";

const { RangePicker } = DatePicker;
const { Option } = Select;

const VoucherEdit = () => {
    const [loading, setLoading] = useState(false);
    const [form] = Form.useForm();
    const navigate = useNavigate();
    const { id } = useParams();

    const handleSubmit = async (values) => {
        const payload = {
            startDate: values.dateRange[0].toISOString(),
            endDate: values.dateRange[1].toISOString(),
            quantity: values.quantity,
            percentDiscount: values.percentDiscount,
            maximumMoneyDiscount: values.maximumMoneyDiscount,
            setNumberExpirationDate: values.setNumberExpirationDate,
            status: values.status,
        };

        try {
            const config = { headers: { 'Content-Type': 'application/json' } };
            await UpdateVoucher(id, payload, config);
            toast.success('Voucher updated successfully!');
            navigate('/admin/voucher');
        } catch (error) {
            toast.error('Error updating voucher:', error);
        }
    };

    return (
        <div className="w-full min-h-screen flex justify-center items-center bg-gray-100 p-6">
            <Card className="w-full max-w-4xl p-6 shadow-lg rounded-lg">
                <div className="flex items-center mb-6">
                    <Link to="/admin/voucher" className="h-full flex">
                        <ArrowLeftOutlined className="text-xl mr-4" />
                    </Link>
                    <h1 className="text-2xl font-bold">Update Voucher</h1>
                </div>

                <Form form={form} layout="vertical" onFinish={handleSubmit} className="space-y-6">
                    <Button type="primary" htmlType="submit" size="large" className="rounded-lg px-6 py-2 text-lg">
                        Update
                    </Button>
                    <Row gutter={24}>
                        <Col span={12}>
                            <Form.Item label="Status" name="status" rules={[{ required: true, message: "Please select a status!" }]}>
                                <Select placeholder="Select a status">
                                    <Option value={0}>Close</Option>
                                    <Option value={1}>Pending</Option>
                                    <Option value={2}>Active</Option>
                                    <Option value={3}>Expired</Option>
                                </Select>
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item label="Maximum Money Discount" name="maximumMoneyDiscount">
                                <InputNumber min={0} style={{ width: "100%" }} placeholder="Enter maximum discount" />
                            </Form.Item>
                        </Col>
                    </Row>

                    <Row gutter={24}>
                        <Col span={12}>
                            <Form.Item label="Quantity" name="quantity" rules={[{ required: true, message: "Please enter quantity!" }]}>
                                <Input type="number" placeholder="Enter quantity" />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item label="Validity Duration" name="setNumberExpirationDate">
                                <Input type="number" placeholder="Enter Validity Duration" />
                            </Form.Item>
                        </Col>
                    </Row>

                    <Row gutter={24}>
                        <Col span={24}>
                            <Form.Item label="Start & End Date" name="dateRange" rules={[{ required: true, message: "Please select date range!" }]}>
                                <RangePicker showTime format="YYYY-MM-DD HH:mm:ss" className="w-full" />
                            </Form.Item>
                        </Col>
                    </Row>

                    <Row gutter={24}>
                        <Col span={24}>
                            <Form.Item label="Percent Discount" name="percentDiscount">
                                <Slider min={0} max={100} marks={{ 0: '0%', 50: '50%', 100: '100%' }} tooltipVisible />
                            </Form.Item>
                        </Col>
                    </Row>
                </Form>
            </Card>
        </div>
    );
}

export default VoucherEdit;
