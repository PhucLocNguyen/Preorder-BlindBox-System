import { useParams } from "react-router";
import {
  EditBlindBox,
  GetActiveBlindBoxById,
} from "../../../api/BlindBox/ApiBlindBox";
import { Button, Form, Input, Select, Upload } from "antd";
import { useEffect, useState } from "react";
import { UploadOutlined } from "@ant-design/icons";

function ProductEdit() {
  const { id } = useParams();
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(true);
  const [mainImage, setMainImage] = useState(null);
  const [galleryImages, setGalleryImages] = useState([]);
  const [detailBlindBox, setDetailBlindBox] = useState({});
  const [deleteImagesIDGallery, setDeleteImagesGallery] = useState([]);

  const getDetailBlindBox = async () => {
    var data = await GetActiveBlindBoxById(id);
    console.log("BlindBox data:", data); // Kiểm tra
    setDetailBlindBox(data);
    setLoading(false);
  };
  useEffect(() => {
    getDetailBlindBox();
  }, []);
  const handleMainImageChange = ({ file }) => {
    setMainImage(file);
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
    formData.append("deletedGalleryImagesID", deleteImagesIDGallery);
    console.log(formData);
    var result = await EditBlindBox({ formData, id });
    console.log(result);
  };
  const deleteOldImageAction = (id)=>{
    var arrDeleteImagesId = [...deleteImagesIDGallery,id];
    setDeleteImagesGallery(arrDeleteImagesId);
  }
  return loading ? (
    <div>Loading....</div>
  ) : (
    <div>
      <h2>
        <div>
          <div className="w-full min-h-screen mx-auto mt-5 p-5 bg-white shadow-lg rounded-lg">
            <h2 className="text-xl font-bold mb-4">Chỉnh sửa blind box</h2>
            <Form
              form={form}
              layout="vertical"
              onFinish={handleSubmit}
              initialValues={{
                name: detailBlindBox.name,
                description: detailBlindBox.description,
                size: detailBlindBox.size,
              }}
            >
              {/* Name */}
              <Form.Item
                label="Tên sản phẩm"
                name="name"
                rules={[
                  { required: true, message: "Vui lòng nhập tên sản phẩm!" },
                ]}
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
                rules={[
                  { required: true, message: "Vui lòng chọn kích thước!" },
                ]}
              >
                <Select placeholder="Chọn kích thước">
                  <Select.Option value="Small">Nhỏ</Select.Option>
                  <Select.Option value="Medium">Trung bình</Select.Option>
                  <Select.Option value="Large">Lớn</Select.Option>
                </Select>
              </Form.Item>

              {/* Main Image */}
              <Form.Item label="Ảnh chính" name="mainImage">
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
                  accept="image/*"
                  beforeUpload={() => false}
                  onChange={({ fileList: newFileList }) => {
                    setGalleryImages(newFileList);
                  }}
                >
                  <Button icon={<UploadOutlined />}>Chọn ảnh</Button>
                </Upload>
                <div className="flex flex-row  h-fit ">
                  {!(
                    detailBlindBox != null &&
                    detailBlindBox.images?.galleryImages != null
                  ) ? (
                    <div>There are no images please upload for view</div>
                  ) : (
                    detailBlindBox.images.galleryImages.map((item, index) => {
                      return (
                        <span className={
                            "relative  w-[100px] h-[100px] " + 
                            (deleteImagesIDGallery.includes(item.imageId) ? "hidden" : "")
                          }>
                            <button 
                              type="button" 
                              onClick={(e) => {
                                e.preventDefault();
                                e.stopPropagation();
                                deleteOldImageAction(item.imageId);
                              }} 
                              className="absolute top-1 right-1 text-white bg-red-500 hover:bg-red-600 
                                         rounded-full w-5 h-5 flex items-center justify-center 
                                         leading-none text-xs"
                            >
                              X
                            </button>
                            <img key={item.imageId} src={item.url} />
                          </span>
                          
                      );
                    })
                  )}
                </div>
              </Form.Item>

              {/* Submit Button */}
              <Form.Item>
                <Button
                  type="primary"
                  htmlType="submit"
                  loading={loading}
                  className="w-full"
                >
                  Chỉnh sửa
                </Button>
              </Form.Item>
            </Form>
          </div>
        </div>
      </h2>
    </div>
  );
}

export default ProductEdit;
