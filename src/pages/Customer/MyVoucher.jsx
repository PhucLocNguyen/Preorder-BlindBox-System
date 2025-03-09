import React, { useEffect, useState } from "react";
import { Spin } from "antd";
import {GetAllUserVoucher} from '../../api/UserVoucher/ApiUserVoucher';

// Thay ảnh nền phù hợp
import VoucherCampaignBackground from "../../assets/Background/voucherCampaignBackgroundImage.png";

function MyVoucher() {
  const [vouchers, setVouchers] = useState([]);
  const [loading, setLoading] = useState(true);

  // Hàm gọi API
  const fetchAllUserVouchers = async () => {
    try {
      setLoading(true);
      const data = await GetAllUserVoucher();
      setVouchers(data);
    } catch (error) {
      console.log(">>> Error fetching user vouchers: ", error);
    } finally {
      setLoading(false);
    }
  };

  // Gọi API khi component mount
  useEffect(() => {
    fetchAllUserVouchers();
  }, []);

  return (
    <section className="bg-gray-100 py-12 md:min-h-[500px]">
      {/* Khối bao nền + parallax + overlay */}
      <div
        style={{
          position: "relative",
          padding: "48px",
          borderRadius: "8px",
          backgroundImage: `url(${VoucherCampaignBackground})`,
          backgroundSize: "cover",
          backgroundPosition: "center",
          backgroundAttachment: "fixed", // Tạo hiệu ứng parallax
          color: "#fff",
        }}
      >
        {/* Overlay mờ để chữ phía trên nổi bật */}
        <div
          style={{
            position: "absolute",
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            backgroundColor: "rgba(0, 0, 0, 0.5)",
            borderRadius: "8px",
            zIndex: 0,
          }}
        ></div>

        {/* Nội dung chính */}
        <div className="relative z-10">
          <h2 className="text-[36px] text-center text-white mb-10 font-bold uppercase">
            Danh sách Voucher của bạn
          </h2>

          <div className="grid grid-cols-12">
            {loading ? (
              // Hiển thị loading
              <div className="col-span-12 flex justify-center items-center">
                <Spin />
              </div>
            ) : vouchers && vouchers.length > 0 ? (
              // Nếu có voucher
              vouchers.map((voucher) => (
                <div key={voucher.userVoucherId} className="col-span-12 md:col-span-4 mb-4">
                  {/* Tuỳ chọn: dùng component <Voucher /> nếu muốn giống hệt giao diện voucher campaign
                      <Voucher VoucherDetail={voucher} />
                      Hoặc custom hiển thị riêng:
                  */}
                  <div className="bg-white text-black p-4 rounded shadow-md">
                    <p className="font-bold">Mã Voucher: {voucher.voucherCode}</p>
                    <p>Giảm giá: {voucher.discount}%</p>
                    <p>Ngày hết hạn: {voucher.expiryDate}</p>
                  </div>
                </div>
              ))
            ) : (
              // Nếu rỗng
              <div className="col-span-12 text-center">
                <p>Hiện chưa có voucher nào.</p>
              </div>
            )}
          </div>
        </div>
      </div>
    </section>
  );
}

export default MyVoucher;
