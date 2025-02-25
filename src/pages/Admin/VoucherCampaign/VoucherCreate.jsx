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
        setLoading(true); // Bật loading khi bắt đầu gửi request

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

            const result = await CreateVoucher(payload, config);

            if (result && result.status === 200) { // Kiểm tra response thành công
                navigate('/admin/voucher');
            }

        } catch (error) {
            toast.error('Error creating voucher: ' + (error.response?.data?.message || error.message));
        } finally {
            setLoading(false); // Tắt loading dù thành công hay thất bại
        }
    };


    return (
        <div className="w-full min-h-screen flex justify-center items-center bg-gray-100 p-10">
            <Card className="w-full max-w-7xl p-10 shadow-2xl rounded-2xl">
                <div className="flex items-center mb-6">
                    <Link to="/admin/voucher" className="h-full flex">
                        <ArrowLeftOutlined className="text-2xl mr-6" />
                    </Link>
                    <h1 className="text-3xl font-bold">Create new voucher</h1>
                </div>
                <Form form={form} layout="vertical" onFinish={handleSubmit} className="space-y-6">
                    <Button type="primary" htmlType="submit" size="large" className="rounded-xl px-8 py-3 text-xl h-14">
                        Create
                    </Button>
                    <Row gutter={[32, 20]}>
                        <Col span={12}>
                            <Form.Item label="Name" name="name" rules={[{ required: true, message: "Please enter the name!" }]}>
                                <Input placeholder="Enter voucher name" className="rounded-xl h-12 text-lg" />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item label="Maximum Money Discount" name="maximumMoneyDiscount"
                                rules={[{ required: true, message: "Please enter maximum money discount!" }]}
                            >
                                <InputNumber
                                    style={{ width: "100%", height: "48px" }}
                                    min={0}
                                    placeholder="Enter money maximum discount"
                                    formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " VND"}
                                    parser={(value) => value.replace(/VND\s?|(|,*)/g, "")}
                                    className="rounded-xl text-lg"
                                />
                            </Form.Item>
                        </Col>
                    </Row>

                    <Row gutter={[32, 20]}>
                        <Col span={12}>
                            <Form.Item label="Quantity" name="quantity" rules={[{ required: true, message: "Please enter the quantity!" }]}>
                                <Input type="number" placeholder="Enter quantity" className="rounded-xl h-12 text-lg" />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item label="Validity Duration" name="setNumberExpirationDate" rules={[{ required: true, message: "Please enter the quantity!" }]}>
                                <Input type="number" placeholder="Enter Validity Duration" className="rounded-xl h-12 text-lg" />
                            </Form.Item>
                        </Col>
                    </Row>

                    <Row gutter={[32, 20]}>
                        <Col span={24}>
                            <Form.Item label="Start & End Date" name="dateRange" rules={[{ required: true, message: "Please select date range!" }]}>
                                <RangePicker showTime format="YYYY-MM-DD HH:mm:ss" className="w-full rounded-xl h-12 text-lg" />
                            </Form.Item>
                        </Col>
                    </Row>

                    <Row gutter={[32, 20]}>
                        <Col span={12}>
                            <Form.Item label="Maximum User Can Get" name="maximumUserCanGet">
                                <Slider min={0} max={10} marks={{ 0: '0', 5: '5', 10: '10' }} tooltipVisible={{ always: true }} className="h-8" />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item label="Percent Discount" name="percentDiscount">
                                <Slider min={0} max={100} marks={{ 0: '0%', 50: '50%', 100: '100%' }} tooltipVisible={{ always: true }} className="h-8" />
                            </Form.Item>
                        </Col>
                    </Row>
                </Form>
            </Card>
        </div>
    );
};

export default VoucherCreate;
