import {
  Button,
  Form,
  Input,
  InputNumber,
  Select,
  DatePicker,
  Space,
} from "antd";
const { Option } = Select;
import { useEffect, useState } from "react";
import {
  ArrowLeftOutlined,
  MinusCircleOutlined,
  PlusOutlined,
} from "@ant-design/icons";
import { Link, useNavigate, useParams } from "react-router";
import ProductCardModal from "../../../components/Search/SearchBlindbox";
import TextArea from "antd/es/input/TextArea";
import {
  GetActivePreorderCampaignBySlug,
  UpdatePreorderCampaign,
} from "../../../api/Pre_orderCampaign/ApiPre_orderCampaign";
import moment from "moment";
import StatusTag from "../../../components/Tags/StatusTag";
import dayjs from "dayjs";

import isSameOrBefore from "dayjs/plugin/isSameOrBefore";

const { RangePicker } = DatePicker;

function PreorderCampaignEdit() {
dayjs.extend(isSameOrBefore);

  const { slug } = useParams();
  const [loading, setLoading] = useState(true);
  const [form] = Form.useForm();
  const [loadMainProduct, setLoadMainProduct] = useState(null);
  const [isUpdated, setIsUpdated] = useState(false);

  const [typeOfCampaign, setTypeOfCampaign] = useState(null);
  // State để xác định form có hợp lệ hay không
  const [isFormValid, setIsFormValid] = useState(false);
  const [detailPre_orderCampaign, setDetailPre_orderCampaign] = useState({});
  const [errorLog, setErrorLog] = useState(null);
  const [isValidMilestones, setIsValidMilestones] = useState(true);
  const navigate = useNavigate();

  const fetchCampaign_BySlug = async () => {
    try {
      const data = await GetActivePreorderCampaignBySlug(slug);

      setLoadMainProduct(data.blindBox);
      setDetailPre_orderCampaign(data);

      // Giả sử server trả về định dạng "YYYY-MM-DD HH:mm"
      // Nếu server trả về ISO, bạn chỉ cần moment(data.startDate) là đủ
      form.setFieldsValue({
        type: data.type === "TimedPricing" ? 0 : 1,
        dateRange: [
          data.startDate ? dayjs(data.startDate, "YYYY-MM-DD HH:mm") : null,
          data.endDate ? dayjs(data.endDate, "YYYY-MM-DD HH:mm") : null,
        ],
        milestones: data.preorderMilestones,
      });

      setTypeOfCampaign(data.type);
      setLoading(false);
    } catch (error) {
      console.error("Lỗi khi lấy dữ liệu:", error);
    }
  };
  useEffect(() => {
    fetchCampaign_BySlug();
  }, [slug]);

  // Mỗi khi field thay đổi, kiểm tra xem form có hợp lệ không
  const onFieldsChange = async () => {
    const fieldsError = form.getFieldsError();

    // Kiểm tra xem đã chọn loại campaign và sản phẩm chưa
    if (typeOfCampaign == null || !loadMainProduct) {
      setIsFormValid(false);
      return;
    }

    // Lấy giá trị milestones
    const milestoneValues = form.getFieldValue("milestones") || [];
    if (milestoneValues.length <= 2) {
      setIsFormValid(false);
      return;
    }
    console.log(milestoneValues);
    // Kiểm tra milestone nào bị trống
    const milestoneHasEmpty = await milestoneValues.some(
      (m) => !m || !m.quantity || !m.price
    );
    if (milestoneHasEmpty) {
      setIsFormValid(false);
      return;
    }
    for (let i = 1; i < milestoneValues.length; i++) {
      const prevPrice = milestoneValues[i - 1].price;
      const currentPrice = milestoneValues[i].price;
      if (typeOfCampaign == 0) {
        // Với type 0: Giá của milestone hiện tại phải > giá của milestone trước
        if (currentPrice >= prevPrice) {
          // time pricing
          setErrorLog(
            `Lỗi tại milestone ${
              i + 1
            }: Giá của mốc hiện tại (${currentPrice}) phải lớn hơn giá của mốc trước (${prevPrice}).`
          );
          setIsValidMilestones(false);
          break;
        }
      } else {
        // Với type 1: Giá của milestone hiện tại phải < giá của milestone trước
        if (currentPrice <= prevPrice) {
          // bulk order
          setErrorLog(
            `Lỗi tại milestone ${
              i + 1
            }: Giá của mốc hiện tại (${currentPrice}) phải nhỏ hơn giá của mốc trước (${prevPrice}).`
          );
          setIsValidMilestones(false);
          break;
        }
      }
      setIsValidMilestones(true);
      setErrorLog(null);
    }
    // Lấy giá trị dateRange
    const dateRange = form.getFieldValue("dateRange");
    console.log(dateRange);
    const [startDate, endDate] = dateRange.map((d) => dayjs(d));

    // So sánh với ngày hiện tại
    if (startDate.isSameOrBefore(dayjs())) {
      setIsFormValid(false);
      return;
    }

    // Kiểm tra endDate cách startDate ít nhất 3 ngày
    if (endDate.isBefore(startDate.add(3, "day"))) {
      setIsFormValid(false);
      return;
    }

    // Nếu mọi thứ OK
    setIsFormValid(true);
    console.log("By pass isUpdate");
    // Kiểm tra xem có lỗi ở bất kỳ field nào không
    const hasErrors = fieldsError.some((field) => field.errors.length > 0);

    setIsFormValid(!hasErrors);
    setIsUpdated(true);
  };

  // Khi người dùng chọn loại chiến dịch, reset milestones
  const handleChangeTypeCampaign = (value) => {
    setTypeOfCampaign(value);
    setErrorLog(null);
    form.setFieldsValue({ type: value, milestones: [] });
  };

  const handleSubmit = async (values) => {
    const data = {
      blindBoxId: parseInt(loadMainProduct?.blindBoxId) || 0,
      type: parseInt(values.type || typeOfCampaign),
      // Lấy giá trị ISO để lưu DB
      startDate: values.dateRange ? values.dateRange[0].toISOString() : null,
      endDate: values.dateRange ? values.dateRange[1].toISOString() : null,
      milestoneRequests: (values.milestones || []).map((item) => ({
        quantity: parseInt(item.quantity),
        price: parseFloat(item.price),
      })),
    };

    await UpdatePreorderCampaign(
      detailPre_orderCampaign.preorderCampaignId,
      data
    );
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
            <div className="col-span-9">
              <div className="bg-white p-4 rounded-xl py-10">
                <div className="flex items-center mb-4">
                  <Link to="/admin/pre-ordercampaign" className="h-full flex">
                    <ArrowLeftOutlined
                      style={{
                        width: "fit-content",
                        height: "100%",
                        padding: "10px",
                      }}
                      title="Về lại trang danh sách chiến dịch"
                    />
                  </Link>
                  <h1 className="text-2xl font-bold ml-2">
                    Chỉnh sửa chiến dịch
                  </h1>
                  <StatusTag status={detailPre_orderCampaign.status} />
                </div>

                {/* Thông tin sản phẩm */}
                <div className="mb-4">
                  {loadMainProduct && (
                    <div className="grid grid-cols-12 gap-6">
                      <div className="col-span-3">
                        <h3 className="text-[16px]">Hình ảnh sản phẩm</h3>
                        <div className="mt-2">
                          <img
                            src={loadMainProduct.images?.mainImage.url}
                            alt="Main"
                            className="w-full h-[290px] object-cover mt-2 rounded-md"
                          />
                        </div>
                      </div>
                      <div className="col-span-9">
                        <div className="mt-2">
                          <h3 className="text-[16px] mt-2 mb-1">
                            Tên sản phẩm
                          </h3>
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
                        <Option value={0}>Timed Pricing</Option>
                        <Option value={1}>Bulk Order</Option>
                      </Select>
                    </Form.Item>
                  </div>

                  <h3 className="text-lg">Thêm các mốc giá và số lượng</h3>
                  {typeOfCampaign !== null && (
                    <Form.List name="milestones">
                      {(fields, { add, remove }) => (
                        <>
                          {fields.map(({ key, name, ...restField }, index) => (
                            <Space
                              key={key}
                              style={{ display: "flex", marginBottom: 8 }}
                              align="baseline"
                            >
                              {/* Số lượng */}
                              <Form.Item
                                {...restField}
                                name={[name, "quantity"]}
                                label="Số lượng"
                                rules={[
                                  {
                                    required: true,
                                    message: "Vui lòng nhập số lượng!",
                                  },
                                  {
                                    validator: (_, value) => {
                                      if (!value) {
                                        return Promise.resolve();
                                      }
                                      if (Number(value) < 1) {
                                        return Promise.reject(
                                          new Error("Số lượng phải >= 1")
                                        );
                                      }
                                      return Promise.resolve();
                                    },
                                  },
                                ]}
                              >
                                <InputNumber
                                  placeholder="Số lượng"
                                  min={1}
                                  style={{ width: "100%" }}
                                />
                              </Form.Item>

                              {/* Số tiền */}
                              <Form.Item
                                {...restField}
                                name={[name, "price"]}
                                label="Số tiền"
                                dependencies={
                                  index > 0
                                    ? [["milestones", index - 1, "price"]]
                                    : []
                                }
                                rules={[
                                  {
                                    required: true,
                                    message: "Vui lòng nhập số tiền!",
                                  },
                                  {
                                    validator: (_, value) => {
                                      if (!value) {
                                        return Promise.resolve();
                                      }
                                      if (Number(value) < 1000) {
                                        return Promise.reject(
                                          new Error("Số tiền phải >= 1000")
                                        );
                                      }
                                      return Promise.resolve();
                                    },
                                  },
                                  // So sánh cột mốc trước
                                  ({ getFieldValue }) => ({
                                    validator(_, value) {
                                      const milestones =
                                        getFieldValue("milestones") || [];
                                      if (index === 0) {
                                        return Promise.resolve();
                                      }
                                      const prev = milestones[index - 1];
                                      if (!prev || !prev.price) {
                                        return Promise.resolve();
                                      }
                                      const prevPrice = parseFloat(prev.price);
                                      const currentPrice = parseFloat(value);

                                      // typeOfCampaign == 0 => Giá tăng dần
                                      // typeOfCampaign == 1 => Giá giảm dần
                                      if (typeOfCampaign === 0) {
                                        if (currentPrice <= prevPrice) {
                                          return Promise.reject(
                                            new Error(
                                              `Giá phải lớn hơn ${prevPrice} (tăng dần)`
                                            )
                                          );
                                        }
                                      } else if (typeOfCampaign === 1) {
                                        if (currentPrice >= prevPrice) {
                                          return Promise.reject(
                                            new Error(
                                              `Giá phải nhỏ hơn ${prevPrice} (giảm dần)`
                                            )
                                          );
                                        }
                                      }
                                      return Promise.resolve();
                                    },
                                  }),
                                ]}
                              >
                                <InputNumber
                                  placeholder="Số tiền"
                                  min={1000}
                                  style={{ width: "100%" }}
                                />
                              </Form.Item>
                              <MinusCircleOutlined
                                onClick={() => remove(name)}
                              />
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
                  {errorLog && (
                    <div>
                      <p className="text-red-600">{errorLog}</p>
                    </div>
                  )}
                </div>
              </div>
            </div>

            {/* Cột bên phải */}
            <div className="col-span-3">
              <div className="py-10 bg-white px-4 rounded-xl">
                <h2 className="text-xl mb-2">Thời gian chiến dịch</h2>
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

                <Form.Item>
                  <Button
                    type="primary"
                    htmlType="submit"
                    loading={loading}
                    // Sửa lại điều kiện disable
                    disabled={!isFormValid || !isUpdated || !isValidMilestones}
                    className="w-full"
                  >
                    Cập nhật
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

export default PreorderCampaignEdit;
