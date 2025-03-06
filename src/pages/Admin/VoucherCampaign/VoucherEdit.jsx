import React, { useState, useEffect } from 'react';
import { Button, Form, InputNumber, DatePicker, Row, Col, Slider, Card } from "antd";
import { UpdateVoucher, GetTheActiveVoucherCampaignByID } from "../../../api/VoucherCampaign/ApiVoucherCampaign";
import { toast } from 'react-toastify';
import { Link, useNavigate, useParams } from 'react-router';
import { ArrowLeftOutlined } from "@ant-design/icons";
import dayjs from "dayjs";

const { RangePicker } = DatePicker;

const VoucherEdit = () => {
    const [loading, setLoading] = useState(false);
    const [form] = Form.useForm();
    const navigate = useNavigate();
    const { id } = useParams();
    const [status, setStatus] = useState(null);

    useEffect(() => {
        const fetchVouchers = async () => {
            try {
                const data = await GetTheActiveVoucherCampaignByID(id);
                setStatus(data.status);

                form.setFieldsValue({
                    dateRange: data.startDate && data.endDate ? [dayjs(data.startDate), dayjs(data.endDate)] : null,
                    quantity: data.quantity,
                    percentDiscount: data.percentDiscount,
                    maximumMoneyDiscount: data.maximumMoneyDiscount,
                    setNumberExpirationDate: data.startDate && data.endDate
                        ? dayjs(data.endDate).diff(dayjs(data.startDate), 'day') - 1
                        : null,
                    status: data.status === "Pending" ? 0 : 1,
                });
            } catch (error) {
                console.error("Lỗi khi lấy dữ liệu từ API:", error);
            }
        };

        fetchVouchers();
    }, [id]);

    const handleSubmit = async (values) => {
        const payload = {
            endDate: values.dateRange[1].toISOString(),
        };

        if (status !== "Active") {
            // Nếu không phải Active, cập nhật toàn bộ dữ liệu
            payload.startDate = values.dateRange[0].toISOString();
            payload.quantity = values.quantity;
            payload.percentDiscount = values.percentDiscount;
            payload.maximumMoneyDiscount = values.maximumMoneyDiscount;
            payload.status = Number(values.status);
            payload.setNumberExpirationDate = values.setNumberExpirationDate;
        }

        try {
            const config = { headers: { 'Content-Type': 'application/json' } };
            const result = await UpdateVoucher(id, payload, config);
            console.log(result);
            toast.success('Voucher updated successfully!');
            navigate('/admin/voucher');
        } catch (error) {
            toast.error('Error updating voucher:', error);
        }
    };

    return (
        <div className="w-full min-h-screen flex justify-center items-center bg-gray-100 p-6">
            <Card className="w-full max-w-5xl p-8 shadow-2xl rounded-xl">
                <div className="flex items-center mb-8">
                    <Link to="/admin/voucher" className="h-full flex">
                        <ArrowLeftOutlined className="text-2xl mr-6" />
                    </Link>
                    <h1 className="text-3xl font-bold">Update Voucher</h1>
                </div>
                <Form form={form} layout="vertical" onFinish={handleSubmit} className="space-y-8">
                    <Button type="primary" htmlType="submit" size="large" className="rounded-xl px-8 py-3 text-xl">
                        Update
                    </Button>
                    <Row gutter={32}>
                        <Col span={12}>
                            <Form.Item label="Status" name="status">
                                <InputNumber
                                    min={0}
                                    disabled={status === "Active"}
                                    style={{ width: "100%", height: "48px" }}
                                    className="rounded-xl text-lg"
                                />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item
                                label="Maximum Money Discount"
                                name="maximumMoneyDiscount"
                                rules={[{ required: true, message: "Please enter maximum money discount!" }]}
                            >
                                <InputNumber
                                    style={{ width: "100%", height: "48px" }}
                                    min={0}
                                    disabled={status === "Active"}
                                    placeholder="Enter maximum money discount"
                                    formatter={(value) =>
                                        value ? `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " VND" : "0 VND"
                                    }
                                    parser={(value) =>
                                        value.replace(/[^0-9]/g, "")
                                    }
                                    className="rounded-xl text-lg"
                                />
                            </Form.Item>
                        </Col>
                    </Row>
                    <Row gutter={32}>
                        <Col span={12}>
                            <Form.Item label="Quantity" name="quantity">
                                <InputNumber
                                    disabled={status === "Active"}
                                    style={{ width: "100%", height: "48px" }}
                                    className="rounded-xl text-lg"
                                />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item label="Validity Duration" name="setNumberExpirationDate">
                                <InputNumber
                                    style={{ width: "100%", height: "48px" }}
                                    className="rounded-xl text-lg"
                                />
                            </Form.Item>
                        </Col>
                    </Row>
                    <Row gutter={32}>
                        <Col span={24}>
                            <Form.Item label="Start & End Date" name="dateRange">
                                <RangePicker
                                    showTime
                                    format="YYYY-MM-DD HH:mm:ss"
                                    className="w-full text-lg p-3 rounded-lg"
                                    disabled={status === "Active" ? [true, false] : false} // Active: chỉ cập nhật EndDate
                                />
                            </Form.Item>
                        </Col>
                    </Row>
                    <Row gutter={32}>
                        <Col span={24}>
                            <Form.Item label="Percent Discount" name="percentDiscount">
                                <Slider
                                    min={0}
                                    max={100}
                                    marks={{ 0: '0%', 50: '50%', 100: '100%' }}
                                    tooltipVisible
                                    className="h-8"
                                    disabled={status === "Active"}
                                />
                            </Form.Item>
                        </Col>
                    </Row>
                </Form>
            </Card>
        </div>
    );
}

export default VoucherEdit;
