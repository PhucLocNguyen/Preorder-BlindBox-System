import { AnimatePresence, motion } from "framer-motion";
import { useState } from "react";
import { CollectActiveVoucherCampaign } from "../api/VoucherCampaign/ApiVoucherCampaign";
import { IsExpired } from "../utils/DateChecking";
import { formatShortVND } from "../utils/FormatMoney";
import GradientButton from "./Buttons/GradientButton";
function Voucher({
  VoucherDetail = {
    voucherCampaignId: 0,
    name: "Voucher giảm giá 10% tối đa 50k",
    maximumMoneyDiscount: 10000,
    maximumUserCanGet: 0,
    takenQuantity: 0,
    quantity: 0,
    startDate:"2025-02-16 13:56:40.637",
    endDate: "2025-02-18 13:56:40.637",
    percentDiscount: 10,
    isCollected: false,
  },
}) {
  const [isCollectedState, setIsCollectedState] = useState(VoucherDetail.isCollected);
  const [popupReceiveQuantity, setPopupReceiveQuantity] = useState(0);
  const [showPopup, setShowPopup] = useState(false);

  const collectVoucher = async (voucherDetailID) => {
    // const response = await CollectActiveVoucherCampaign({
    //   voucherDetailID: voucherDetailID,
    // });

    // Hiển thị popup số lượng nhận được
    // setPopupReceiveQuantity(response.quantity);
    setPopupReceiveQuantity(VoucherDetail.maximumUserCanGet);

    setIsCollectedState(true);
    setShowPopup(true);

    // Ẩn popup sau 1.2 giây
    setTimeout(() => {
      setShowPopup(false);
    }, 1200);
  };

  return (
    <div className="relative card-con grid grid-cols-12 z-10">
      {/* Vùng chứa hiệu ứng răng cưa */}
      <div
        className="relative z-[11] w-full col-span-3 min-h-[250px] rounded-lg bg-gradient-to-b from-[#FBA518] to-[#A89C29] backdrop-blur-lg shadow-lg"
        style={{
          maskImage:
            "radial-gradient(circle at 10px center, transparent 10px, red 10.5px)",
          maskPosition: "-10px center",
          maskSize: "100% 48px",
        }}
      ></div>

      {/* Nội dung voucher */}
      <div className="relative w-full col-span-9 h-full bg-white -ml-4 z-20 rounded-r-lg border-l-2 border-[#F9CB43] p-2 text-[#000]">
        <h3 className="text-[#000] text-lg">
          <b>{VoucherDetail.Name}</b>
        </h3>
        <p>
          Giảm giá {VoucherDetail.percentDiscount + "%"} tối đa {formatShortVND(VoucherDetail.maximumMoneyDiscount)}
        </p>
        <p>
          Số lượng có hạn - đã lấy {VoucherDetail.takenQuantity} trên {VoucherDetail.quantity}
        </p>

        {/* Badge số lượng tối đa user có thể lấy */}
        <div className="absolute badge z-[22] -right-2 top-4 w-fit h-fit bg-[#cf1e1e] py-2 px-4 rounded-lg border-2 border-white">
          <p className="text-white">
            X {VoucherDetail.maximumUserCanGet}
          </p>
        </div>

        {/* Kiểm tra nếu hết hạn hoặc đã nhận voucher */}
        {IsExpired(VoucherDetail.endDate) ? (
          <GradientButton text="Đã hết hạn rồi" />
        ) : !isCollectedState ? (
          <GradientButton
            text="Nhận voucher"
            onClick={() => {
              collectVoucher(VoucherDetail.voucherCampaignId);
            }}
          />
        ) : (
          <GradientButton text="Đã nhận rồi" disabled={true}/>
        )}
        {/* Hiển thị popup số lượng voucher nhận được */}
      <AnimatePresence>
        {showPopup && (
          <motion.div
            className="absolute z-50 transform -translate-x-1/2 -translate-y-1/2 bg-[#fcd34d] text-[#000] py-2 px-6 rounded-md shadow-lg text-lg font-semibold"
            initial={{ opacity: 0, scale: 0.8 }}
            animate={{ opacity: 1, scale: 1, y:-10 }}
            exit={{ opacity: 0, scale: 0.8 }}
            transition={{ duration: 0.5 }}
          >
            + {popupReceiveQuantity} voucher
          </motion.div>
        )}
      </AnimatePresence>
      </div>

      
    </div>
  );
}

export default Voucher;
