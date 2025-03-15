import React, { useState, useEffect } from "react";
import { Form, Input, Upload, Button, message } from "antd";
import { UploadOutlined, ArrowLeftOutlined } from "@ant-design/icons";
import { EditBanner, GetActiveBanner } from "../../../api/Banner/ApiBanner";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

const BannerEdit = ({ bannerId, onSuccess }) => {
    const [form] = Form.useForm();
    const [imagePreview, setImagePreview] = useState(null);
    const navigate = useNavigate();
    const [bannerImage, setBannerImage] = useState(null);
    const handleBannerImageChange = ({ file }) => {
        setBannerImage(file);
    };

    useEffect(() => {
        const fetchBanner = async () => {
            try {
                const data = await GetActiveBanner(bannerId);
                form.setFieldsValue({
                    title: data.title,
                    callToActionUrl: data.callToActionUrl,
                    priority: data.priority,
                });
                setImagePreview(data.imageUrl); // Cập nhật ảnh preview
                console.log("check data", data);
            } catch (error) {
                console.error("Error when fetching data", error);
            }
        };

        fetchBanner();
    }, [bannerId, form]);




    const handleSubmit = async (values) => {
        try {
            const formData = new FormData();
            formData.append("title", values.title);
            formData.append("callToActionUrl", values.callToActionUrl);
            formData.append("priority", parseInt(values.priority, 10));
            formData.append("file", bannerImage); // Gửi file gốc
            console.log(">>> check formData: ", formData);
            var result = await EditBanner({ formData, bannerId });
            console.log(">>> check result: ", result);
            toast.success("Banner edited successfully!");
            onSuccess();
            navigate("/admin/banner-management");
            setTimeout(() => {
                window.location.reload();
            }, 1000);
        } catch (error) {
            message.error("Edit banner failed");
        }
    };

    return (
        <Form form={form} layout="vertical" onFinish={handleSubmit}>
            <div className="flex items-center mb-6 cursor-pointer" onClick={() => navigate("/admin/banner-management")}>
                <ArrowLeftOutlined className="text-2xl mr-4" />
                <h1 className="text-3xl font-bold">Chỉnh sửa Banner</h1>
            </div>
            <Form.Item name="title" label="Tiêu Đề" rules={[{ required: true, message: "Please enter banner title" }]}>
                <Input placeholder="Enter banner title" />
            </Form.Item>
            <Form.Item name="callToActionUrl" label="Url thực hiện" rules={[{ required: true, message: "Please enter Action Url" }]}>
                <Input placeholder="Enter Action Url" />
            </Form.Item>
            <Form.Item name="priority" label="Độ ưu tiên" rules={[{ required: true, message: "Please enter Priority" }]}>
                <Input type="number" min={1} placeholder="Enter Priority" />
            </Form.Item>
            <Form.Item label="File">
                <Upload
                    beforeUpload={() => false}
                    onChange={handleBannerImageChange}
                    showUploadList={false}
                >
                    <Button icon={<UploadOutlined />}>Chọn ảnh</Button>
                </Upload>
                {bannerImage ? (
                    <div className="mt-2">
                        <img
                            src={URL.createObjectURL(bannerImage)}
                            alt="Main"
                            className="w-full h-[300px] object-contain mt-2 rounded-md"
                        />
                    </div>
                ) : imagePreview ? (
                    <div className="mt-2">
                        <img
                            src={imagePreview}
                            alt="Current Banner"
                            className="w-full h-[300px] object-contain mt-2 rounded-md"
                        />
                    </div>
                ) : null}
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
                >
                    Lưu
                </Button>
            </div>
        </Form>
    );
};

export default BannerEdit;