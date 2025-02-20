import React, { useState, useEffect, useCallback } from "react";
import { Table, Tag, Space, Input, Button, Modal, Checkbox, Spin, Pagination, notification } from "antd";
import { EyeOutlined, EditOutlined, DeleteOutlined, SearchOutlined, PlusOutlined } from "@ant-design/icons";
import { useNavigate } from 'react-router-dom';
import Pre_orderCampaignEdit from "./Pre-orderCampaignEdit";
import Pre_orderCampaignCreate from "./Pre-orderCampaignCreate";
import axios from 'axios';
import { GetTheActivePreorderCampaign } from "../../../api/Pre_orderCampaign/ApiPre_orderCampaign";
import useFetchDataPagination from "../../../hooks/useFetchDataPagination";
import noThumbnailImage from "../../../assets/noThumbnailImage.jpg"
const { Search } = Input;

const Pre_orderCampaign = () => {
    const [selectedPre_orderCampaign, setSelectedPre_orderCampaign] = useState([]);
    const [search, setSearch] = useState("");
    const [isCreateModalVisible, setIsCreateModalVisible] = useState(false);
    const [isEditModalVisible, setIsEditModalVisible] = useState(false);
    const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
    const [Pre_orderCampaignToDelete, setPre_orderCampaignToDelete] = useState(null);
    const [selectedPre_orderCampaign_Id, setSelectedPre_orderCampaign_Id] = useState(new Set());
    const [warning, setWarning] = useState('');
    const [pageSize, setPageSize] = useState(5);
    const [pageIndex, setPageIndex] = useState(1);
    const navigate = useNavigate();

    const fetchPreorderCampaign = useCallback(
        () => GetTheActivePreorderCampaign(pageSize, pageIndex),
        [pageSize, pageIndex]
    );
    const { data, loading, refetch, pagination } = useFetchDataPagination(fetchPreorderCampaign, [
        pageSize,
        pageIndex,
    ]);

    const handleAddPre_orderCampaign = () => {

        setIsCreateModalVisible(true);

    };

    const handleEditPre_orderCampaign = (preorderCampaignId) => {
        setSelectedPre_orderCampaign(preorderCampaignId);
        setIsEditModalVisible(true);
    };
    const handleViewPre_orderCampaign = (preorderCampaignId) => {
        navigate(`/admin/pre-ordercampaign-details/${preorderCampaignId}`);
    };

    const handleCancelCreate = () => {
        setIsCreateModalVisible(false);
        setSelectedPre_orderCampaign(null);
    };

    const handleCancelEdit = () => {
        setIsEditModalVisible(false);
    };

    const handleDeletePre_orderCampaign = (record) => {
        setPre_orderCampaignToDelete(record);
        setIsDeleteModalVisible(true);
    };


    const confirmDeletePre_orderCampaign = async (preorderCampaignId) => {
        try {
            const response = await axios.delete(`https://preorderblindboxsystem-c9ftb6dtcvdkh3ge.centralus-01.azurewebsites.net/api/PreorderCampaign/${preorderCampaignId}`);
            if (response.data && response.data.result) {
                const { isDeleted } = response.data.result;

                if (isDeleted === false) {
                    const element = document.getElementById(preorderCampaignId);
                    if (element) {
                        element.setAttribute("isDeleted", "true");
                    }
                }
            }

        } catch (error) {
            console.error('Error deleting Pre_orderCampaign:', error);
            return <div>Error: {error}</div>;
        }
        refetch();
        notification.success({
            message: 'Success',
            description: 'The campaign has been successfully deleted.',
        });


    };
    const columns = [
        {
            title: "No.",
            key: "index",
            render: (_, __, index) => (pagination.current - 1) * pagination.pageSize + index + 1,
        },
        {
            title: "Image",
            key: "mainImage",
            render: (_, record) => (
                <img
                    src={record.blindBox?.mainImages?.url || noThumbnailImage}
                    alt="Main"
                    className="w-32 h-32 object-contain rounded-md shadow-md"
                />
            ),
        },
        {
            title: "Name",
            key: "name",
            render: (_, record) => record.blindBox?.name || "N/A",
        },
        {
            title: "Type",
            render: (_, record) => record.type || "Unknown",
            key: "type",
        },
        {
            title: "Size",
            key: "size",
            render: (_, record) => record.blindBox?.size || "Unknown",
        },
        {
            title: "Start Date",
            key: "startDate",
            render: (_, record) =>
                record.startDate
                    ? new Date(record.startDate).toLocaleDateString("vi-VN", {
                        day: "2-digit",
                        month: "2-digit",
                        year: "numeric",
                    })
                    : "N/A",
        },
        {
            title: "Action",
            key: "action",
            render: (_, record) => (
                <Space size="middle">
                    <EyeOutlined style={{ color: "blue", fontSize: "18px" }} onClick={() => handleViewPre_orderCampaign(record)} />
                    <EditOutlined style={{ color: "orange", fontSize: "18px" }} onClick={() => handleEditPre_orderCampaign(record)} />
                    <DeleteOutlined style={{ color: "red", fontSize: "18px" }} onClick={() => handleDeletePre_orderCampaign(record)} />
                </Space>
            ),
        },
    ];

    const filteredData = data.filter((item) =>
        item.blindBox?.name?.toLowerCase().includes(search.toLowerCase())
    );

    useEffect(() => {
        console.log(pagination);
    }, [filteredData]);


    return (
        <div className="dashboard-container overflow-auto h-screen pr-4 scrollbar-thin scrollbar-thumb-gray-400 scrollbar-track-gray-200 scrollbar-thumb-opacity-80">
            <div className="w-full mx-auto mt-5 p-5 bg-white shadow-lg rounded-lg">
                <div className="flex justify-between">
                    <h2 className="text-2xl font-bold mb-4">Pre-order Campaign Management</h2>
                    <div>
                        <Button className="bg-blue-500 text-white px-4 py-2 rounded mt-8" icon={<PlusOutlined />} onClick={handleAddPre_orderCampaign}>
                            Add New Campaign
                        </Button>
                    </div>
                </div>

                <Search placeholder="Search products..." allowClear enterButton={<SearchOutlined />} style={{ width: 300 }} onChange={(e) => setSearch(e.target.value)} />

                {warning && <div className="w-full text-right text-red-500 mt-2">{warning}</div>}

                {loading ? (
                    <div className="flex justify-center ">
                        <Spin size="large" />
                    </div>
                ) : (
                    <>
                        <Table
                            bordered={true}
                            columns={columns}
                            dataSource={filteredData}
                            pagination={false}
                            rowKey="id"

                            className="border rounded-md mt-5"
                        />


                        <div className="flex justify-end mt-4">
                            <Pagination
                                current={pagination.current}
                                total={pagination.total}
                                pageSize={pagination.pageSize}
                                onChange={(page) => {
                                    setPageIndex(page);
                                }}
                                className="shadow-md p-2 rounded-md"
                            />
                        </div>
                    </>
                )}

                <Modal open={isCreateModalVisible} onCancel={handleCancelCreate} footer={null} width={720} closable={false}>
                    <Pre_orderCampaignCreate onSuccess={handleCancelCreate} selectedPre_orderCampaign={selectedPre_orderCampaign} />
                </Modal>
                <Modal open={isEditModalVisible} onCancel={handleCancelEdit} footer={null} width={720} closable={false}>
                    <Pre_orderCampaignEdit onSuccess={handleCancelEdit} selectedPre_orderCampaign={selectedPre_orderCampaign} />
                </Modal>

                <Modal
                    title="Delete product"
                    open={isDeleteModalVisible}
                    onCancel={() => setIsDeleteModalVisible(false)}
                    footer={[
                        <Button key="cancel" onClick={() => setIsDeleteModalVisible(false)}>
                            Cancel
                        </Button>,
                        <Button key="delete" type="primary" danger onClick={() => confirmDeletePre_orderCampaign(Pre_orderCampaignToDelete?.preorderCampaignId)}>
                            Delete
                        </Button>,
                    ]}
                >
                    <p>Do you want to delete the product "{Pre_orderCampaignToDelete?.preorderCampaignId}"?</p>
                </Modal>
            </div>
        </div>
    );
};

export default Pre_orderCampaign;
