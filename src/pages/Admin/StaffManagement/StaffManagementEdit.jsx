import React, { useState, useEffect } from "react";
import { Form, Input, Button, Upload, message, Card } from "antd";
import { UploadOutlined } from "@ant-design/icons";
import { GetActiveStaff, EditStaff } from "../../../api/StaffManagement/ApiStaffManager";

const StaffManagementEdit = ({ userId, onSuccess }) => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(true);
    const [imagePreview, setImagePreview] = useState(null);
    const [staffImage, setStaffImage] = useState(null);
    const handleStaffImageChange = ({ file }) => {
        setStaffImage(file);
    };
    useEffect(() => {
        const fetchStaff = async () => {
            try {
                const staffData = await GetActiveStaff(userId);
                if (staffData) {
                    form.setFieldsValue({
                        fullName: staffData.fullName,
                        address: staffData.address,
                        phone: staffData.phone || "",

                    });
                }
                setImagePreview(staffData.thumbnail);
                console.log("check data", staffData);
            } catch (error) {
                message.error("Failed to fetch staff details!");
            } finally {
                setLoading(false);
            }
        };

        if (userId) {
            fetchStaff();
        }
    }, [userId, form]);

    const onFinish = async (values) => {
        const formData = new FormData();
        formData.append("fullName", values.fullName);
        formData.append("phone", values.phone);
        formData.append("thumbnail", staffImage);
        formData.append("address", values.address);


        try {
            await EditStaff({ formData, userId });
            message.success("Staff updated successfully!");
            onSuccess();
            setTimeout(() => {
                window.location.reload();
            }, 1000);
        } catch (error) {
            message.error("Update failed!");
        }
    };



    return (
        <Card className="max-w-lg mx-auto p-6 shadow-lg rounded-xl bg-white">
            <h2 className="text-2xl font-semibold text-center mb-6">Chỉnh sửa nhân viên</h2>
            <Form form={form} layout="vertical" onFinish={onFinish} className="space-y-4">
                <Form.Item label="Họ và tên" name="fullName" rules={[{ required: true, message: "Please enter full name!" }]}>
                    <Input size="large" className="w-full border-gray-300 rounded-lg" />
                </Form.Item>

                <Form.Item label="Số điện thoại" name="phone" rules={[
                    { required: true, message: "Please enter phone number!" },
                    { max: 10, message: "Phone number must be at maximum 10 number digits!" },
                ]}>
                    <Input size="large" className="w-full border-gray-300 rounded-lg" />
                </Form.Item>

                <Form.Item label="Địa chỉ" name="address" rules={[{ required: true, message: "Please enter address!" }]}>
                    <Input size="large" className="w-full border-gray-300 rounded-lg" />
                </Form.Item>

                <Form.Item label="Hình đại diện">
                    <Upload
                        beforeUpload={() => false}
                        onChange={handleStaffImageChange}
                        showUploadList={false}
                    >
                        <Button icon={<UploadOutlined />}>Chọn ảnh</Button>
                    </Upload>
                    {staffImage ? (
                        <div className="mt-2">
                            <img
                                src={URL.createObjectURL(staffImage)}
                                alt="Main"
                                className="w-full h-[300px] object-contain mt-2 rounded-md"
                            />
                        </div>
                    ) : imagePreview ? (
                        <div className="mt-2">
                            <img
                                src={imagePreview}
                                alt="Current Staff"
                                className="w-full h-[300px] object-contain mt-2 rounded-md"
                            />
                        </div>
                    ) : null}
                </Form.Item>

                <div className="flex justify-center gap-4 mt-4">
                    <Button size="large" onClick={onSuccess} className="bg-gray-200 text-gray-700 px-6 py-2 rounded-lg">Hủy</Button>
                    <Button type="primary" htmlType="submit" size="large" className="bg-blue-600 text-white px-6 py-2 rounded-lg">Lưu</Button>
                </div>
            </Form>
        </Card>
    );
};

export default StaffManagementEdit;
