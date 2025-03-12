import React from "react";
import { Truck, RefreshCcw, ShieldCheck, CreditCard, Heart, Handshake, Rocket, ShoppingCart } from "lucide-react";

// Dữ liệu cho mục hỗ trợ
const benefits = [
  {
    icon: <Truck size={40} className="text-black" />, 
    title: "Miễn phí", 
    highlight: "vận chuyển toàn quốc",
  },
  {
    icon: <RefreshCcw size={40} className="text-black" />, 
    title: "Hỗ trợ", 
    highlight: "đổi trả 1-1 30 ngày",
  },
  {
    icon: <ShieldCheck size={40} className="text-black" />, 
    title: "Được đảm bảo", 
    highlight: "bởi Preorder Blindbox System",
  },
  {
    icon: <CreditCard size={40} className="text-black" />, 
    title: "Hỗ trợ", 
    highlight: "mua trước trả sau",
  },
];

// Dữ liệu cho mục cam kết dịch vụ
const commitments = [
  {
    icon: <Heart size={40} className="text-black" />, 
    title: "HỖ TRỢ", 
    subtitle: "24/7",
  },
  {
    icon: <Handshake size={40} className="text-black" />, 
    title: "ĐẢM BẢO", 
    subtitle: "UY TÍN CHẤT LƯỢNG",
  },
  {
    icon: <Rocket size={40} className="text-black" />, 
    title: "GIAO HÀNG", 
    subtitle: "NHANH CHÓNG",
  },
  {
    icon: <ShoppingCart size={40} className="text-black" />, 
    title: "MUA HÀNG", 
    subtitle: "CỰC KÌ DỄ DÀNG",
  },
];

const SupportAndCommitments = () => {
  return (
    <div className="space-y-4 mb-6">
      {/* Phần cam kết dịch vụ */}
      <div className="bg-white p-6 rounded-lg shadow-md">
        <h2 className="text-2xl font-bold text-center mb-6">CAM KẾT DỊCH VỤ</h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-4">
          {commitments.map((item, index) => (
            <div key={index} className="flex flex-col items-center">
              {item.icon}
              <p className="font-bold text-gray-800 mt-2">{item.title}</p>
              <p className="text-gray-500">{item.subtitle}</p>
            </div>
          ))}
        </div>
      </div>

      {/* Phần hỗ trợ khách hàng */}
      <div className="bg-yellow-400 p-6 rounded-lg">
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-4">
          {benefits.map((item, index) => (
            <div
              key={index}
              className="bg-white p-4 rounded-lg flex flex-col items-center justify-center shadow-md"
            >
              <div className="relative">
                {item.icon}
                <span className="absolute -bottom-1 -right-2 bg-yellow-400 text-white text-xs px-2 py-1 rounded-full">
                  ✔
                </span>
              </div>
              <p className="mt-3 text-gray-500 text-center">
                {item.title} <br />
                <span className="font-bold text-black">{item.highlight}</span>
              </p>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default SupportAndCommitments;
