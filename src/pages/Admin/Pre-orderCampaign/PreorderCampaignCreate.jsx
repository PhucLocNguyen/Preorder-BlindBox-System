import { Button, Form, Input, Select, DatePicker, Space } from "antd";
const { Option } = Select;
import { useState, useEffect } from "react";
import {
  ArrowLeftOutlined,
  MinusCircleOutlined,
  PlusOutlined,
} from "@ant-design/icons";
import { Link, useNavigate } from "react-router";
import ProductCardModal from "../../../components/Search/SearchBlindbox";
import TextArea from "antd/es/input/TextArea";
import { CreatePreorderCampaign } from "../../../api/Pre_orderCampaign/ApiPre_orderCampaign";

const { RangePicker } = DatePicker;

function PreorderCampaignCreate() {
  const [loading, setLoading] = useState(false);
  const [form] = Form.useForm();
  const [loadMainProduct, setLoadMainProduct] = useState(null);
  const [typeOfCampaign, setTypeOfCampaign] = useState(null);
  // State để xác định form có hợp lệ hay không (đã chạm và không có lỗi)
  const [isFormValid, setIsFormValid] = useState(false);

  const navigate = useNavigate();

  // Cập nhật trạng thái form mỗi khi các field thay đổi
  const onFieldsChange = () => {
    const fieldsError = form.getFieldsError();
    if(typeOfCampaign==null){
        setIsFormValid(false);
        return;
    }
    if(loadMainProduct==null){
        setIsFormValid(false);
        return;
    }
    const milestoneValues = form.getFieldValue("milestones") || null;
    console.log(milestoneValues);
    if(milestoneValues.length<=1){
        setIsFormValid(false);
        return;
    }
    // Nếu có milestone được thêm, kiểm tra xem có field nào bị trống hay không
    const milestoneHasEmpty = milestoneValues.some((m) => {
      // Kiểm tra nếu m là undefined hoặc thiếu quantity hoặc price
      return !m || m.quantity === undefined || m.quantity === "" || m.price === undefined || m.price === "";
    });
    if(milestoneHasEmpty){
        setIsFormValid(false);
        return;
    }
    const dateRange= form.getFieldValue("dateRange") || null;
    console.log(dateRange);
    var dateNow = new Date();
    console.log(dateRange);
    if(dateRange==null){
        setIsFormValid(false);
        return;
    }
    if(dateRange[0]<dateNow){
        setIsFormValid(false);
        return;
    }

    const hasErrors = fieldsError.some(field => field.errors.length > 0);
    // Kiểm tra xem các field đã được chạm chưa
    console.log(hasErrors);
    setIsFormValid(!hasErrors);
  };

  // Khi người dùng chọn loại chiến dịch, cập nhật state và reset field milestones
  const handleChangeTypeCampaign = (value) => {
    setTypeOfCampaign(value);
    form.setFieldsValue({ typeOfCampaign: value, milestones: [] });
  };

  const handleSubmit = async (values) => {
    const formData = new FormData();
    formData.append("blindBoxId", loadMainProduct?.blindBoxId || "");
    // Thêm field typeOfCampaign
    formData.append("type", values.type || typeOfCampaign);
    console.log(values);
    // Thời gian chiến dịch (RangePicker trả về mảng Moment)
    if (values.dateRange) {
      formData.append("startTime", values.dateRange[0].toISOString());
      formData.append("endTime", values.dateRange[1].toISOString());
    }

    // Milestones (là mảng các object có quantity và price)
    if (values.milestones) {
      values.milestones.forEach((item, index) => {
        formData.append(`milestones[${index}][quantity]`, item.quantity);
        formData.append(`milestones[${index}][price]`, item.price);
      });
    }
    
    await CreatePreorderCampaign(formData);
    navigate("/admin/pre-ordercampaign");
  };

  return (
    <div>
      <div className="w-full min-h-screen h-full mx-auto bg-white shadow-lg rounded-lg">
        <Form
          form={form}
          layout="vertical"
          onFinish={handleSubmit}
          onFieldsChange={onFieldsChange}
        >
          <div className="grid grid-cols-12 gap-4 min-h-screen mx-auto mt-5 p-5 bg-[#e5e7eb] shadow-lg rounded-lg">
            <div className="col-span-9 ">
              <div className="bg-white p-4 rounded-lg">
                <div className="flex items-center mb-4">
                  <Link to="/admin/pre-ordercampaign" className="h-full flex">
                    <ArrowLeftOutlined
                      style={{
                        width: "fit-content",
                        height: "100%",
                        padding: "10px",
                      }}
                      title="Về lại trang sản phẩm"
                    />
                  </Link>
                  <h1 className="text-2xl font-bold">Tạo Chiến dịch mới</h1>
                </div>

                <div className="mb-4">
                  <div className="flex items-center gap-3">
                    <h2 className="text-xl mb-2 mt-2">
                      Thông tin chi tiết về sản phẩm
                    </h2>
                    <ProductCardModal setLoadMainProduct={setLoadMainProduct} />
                  </div>
                  {loadMainProduct != null && (
                    <div className="grid grid-cols-12 gap-6">
                      <div className="col-span-3">
                        <h3 className="text-[16px]">Hình ảnh sản phẩm</h3>
                        <div className="mt-2">
                          <img
                            src={loadMainProduct.images?.mainImage.url}
                            alt="Main"
                            className="w-full h-[290px] object-cover mt-2"
                            style={{ borderRadius: "10px" }}
                          />
                        </div>
                      </div>
                      <div className="col-span-9">
                        <div className="mt-2">
                          <h3 className="text-[16px] mt-2 mb-">Tên sản phẩm</h3>
                          <Form.Item
                            name="name"
                            initialValue={loadMainProduct?.name}
                          >
                            <Input disabled />
                          </Form.Item>
                          <h3 className="text-[16px] mt-2 mb-1">Mô tả</h3>
                          <Form.Item
                            name="description"
                            initialValue={loadMainProduct?.description}
                          >
                            <TextArea style={{ height: "150px" }} disabled />
                          </Form.Item>
                          <h3 className="text-[16px] mt-2 mb-1">Kích cỡ</h3>
                          <Form.Item
                            name="size"
                            initialValue={loadMainProduct?.size}
                          >
                            <Select disabled className="w-full">
                              <Option value={loadMainProduct?.size}>
                                {loadMainProduct?.size}
                              </Option>
                            </Select>
                          </Form.Item>
                        </div>
                      </div>
                    </div>
                  )}
                </div>

                {/* Milestones */}
                <div>
                  <h2 className="text-xl mb-2 mt-2">
                    Các cột mốc cho chiến dịch
                  </h2>
                  <div className="mb-4">
                    <h3 className="text-lg">Chọn loại chiến dịch</h3>
                    <Form.Item
                      name="type"
                      rules={[
                        {
                          required: true,
                          message: "Vui lòng chọn loại chiến dịch!",
                        },
                      ]}
                    >
                      <Select
                        placeholder="Vui lòng chọn loại chiến dịch"
                        className="w-full"
                        onChange={handleChangeTypeCampaign}
                      >
                        <Option value="BulkOrder">Bulk Order</Option>
                        <Option value="TimedPricing">Timed Pricing</Option>
                      </Select>
                    </Form.Item>
                  </div>
                  <div>
                    <h3 className="text-lg">Thêm các mốc giá và số lượng</h3>
                  </div>
                  {typeOfCampaign && (
                    <Form.List name="milestones">
                      {(fields, { add, remove }) => (
                        <>
                          {fields.map(({ key, name, ...restField }, index) => (
                            <Space
                              key={key}
                              style={{ display: "flex", marginBottom: 8 }}
                              align="baseline"
                            >
                              <Form.Item
                                {...restField}
                                name={[name, "quantity"]}
                                label="Số lượng"
                                rules={[
                                  {
                                    required: true,
                                    message: "Vui lòng nhập số lượng!",
                                  },
                                ]}
                              >
                                <Input placeholder="Số lượng" />
                              </Form.Item>
                              <Form.Item
                                {...restField}
                                name={[name, "price"]}
                                label="Số tiền"
                                rules={[
                                  {
                                    required: true,
                                    message: "Vui lòng nhập số tiền!",
                                  },
                                  ({ getFieldValue }) => ({
                                    validator(_, value) {
                                      const milestones =
                                        getFieldValue("milestones") || [];
                                      if (index === 0) {
                                        return Promise.resolve();
                                      }
                                      const previousMilestone =
                                        milestones[index - 1];
                                      if (
                                        previousMilestone === undefined ||
                                        previousMilestone.price === undefined
                                      ) {
                                        return Promise.resolve();
                                      }
                                      const prevPrice = parseFloat(
                                        previousMilestone.price
                                      );
                                      const currentPrice = parseFloat(value);
                                      if (typeOfCampaign === "TimedPricing") {
                                        if (currentPrice <= prevPrice) {
                                          return Promise.reject(
                                            new Error(
                                              `Giá phải tăng dần so với cột mốc trước (phải lớn hơn ${previousMilestone.price})`
                                            )
                                          );
                                        }
                                      } else if (typeOfCampaign === "BulkOrder") {
                                        if (currentPrice >= prevPrice) {
                                          return Promise.reject(
                                            new Error(
                                              `Giá phải giảm dần so với cột mốc trước (phải nhỏ hơn ${previousMilestone.price})`
                                            )
                                          );
                                        }
                                      }
                                      return Promise.resolve();
                                    },
                                  }),
                                ]}
                              >
                                <Input placeholder="Số tiền" />
                              </Form.Item>
                              <MinusCircleOutlined onClick={() => remove(name)} />
                            </Space>
                          ))}
                          <Form.Item>
                            <Button
                              type="dashed"
                              onClick={() => add()}
                              block
                              icon={<PlusOutlined />}
                            >
                              Thêm cột mốc
                            </Button>
                          </Form.Item>
                          
                        </>
                      )}
                    </Form.List>
                  )}
                </div>
              </div>
            </div>
            <div className="col-span-3">
              {/* Main Image & DatePicker */}
              <div className="py-10 bg-white px-4 rounded-lg">
                <div>
                  <h2 className="text-xl mb-2">
                    Thời gian chiến dịch hoạt động
                  </h2>
                  <Form.Item
                    name="dateRange"
                    label="Chọn khoảng thời gian"
                    rules={[
                      {
                        required: true,
                        message: "Vui lòng chọn khoảng thời gian!",
                      },
                    ]}
                  >
                    <RangePicker
                      showTime={{ format: "HH:mm" }}
                      format="YYYY-MM-DD HH:mm"
                    />
                  </Form.Item>
                </div>
                <Form.Item>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={loading}
                    disabled={!isFormValid}
                    className="w-full"
                  >
                    Tạo chiến dịch
                  </Button>
                </Form.Item>
              </div>
            </div>
          </div>
        </Form>
      </div>
    </div>
  );
}

export default PreorderCampaignCreate;
