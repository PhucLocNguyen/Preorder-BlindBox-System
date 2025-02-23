import React, { useState, useCallback } from "react";
import { Table, Spin, Pagination, Checkbox, Input, Button, Modal, Space } from "antd";
import { GetTheActiveBlindBox } from "../../../api/BlindBox/ApiBlindBox";
import useFetchDataPagination from "../../../hooks/useFetchDataPagination";
import Pre_orderCampaignCreate from "./Pre-orderCampaignCreate";
import { PlusOutlined } from "@ant-design/icons";
const BlindboxtoAdd = () => {
    const [pageSize, setPageSize] = useState(5);
    const [pageIndex, setPageIndex] = useState(1);
    const [selectedProductId, setSelectedProductId] = useState(new Set());
    const [warning, setWarning] = useState('');
    const [searchName, setSearchName] = useState(""); // State để nhập tên sản phẩm
    const [isCreateModalVisible, setIsCreateModalVisible] = useState(false);
    const [selectedProduct, setSelectedProduct] = useState(null);
    const [selectedPre_orderCampaign, setSelectedPre_orderCampaign] = useState([]);
    const fetchPreorderCampaign = useCallback(
        () => GetTheActiveBlindBox(pageSize, pageIndex),
        [pageSize, pageIndex]
    );

    const { data, loading, refetch, pagination } = useFetchDataPagination(fetchPreorderCampaign, [
        pageSize,
        pageIndex,
    ]);

    const handleAdd_toPre_oderCampaign = () => {
        setWarning('');

        if (selectedProductId.size === 0) {
            setWarning('Please choose a product before adding blindbox in pre-order campaign');
        } else {
            const selectedKey = Array.from(selectedProductId)[0];

            // Tìm object trong mảng `data` có `blindBoxId` tương ứng
            const selectedItem = data.find(item => item.blindBoxId === selectedKey);

            if (selectedItem) {
                setSelectedPre_orderCampaign([selectedItem]); // Lưu object vào state
                setIsCreateModalVisible(true);
            } else {
                setWarning("Selected item not found.");
            }
        }
    };

    const handleCancelCreate = () => {
        setIsCreateModalVisible(false);
        setSelectedProduct(null);
    };



    const handleCheckbox = (blindBoxId) => {
        console.log("Fetched data:", data);

        setWarning('');
        setSelectedProductId(prev => {
            console.log("BlindBox ID được chọn:", blindBoxId); // Kiểm tra giá trị
            const updated = new Set(prev);
            if (updated.has(blindBoxId)) {
                updated.delete(blindBoxId);
            } else {
                updated.clear();
                updated.add(blindBoxId);
            }
            console.log("Danh sách BlindBoxId đã chọn sau cập nhật:", Array.from(updated));
            return new Set(updated);
        });
    };
    // Lọc dữ liệu dựa trên tên nhập vào
    const filteredData = data?.filter(item => item.name.toLowerCase() === searchName.toLowerCase());

    const columns = [
        {
            title: "",
            dataIndex: "blindBoxId",
            key: "blindBoxId",
            align: "center",
            render: (_, record) => {
                console.log(`Checkbox render cho blindBoxId: ${record.blindBoxId}, Checked: ${selectedProductId.has(record.blindBoxId)}`)
                return (
                    <Space size="middle">
                        <Checkbox
                            checked={selectedProductId.has(record.blindBoxId)}
                            onChange={() => handleCheckbox(record.blindBoxId)}
                        />
                    </Space>
                )
            },
        },

        {
            title: "No.",
            key: "index",
            align: "center",
            render: (_, __, index) =>
                <div className="text-center text-red-500 font-bold">{(pagination.current - 1) * pagination.pageSize + index + 1}</div>,
        },

        {
            title: "Main Image",
            align: "center",
            dataIndex: ["images", "mainImage", "url"], // Lấy ảnh từ nested object
            key: "mainImage",
            render: (url) => (
                <div className="justify-center ml-20">
                    <img
                        src={url ? url : noThumbnailImage}
                        alt="Main"
                        className="w-32 h-32 object-contain rounded-md shadow-md"
                    />
                </div>
            ),
        },
        {
            align: "center",
            title: "Name",
            key: "name",
            render: (_, record) => <div className="text-center">{record.name || "Unknown"}</div>,
        },

        {
            align: "center",
            title: "Description",
            key: "description",
            render: (_, record) => <div className="text-center">{record.description || "Unknown"}</div>,
        },

        {
            align: "center",
            title: "Size",
            key: "size",
            render: (_, record) => <div className="text-center">{record.size || "Unknown"}</div>,
        },
        {
            align: "center",
            title: "Create At",
            key: "createdAt",
            render: (_, record) =>
                <div className="text-center">{
                    record.createdAt
                        ? new Date(record.createdAt).toLocaleDateString("vi-VN", {
                            day: "2-digit",
                            month: "2-digit",
                            year: "numeric",
                        })
                        : "N/A"}</div>,
        },
    ];

    return (
        <div className="dashboard-container overflow-auto h-screen pr-4 scrollbar-thin scrollbar-thumb-gray-400 scrollbar-track-gray-200 scrollbar-thumb-opacity-80">
            <div className="w-full mx-auto mt-5 p-5 bg-white shadow-lg rounded-lg">
                <div className="flex justify-between">
                    <h2 className="text-2xl font-bold mb-4">Pre-order Campaign Management</h2>
                </div>
                <div className="flex items-center gap-4 mb-8">
                    <Input
                        placeholder="Enter blindbox's name..."
                        value={searchName}
                        onChange={(e) => setSearchName(e.target.value)}
                        className="w-full p-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                    />
                    <Button
                        className="bg-blue-500 text-white px-4 py-2 rounded shadow hover:bg-blue-600"
                        icon={<PlusOutlined />}
                        onClick={handleAdd_toPre_oderCampaign}
                    >
                        Choose to App
                    </Button>
                </div>

                {loading ? (
                    <div className="flex justify-center">
                        <Spin size="large" />
                    </div>
                ) : (
                    <>
                        {/* Chỉ hiển thị table nếu có dữ liệu khớp */}
                        {warning && (
                            <div className="text-red-500 font-bold text-center mb-4">
                                {warning}
                            </div>
                        )}
                        {filteredData.length > 0 ? (

                            <Table
                                bordered={true}
                                columns={columns}
                                dataSource={filteredData}
                                pagination={false}
                                rowKey="blindBoxId"
                                className="border rounded-md mt-8  shadow-lg"
                            />
                        ) : (
                            <p className="text-center text-gray-500 mt-5">Blindbox is not available.</p>
                        )}

                        <div className="flex justify-end mt-4">
                            <Pagination
                                current={pagination.current}
                                total={pagination.total}
                                pageSize={pagination.pageSize}
                                onChange={(page) => setPageIndex(page)}
                                className="shadow-md p-2 rounded-md"
                            />
                        </div>
                    </>
                )}
                <Modal open={isCreateModalVisible} onCancel={handleCancelCreate} footer={null} width={720} closable={false}>
                    <Pre_orderCampaignCreate
                        onSuccess={handleCancelCreate}
                        selectedProduct={selectedPre_orderCampaign.length > 0 ? selectedPre_orderCampaign[0].blindBoxId : null}
                    />
                </Modal>



            </div>
        </div>
    );
};

export default BlindboxtoAdd;
