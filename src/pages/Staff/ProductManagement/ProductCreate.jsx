import { Button, Form, Input, Select, Upload } from "antd";
const { Option } = Select;
import { useState } from "react";
import {UploadOutlined} from "@ant-design/icons";
import { CreateBlindBox } from "../../../api/BlindBox/ApiBlindBox";
function ProductCreate() {
  const [loading, setLoading] = useState(false);
  const [form] = Form.useForm();
  const [mainImage, setMainImage] = useState(null);
  const [galleryImages, setGalleryImages] = useState([]);
  const handleMainImageChange = ({ file }) => {
    setMainImage(file);
  };

  const handleGalleryImagesChange = ({ fileList }) => {
    setGalleryImages(fileList.map((file) => file.originFileObj));
  };
  const handleSubmit = async (values) => {
    const formData = new FormData();
    formData.append("name", values.name);
    formData.append("description", values.description);
    formData.append("size", values.size);
    if (mainImage) formData.append("mainImage", mainImage);
    galleryImages.forEach((file, index) => {
      formData.append(`galleryImages`, file);
    });
   var result = await CreateBlindBox({formData});
   console.log(result);
  };
  return (
    <div>
      <div className="w-full min-h-screen mx-auto mt-5 p-5 bg-white shadow-lg rounded-lg">
        <h2 className="text-xl font-bold mb-4">Tạo Blind Box Mới</h2>
        <Form form={form} layout="vertical" onFinish={handleSubmit}>
          {/* Name */}
          <Form.Item
            label="Tên sản phẩm"
            name="name"
            rules={[{ required: true, message: "Vui lòng nhập tên sản phẩm!" }]}
          >
            <Input placeholder="Nhập tên sản phẩm" />
          </Form.Item>

          {/* Description */}
          <Form.Item
            label="Mô tả"
            name="description"
            rules={[{ required: true, message: "Vui lòng nhập mô tả!" }]}
          >
            <Input.TextArea placeholder="Nhập mô tả sản phẩm" rows={4} />
          </Form.Item>

          {/* Size */}
          <Form.Item
            label="Kích thước"
            name="size"
            rules={[{ required: true, message: "Vui lòng chọn kích thước!" }]}
          >
            <Select placeholder="Chọn kích thước">
              <Option value="Small">Nhỏ</Option>
              <Option value="Medium">Trung bình</Option>
              <Option value="Large">Lớn</Option>
            </Select>
          </Form.Item>

          {/* Main Image */}
          <Form.Item label="Ảnh chính">
            <Upload
              beforeUpload={() => false} // Ngăn upload tự động
              onChange={handleMainImageChange}
              showUploadList={false} // Không hiển thị danh sách ảnh
            >
              <Button icon={<UploadOutlined />}>Chọn ảnh</Button>
            </Upload>
            {mainImage && (
              <div className="mt-2">
                <img
                  src={URL.createObjectURL(mainImage)}
                  alt="Main"
                  className="w-32 h-32 object-cover mt-2 rounded-md"
                />
              </div>
            )}
          </Form.Item>

          {/* Gallery Images */}
          <Form.Item label="Ảnh phụ">
            <Upload
              multiple
              listType="picture-card"
              accept="jpg,png,jpeg"
              beforeUpload={() => false}
              onChange={handleGalleryImagesChange}
            >
              <Button icon={<UploadOutlined />}>Chọn ảnh</Button>
            </Upload>
          </Form.Item>

          {/* Submit Button */}
          <Form.Item>
            <Button
              type="primary"
              htmlType="submit"
              loading={loading}
              className="w-full"
            >
              Tạo sản phẩm
            </Button>
          </Form.Item>
        </Form>
      </div>
    </div>
  );
}

export default ProductCreate;
