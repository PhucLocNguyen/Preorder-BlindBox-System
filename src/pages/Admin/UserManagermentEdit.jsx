import React from 'react';
import { Form, Input, Button, Row, Col, Card, DatePicker, message } from 'antd';
import GradientButton from "../../components/Buttons/GradientButton";
const UserManagermentEdit = ({ onSuccess }) => {
    const onFinish = (values) => {
        onSuccess();
    };
    return (
        <Card
            className="p-5 bg-white rounded-lg shadow-md"
            style={{ maxWidth: '600px', margin: '0 auto' }}
        >
            <h2 className="text-xl font-bold mb-4 text-center">Update User</h2>
            <Form layout="vertical" onFinish={onFinish}>
                <Row gutter={16}>
                    <Col span={6}>
                        <Form.Item label="Role ID" name="roleID" rules={[{ required: true, message: 'Please enter Role ID!' }]}>
                            <Input />
                        </Form.Item>
                    </Col>
                    <Col span={8}>
                        <Form.Item label="Wallet ID" name="walletID" rules={[{ required: true, message: 'Please enter Wallet ID!' }]}>
                            <Input />
                        </Form.Item>
                    </Col>
                    <Col span={10}>
                        <Form.Item label="Full Name" name="fullname" rules={[{ required: true, message: 'Please enter full name!' }]}>
                            <Input />
                        </Form.Item>
                    </Col>
                </Row>

                <Row gutter={16}>
                    <Col span={6}>
                        <Form.Item label="Status" name="status">
                            <Input />
                        </Form.Item>
                    </Col>
                    <Col span={8}>
                        <Form.Item label="Phone" name="phone" rules={[{ required: true, message: 'Please enter phone number!' }]}>
                            <Input />
                        </Form.Item>
                    </Col>
                    <Col span={10}>
                        <Form.Item label="Password" name="password" rules={[{ required: true, message: 'Please enter password!' }]}>
                            <Input.Password />
                        </Form.Item>
                    </Col>
                </Row>

                <Row gutter={16}>


                    <Col span={12}>
                        <Form.Item label="Email" name="email" rules={[{ required: true, type: 'email', message: 'Please enter a valid email!' }]}>
                            <Input />
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item label="Address" name="address" rules={[{ required: true, message: 'Please enter address!' }]}>
                            <Input />
                        </Form.Item>
                    </Col>
                </Row>



                <Row gutter={16}>
                    <Col span={12}>
                        <Form.Item label="Bank Account Number" name="bankAccountNumber">
                            <Input />
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item label="Bank Name" name="bankName">
                            <Input />
                        </Form.Item>
                    </Col>
                </Row>

                <Row gutter={16}>
                    <Col span={12}>
                        <Form.Item label="Created Date" name="createdDate">
                            <DatePicker style={{ width: '100%' }} />
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item label="Updated Date" name="updatedDate">
                            <DatePicker style={{ width: '100%' }} />
                        </Form.Item>
                    </Col>
                </Row>

                <Row gutter={16}>
                    <Col span={24}>
                        <Form.Item label="Thumbnail" name="thumbnail">
                            <Input placeholder="Image URL" />
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
};

export default UserManagermentEdit;
