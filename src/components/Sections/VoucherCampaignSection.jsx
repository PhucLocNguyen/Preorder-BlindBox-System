import { useEffect } from "react";
import { useState } from "react";
import { GetTheActiveVoucherCampaign } from "../../api/VoucherCampaign/ApiVoucherCampaign";
import useFetchData from "../../hooks/useFetchData";
import Voucher from "../Voucher";
import { Spin } from "antd";
function VoucherCampaignSection() {
  const { data, loading, refetch } = useFetchData(GetTheActiveVoucherCampaign);
  return (
    <section className="bg-gray-100 py-12 md:min-h-[500px]">
      <div
        style={{
          position: "relative",
          padding: "48px",
          borderRadius: "8px",
          backgroundImage:
            'url("https://img.pikbest.com/backgrounds/20190426/voucher-with-golden-bow_1890652.jpg!bw700',
          backgroundSize: "cover",
          backgroundPosition: "center",
          backgroundAttachment: "fixed", // Enable parallax effect
          color: "#fff", // Text color to contrast with the background
        }}
      >
        <div
          style={{
            position: "absolute",
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            backgroundColor: "rgba(0, 0, 0, 0.5)", // Semi-transparent overlay
            borderRadius: "8px",
            zIndex: 0,
          }}
        ></div>
        <div className="relative z-10">
          <h2 className="text-[36px] text-center text-white mb-10 font-bold uppercase">
            Trợ giá với mã giảm giá với số lượng có hạn
          </h2>
          <div className="grid grid-cols-12">
            {loading ? (
              <Spin />
            ) : (
              data.map((item) => {
                return (
                  <div className="col-span-4 mb-4">
                    <Voucher
                      VoucherDetail={item}
                      key={item.voucherCampaignId}
                    />
                  </div>
                );
              })
            )}
          </div>
        </div>
      </div>
    </section>
  );
}

export default VoucherCampaignSection;
