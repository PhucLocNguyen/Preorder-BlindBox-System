import React, { useState } from 'react';
import { Button, Form, Input, DatePicker, Row, Col, Slider, InputNumber, Card } from "antd";
import { CreateVoucher } from "../../../api/VoucherCampaign/ApiVoucherCampaign";
import { toast } from 'react-toastify';
import { Link, useNavigate } from 'react-router';
import {
    ArrowLeftOutlined,
} from "@ant-design/icons";
const { RangePicker } = DatePicker;

const VoucherCreate = () => {
    const [loading, setLoading] = useState(false);
    const [form] = Form.useForm();
    const navigate = useNavigate();

    const handleSubmit = async (values) => {
        const payload = {
            name: values.name,
            startDate: values.dateRange[0].toISOString(),
            endDate: values.dateRange[1].toISOString(),
            quantity: values.quantity,
            maximumUserCanGet: values.maximumUserCanGet,
            percentDiscount: values.percentDiscount,
            maximumMoneyDiscount: values.maximumMoneyDiscount,
            setNumberExpirationDate: values.setNumberExpirationDate
        };

        try {
            const config = { headers: { 'Content-Type': 'application/json' } };
            await CreateVoucher(payload, config);
            toast.success('Preorder Campaign created successfully!');
            navigate('/admin/voucher');
        } catch (error) {
            toast.error('Error creating Preorder Campaign:', error);
        }
    };

    return (
        <div className="w-full min-h-screen flex justify-center items-center bg-gray-100 p-6">
            <Card className="w-full max-w-4xl p-6 shadow-lg rounded-lg">
                <div className="flex items-center mb-4">
                    <Link to="/admin/voucher" className="h-full flex">
                        <ArrowLeftOutlined
                            style={{
                                width: "fit-content",
                                height: "100%",
                                padding: "10px",
                            }}
                            title="Về lại trang sản phẩm"
                        />
                    </Link>
                    <h1 className="text-2xl font-bold">Create new voucher</h1>
                </div>
                <Form form={form} layout="vertical" onFinish={handleSubmit} className="space-y-4">
                    <Button type="primary" htmlType="submit" size="large" className="rounded-lg px-6 py-2 text-lg">
                        Create
                    </Button>
                    <Row gutter={[24, 16]}>
                        <Col span={12}>
                            <Form.Item label="Name" name="name" rules={[{ required: true, message: "Please enter the name!" }]}>
                                <Input placeholder="Enter voucher name" className="rounded-lg" />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item label="Maximum Money Discount" name="maximumMoneyDiscount">
                                <InputNumber
                                    style={{ width: "100%" }}
                                    min={0}
                                    placeholder="Enter maximum discount"
                                    formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " VND"}
                                    parser={(value) => value.replace(/VND\s?|(|,*)/g, "")}
                                    className="rounded-lg"
                                />
                            </Form.Item>
                        </Col>
                    </Row>

                    <Row gutter={[24, 16]}>
                        <Col span={12}>
                            <Form.Item label="Quantity" name="quantity" rules={[{ required: true, message: "Please enter the quantity!" }]}>
                                <Input type="number" placeholder="Enter quantity" className="rounded-lg" />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item label="Validity Duration" name="setNumberExpirationDate">
                                <Input type="number" placeholder="Enter Validity Duration" className="rounded-lg" />
                            </Form.Item>
                        </Col>
                    </Row>

                    <Row gutter={[24, 16]}>
                        <Col span={24}>
                            <Form.Item label="Start & End Date" name="dateRange" rules={[{ required: true, message: "Please select date range!" }]}>
                                <RangePicker showTime format="YYYY-MM-DD HH:mm:ss" className="w-full rounded-lg" />
                            </Form.Item>
                        </Col>
                    </Row>

                    <Row gutter={[24, 16]}>
                        <Col span={12}>
                            <Form.Item label="Maximum User Can Get" name="maximumUserCanGet">
                                <Slider min={0} max={10} marks={{ 0: '0', 5: '5', 10: '10' }} tooltipVisible />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item label="Percent Discount" name="percentDiscount">
                                <Slider min={0} max={100} marks={{ 0: '0%', 50: '50%', 100: '100%' }} tooltipVisible />
                            </Form.Item>
                        </Col>
                    </Row>
                </Form>
            </Card>
        </div>
    );
};

export default VoucherCreate;