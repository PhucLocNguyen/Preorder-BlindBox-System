import React, { useEffect } from "react";
import { Form, Input, Upload, Switch, Button } from "antd";
import { UploadOutlined } from "@ant-design/icons";

const BannerEdit = ({ onSave, editingBanner }) => {
    const [form] = Form.useForm();

    // Gán giá trị mặc định cho form khi chỉnh sửa
    useEffect(() => {
        if (editingBanner) {
            form.setFieldsValue(editingBanner);
        }
    }, [editingBanner, form]);

    const handleSubmit = () => {
        form.validateFields().then((values) => {
            onSave(values);
        });
    };

    return (
        <Form form={form} layout="vertical">
            <Form.Item name="title" label="Title" rules={[{ required: true, message: "Please enter banner title" }]}>
                <Input placeholder="Enter banner title" />
            </Form.Item>
            <Form.Item name="image" label="Image">
                <Upload>
                    <Button icon={<UploadOutlined />}>Upload Image</Button>
                </Upload>
            </Form.Item>
            <Form.Item name="status" label="Status" valuePropName="checked">
                <Switch checkedChildren="Active" unCheckedChildren="Inactive" />
            </Form.Item>
            <Button type="primary" onClick={handleSubmit} block>
                Update Banner
            </Button>
        </Form>
    );
};

export default BannerEdit;
