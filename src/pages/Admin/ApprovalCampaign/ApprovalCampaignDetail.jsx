import { Form, Image, Spin, Steps, Tooltip, Progress } from "antd";
import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { GetActivePreorderCampaignBySlug } from "../../../api/Pre_orderCampaign/ApiPre_orderCampaign";

const { Step } = Steps;

function ApprovalCampaignDetail() {
  const { slug } = useParams();
  const [loading, setLoading] = useState(true);
  const [detailPreorderCampaign, setDetailPreorderCampaign] = useState(null);
  const [preorderMilestones, setPreorderMilestones] = useState([]);
  const [currentStep, setCurrentStep] = useState(0);
  const [milestoneProgress, setMilestoneProgress] = useState(0);

  // Fetch dữ liệu từ API
  const fetchCampaignBySlug = async () => {
    try {
      const data = await GetActivePreorderCampaignBySlug(slug);
      setDetailPreorderCampaign(data);
      setPreorderMilestones(data.preorderMilestones);
      calculateProgress(data.placedOrderCount, data.preorderMilestones);
      setLoading(false);
    } catch (error) {
      console.error("Error fetching campaign: ", error);
    }
  };

  useEffect(() => {
    fetchCampaignBySlug();
  }, [slug]);

  // Tính toán tiến trình hiện tại
  const calculateProgress = (placedOrders, milestones) => {
    let accumulatedOrders = 0;
    for (let i = 0; i < milestones.length; i++) {
      accumulatedOrders += milestones[i].quantity;

      if (placedOrders < accumulatedOrders) {
        // Tính phần trăm trong milestone hiện tại
        const prevOrders = accumulatedOrders - milestones[i].quantity;
        const progress =
          ((placedOrders - prevOrders) / milestones[i].quantity) * 100;

        setCurrentStep(i);
        setMilestoneProgress(progress);
        return;
      }
    }
    // Nếu đã hoàn thành tất cả milestones
    setCurrentStep(milestones.length);
    setMilestoneProgress(100);
  };

  return (
    <div>
      {loading ? (
        <div>
          <Spin size="large" />
        </div>
      ) : (
        <>
          <div className="mt-8">
            <h2 className="text-[24px]">Thông tin chi tiết của chiến dịch</h2>

            {/* Ngày & Trạng thái */}
            <div className="flex gap-3 text-lg my-4">
              <span className="bg-blue-100 text-blue-600 px-5 py-2 rounded-md">
                Start:{" "}
                {new Date(
                  detailPreorderCampaign.startDate
                ).toLocaleDateString()}
              </span>
              <span className="bg-red-100 text-red-600 px-5 py-2 rounded-md">
                End:{" "}
                {new Date(detailPreorderCampaign.endDate).toLocaleDateString()}
              </span>
              <span
                className={`px-5 py-2 rounded-md ${
                  detailPreorderCampaign.status === "Pending"
                    ? "bg-yellow-100 text-yellow-600"
                    : "bg-green-100 text-green-600"
                }`}
              >
                {detailPreorderCampaign.status || "No status"}
              </span>
              <span className="bg-gray-100 text-gray-800 px-5 py-2 rounded-md">
                {detailPreorderCampaign.type || "No type"}
              </span>
            </div>

            {/* Hình ảnh */}
            <div className="flex gap-4 max-[450px] overflow-y-scroll">
              <Image.PreviewGroup>
                <div className="w-[300px] bg-gray-100 object-cover rounded-xl">
                  <Image
                    src={detailPreorderCampaign.blindBox.images?.mainImage.url}
                    alt="Main"
                    style={{ borderRadius: "20px" }}
                  />
                  <div className="flex flex-1 bg-gray-100 p-2 gap-2 rounded-xl flex-wrap">
                    {detailPreorderCampaign.blindBox.images.galleryImages.map(
                      (item) => (
                        <span key={item.imageId} className={"relative w-1/6"}>
                          <Image src={item.url} className="rounded-xl" />
                        </span>
                      )
                    )}
                  </div>
                </div>
              </Image.PreviewGroup>

              {/* Thông tin sản phẩm */}
              <div className="flex-1 p-4">
                <h3 className="text-xl font-bold text-gray-800">
                  {detailPreorderCampaign?.blindBox?.name ||
                    "No name available"}
                </h3>
                <p className="text-lg mb-2 rounded-xl bg-gray-100 p-2 text-gray-600 mt-3">
                  {detailPreorderCampaign?.blindBox?.description ||
                    "No description available"}
                </p>
                <p className="text-xl font-semibold">
                  Size:{" "}
                  {detailPreorderCampaign?.blindBox?.size ||
                    "No size available"}
                </p>
              </div>
            </div>

            <div className="bg-gray-100 mt-4 p-2">
              <h2 className="text-[24px]">Tiến trình đạt mục tiêu</h2>

              <Steps progressDot current={currentStep} size="default">
                {preorderMilestones.map((milestone, index) => {
                  // Tính tổng đơn tích lũy đến milestone hiện tại
                  const cumulativeOrders = preorderMilestones
                    .slice(0, index + 1)
                    .reduce((sum, m) => sum + m.quantity, 0);

                  return (
                    <Step
                      key={index}
                      title={
                        <Tooltip
                          title={`Mức giá: ${milestone.price.toLocaleString()} VND`}
                        >
                          Mốc {milestone.milestoneNumber}
                        </Tooltip>
                      }
                      description={
                        index === currentStep ? (
                          <Progress
                            percent={
                              (detailPreorderCampaign.placedOrderCount /
                                cumulativeOrders) *
                              100
                            }
                            size="small"
                            status="active"
                          />
                        ) : (
                          `Đạt ${cumulativeOrders} đơn`
                        )
                      }
                    />
                  );
                })}
              </Steps>
            </div>
          </div>
        </>
      )}
    </div>
  );
}

export default ApprovalCampaignDetail;
