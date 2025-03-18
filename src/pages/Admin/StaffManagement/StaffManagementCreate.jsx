import React, { useState } from "react";
import { Button, Form, Input, Card } from "antd";
import { CreateStaffAccount } from "../../../api/StaffManagement/ApiStaffManager";
import { toast } from "react-toastify";
import { Link, useNavigate } from "react-router-dom";
import { ArrowLeftOutlined } from "@ant-design/icons";

const StaffManagementCreate = ({ onSuccess }) => {
    const [loading, setLoading] = useState(false);
    const [form] = Form.useForm();
    const navigate = useNavigate();

    const handleSubmit = async (values) => {
        setLoading(true);

        const payload = {
            fullName: values.fullName,
            email: values.email,
            password: values.password,
            confirmPassword: values.confirmPassword,
            address: values.address,
        };

        try {
            const config = { headers: { "Content-Type": "application/json" } };

            const response = await CreateStaffAccount(payload, config);
            console.log(response);
            toast.success("Staff account created successfully!");
            if (response) {
                setTimeout(() => {
                    window.location.reload();
                }, 1000);
                onSuccess();
            }
        } catch (error) {
            toast.error(
                "Error creating staff account: " +
                (error.response?.data?.message || error.message)
            );
        } finally {
            setLoading(false);
        }

    };

    return (
        <div className="w-full min-h-screen flex justify-center items-center bg-gray-100 p-6">
            <Card className="w-full max-w-lg p-8 shadow-xl rounded-2xl bg-white">
                <div className="flex items-center mb-6">
                    <h1 className="text-2xl font-semibold text-gray-800">
                        Tạo mới tài khoản nhân viên
                    </h1>
                </div>

                <Form form={form} layout="vertical" onFinish={handleSubmit} className="space-y-5">
                    <Form.Item
                        label="Họ và tên"
                        name="fullName"
                        rules={[{ required: true, message: "Please enter full name!" }]}
                    >
                        <Input
                            placeholder="Enter full name"
                            className="rounded-lg h-12 text-lg px-4 border-gray-300 focus:ring-2 focus:ring-blue-500"
                        />
                    </Form.Item>

                    <Form.Item
                        label="Email"
                        name="email"
                        rules={[
                            { required: true, message: "Please enter email!" },
                            { type: "email", message: "Invalid email format!" },
                        ]}
                    >
                        <Input
                            placeholder="Enter email"
                            className="rounded-lg h-12 text-lg px-4 border-gray-300 focus:ring-2 focus:ring-blue-500"
                        />
                    </Form.Item>

                    <Form.Item
                        label="Mật khẩu"
                        name="password"
                        rules={[
                            { required: true, message: "Please enter password!" },
                            { min: 6, message: "Password must be at least 6 characters!" },
                        ]}
                    >
                        <Input.Password
                            placeholder="Enter password"
                            className="rounded-lg h-12 text-lg px-4 border-gray-300 focus:ring-2 focus:ring-blue-500"
                        />
                    </Form.Item>


                    <Form.Item
                        label="Xác nhận mật khẩu"
                        name="confirmPassword"
                        dependencies={["password"]}
                        rules={[
                            { required: true, message: "Please confirm password!" },
                            { min: 6, message: "Password must be at least 6 characters!" },
                            ({ getFieldValue }) => ({
                                validator(_, value) {
                                    if (!value || getFieldValue("password") === value) {
                                        return Promise.resolve();
                                    }
                                    return Promise.reject(new Error("Passwords do not match!"));
                                },
                            }),
                        ]}
                    >
                        <Input.Password
                            placeholder="Confirm password"
                            className="rounded-lg h-12 text-lg px-4 border-gray-300 focus:ring-2 focus:ring-blue-500"
                        />
                    </Form.Item>

                    <Form.Item
                        label="Địa chỉ"
                        name="address"
                        rules={[{ required: true, message: "Please enter address!" }]}
                    >
                        <Input
                            placeholder="Enter address"
                            className="rounded-lg h-12 text-lg px-4 border-gray-300 focus:ring-2 focus:ring-blue-500"
                        />
                    </Form.Item>

                    <div className="flex justify-between gap-4">
                        <Button
                            size="large"
                            className="w-1/2 rounded-lg h-12 bg-gray-200 hover:bg-gray-300 text-gray-800"
                            onClick={onSuccess}
                        >
                            Hủy
                        </Button>
                        <Button
                            type="primary"
                            htmlType="submit"
                            size="large"
                            className="w-1/2 rounded-lg h-12 bg-blue-600 hover:bg-blue-700 text-white"
                            onClick={onSuccess}
                        >
                            Tạo
                        </Button>
                    </div>
                </Form>
            </Card>
        </div>
    );
};

export default StaffManagementCreate;
