import React from "react";
import { Form, Input, Upload, Switch, Button } from "antd";
import { UploadOutlined } from "@ant-design/icons";

const BannerCreate = ({ onSave }) => {
    const [form] = Form.useForm();

    const handleSubmit = () => {
        form.validateFields().then((values) => {
            onSave(values);
            form.resetFields();
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
                Create Banner
            </Button>
        </Form>
    );
};

export default BannerCreate;
