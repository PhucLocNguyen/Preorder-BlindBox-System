import React, { useState, useEffect } from "react";
import { Form, Input, Button, Upload, message, Card } from "antd";
import { UploadOutlined } from "@ant-design/icons";
import { GetActiveStaff, EditStaff } from "../../../api/StaffManagement/ApiStaffManager";

const StaffManagementEdit = ({ userId, onSuccess }) => {
    const [form] = Form.useForm();
    const [fileList, setFileList] = useState([]);
    const [loading, setLoading] = useState(true);

    const getBase64 = (file) => {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result);
            reader.onerror = (error) => reject(error);
        });
    };

    const handleUploadChange = async ({ fileList }) => {
        if (fileList.length > 0) {
            const base64 = await getBase64(fileList[0].originFileObj);
            setFileList([{ ...fileList[0], base64 }]);
        } else {
            setFileList([]);
        }
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
        const payload = {
            fullName: values.fullName,
            phone: values.phone,
            address: values.address,
            thumbnail: fileList.length > 0 ? fileList[0].base64 : "",
        };

        try {
            const result = await EditStaff(userId, payload);
            console.log("Update staff result:", result);
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

                <Form.Item label="Hình đại diện" name="thumbnail">
                    <Upload beforeUpload={() => false} onChange={handleUploadChange} fileList={fileList} listType="picture">
                        <Button icon={<UploadOutlined />} size="large">Upload Thumbnail</Button>
                    </Upload>
                </Form.Item>

                <div className="flex justify-center gap-4 mt-4">
                    <Button size="large" onClick={onSuccess} className="bg-gray-200 text-gray-700 px-6 py-2 rounded-lg">Cancel</Button>
                    <Button type="primary" htmlType="submit" size="large" className="bg-blue-600 text-white px-6 py-2 rounded-lg">Update</Button>
                </div>
            </Form>
        </Card>
    );
};

export default StaffManagementEdit;
