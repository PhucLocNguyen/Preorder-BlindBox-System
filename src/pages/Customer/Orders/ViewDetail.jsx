import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { GetAllOrderDetailsByOrderID } from "../../../api/OrderDetail/ApiOrderDetail";
import { GetOrderById } from "../../../api/Order/ApiOrder";
import OrderTrackingSteps from "../../../components/Steps/OrderTrackingSteps";
import { Breadcrumb, Card, Spin, Tag } from "antd";
import { HomeOutlined, ShoppingOutlined } from "@ant-design/icons";
import { Link } from "react-router";
import TableListOrderDetail from "../../../components/OrderDetails/TableListOrderDetail";
function ViewDetail() {
  const { id } = useParams();
  const [orderInformation, setOrderInformation] = useState({
    orderId: 1,
    userVoucherId: null,
    customerId: 1,
    createdDate: "15:30:00 27/02/2025",
    amount: 150000,
    receiver: "Nguyễn Văn An",
    receiverPhone: "0987654321",
    receiverAddress: "123 Đường Lý Thái Tổ, Quận 10, TP.HCM",
    status: "delivered",
  });
  const [userVoucherById, setUserVoucherById] = useState({
    userVoucherId: 1,
    voucherCampaignId: 1,
    name: "Summer Sale",
    percentDiscount: 10,
    maximumMoneyDiscount: 100,
    quantity: 1,
    usedQuantity: 0,
    createdDate: "2025-02-27T23:20:43.197",
  });
  const [orderDetails, setOrderDetails] = useState([
    {
      orderDetailId: 1,
      blindBox: {
        blindBoxId: 1,
        name: "Hộp Bí Ẩn A",
        description: "Một hộp đầy bất ngờ thú vị",
        listedPrice: 300000,
        size: "Nhỏ",
        createdAt: "2025-02-27T23:20:43.197",
        images: {
          mainImage: {
            imageId: 1,
            url: "https://product.hstatic.net/200000863773/product/mo-hinh-do-choi-cqtoys-bao-ao-blindbox-cuddle_25536e0b75f84a4b8f4c92da5f698847.png",
            isMainImage: true,
            createdAt: "2025-02-27T23:20:43.197",
          },
          galleryImages: [],
        },
      },
      quantity: 5,
      unitEndCampaignPrice: 12000,
      amount: 60000,
    },
    {
      orderDetailId: 2,
      blindBox: {
        blindBoxId: 2,
        name: "Hộp Bí Ẩn B",
        description: "Nhiều điều bất ngờ hơn nữa",
        listedPrice: 3500000,
        size: "Trung bình",
        createdAt: "2025-02-27T23:20:43.197",
        images: {
          mainImage: {
            imageId: 2,
            url: "https://pos.nvncdn.com/71a8b2-3946/ps/20241212_YReXWDols5.png",
            isMainImage: true,
            createdAt: "2025-02-27T23:20:43.197",
          },
          galleryImages: [],
        },
      },
      quantity: 2,
      unitEndCampaignPrice: 15000,
      amount: 30000,
    },
    {
      orderDetailId: 3,
      blindBox: {
        blindBoxId: 3,
        name: "Hộp Bí Ẩn C",
        description: "Bất ngờ đặc biệt dành riêng cho bạn",
        listedPrice: 5000000,
        size: "Lớn",
        createdAt: "2025-02-27T23:20:43.197",
        images: {
          mainImage: {
            imageId: 3,
            url: "https://cdn-icons-png.flaticon.com/512/1995/1995574.png",
            isMainImage: true,
            createdAt: "2025-02-27T23:20:43.197",
          },
          galleryImages: [],
        },
      },
      quantity: 3,
      unitEndCampaignPrice: 20000,
      amount: 60000,
    },
  ]);
  const [loading, setLoading] = useState(true);
  const getDetailOrder = async () => {
    var responseOrderDetails = await GetAllOrderDetailsByOrderID(id);
    var responseOrderInformation = await GetOrderById(id);

    // setOrderInformation(responseOrderInformation);
    // setOrderDetails(responseOrderDetails);
    setLoading(false);
  };
  useEffect(() => {
    getDetailOrder();
  }, []);
  const totalAmount = orderDetails.reduce((acc, item) => acc + item.amount, 0);

  const discountAmount =
    userVoucherById &&
    totalAmount * (userVoucherById.percentDiscount / 100) >
      userVoucherById.maximumMoneyDiscount
      ? userVoucherById.maximumMoneyDiscount
      : totalAmount * (userVoucherById?.percentDiscount / 100) || 0;
  return (
    <div className="max-w-screen-xl w-full mx-auto min-h-screen">
      <Breadcrumb
        items={[
          {
            title: (
              <Link to="/">
                <HomeOutlined />
                <span className="ml-1">Trang chủ</span>
              </Link>
            ),
          },
          {
            title: (
              <Link to="/myorder">
                <ShoppingOutlined />
                <span className="ml-1">Đơn hàng của tôi</span>
              </Link>
            ),
          },
          {
            title: <span className="ml-1">Xem chi tiết đơn hàng</span>,
          },
        ]}
      />

      {loading ? (
        <div>
          <Spin />
        </div>
      ) : (
        <div className="py-2">
          <h2 className="text-2xl mb-2">Chi tiết đơn hàng #{id}</h2>
          <Card title="Trạng thái của đơn hàng">
            <OrderTrackingSteps
              key={"OrderTracking"}
              status={orderInformation.status}
            />
          </Card>
          <div className="grid grid-cols-12">
            <div className="col-span-9">
              <Card title="Thông tin người nhận đơn hàng" bordered={true}>
                <p>
                  <b>Người nhận:</b> {orderInformation.receiver}
                </p>
                <p>
                  <b>SĐT:</b> {orderInformation.receiverPhone}
                </p>
                <p>
                  <b>Địa chỉ giao hàng:</b> {orderInformation.receiverAddress}
                </p>
                <p>
                  <b>Ngày đặt hàng:</b> {orderInformation.createdDate}
                </p>
                <p>
                  <b>Tổng tiền:</b> {orderInformation.amount.toLocaleString()}{" "}
                  VND
                </p>
              </Card>
              <Card title="Các sản phẩm được mua">
                <TableListOrderDetail orderDetails={orderDetails} />
              </Card>
            </div>
            <div className="col-span-3">
              <Card title="Thanh toán">
                <div className="bg-white p-4 rounded-lg shadow">
                  <h2 className="text-lg font-bold mb-2">Payment</h2>
                  <div className="space-y-2">
                    <div className="flex justify-between">
                      <p>Subtotal</p>
                      <p>
                        {totalAmount}
                        VND
                      </p>
                    </div>
                    <div className="flex justify-between">
                      <p>
                        Discount (
                        {userVoucherById.length === 0
                          ? "0"
                          : userVoucherById.percentDiscount}
                        %)
                      </p>
                      <p>
                        -{discountAmount}
                        VND
                      </p>
                    </div>
                    <div className="flex justify-between font-bold">
                      <p>Total</p>
                      <p>{orderInformation.amount} VND</p>
                    </div>
                  </div>
                </div>
              </Card>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default ViewDetail;
