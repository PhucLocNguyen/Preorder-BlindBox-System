import React, { useState, useEffect, useCallback } from "react";
import { Table, Tag, Space, Input, Button, Modal, Checkbox, Spin, Pagination, notification } from "antd";
import { EyeOutlined, EditOutlined, DeleteOutlined, SearchOutlined, PlusOutlined } from "@ant-design/icons";
import { useNavigate } from 'react-router-dom';
import Pre_orderCampaignEdit from "./Pre-orderCampaignEdit";
import Pre_orderCampaignCreate from "./Pre-orderCampaignCreate";
import axios from 'axios';

import { GetTheActivePreorderCampaign, GetActivePreorderCampaignBySlug } from "../../../api/Pre_orderCampaign/ApiPre_orderCampaign";
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
    const [warning, setWarning] = useState('');
    const [pageSize, setPageSize] = useState(5);
    const [pageIndex, setPageIndex] = useState(1);
    const navigate = useNavigate();
    const [detailPre_orderCampaign, setDetailPre_orderCampaign] = useState({});


    const fetchPreorderCampaign = useCallback(
        () => GetTheActivePreorderCampaign(pageSize, pageIndex),
        [pageSize, pageIndex]
    );
    const { data, loading, refetch, pagination } = useFetchDataPagination(fetchPreorderCampaign, [
        pageSize,
        pageIndex,
    ]);

    useEffect(() => {
        const getDetailPre_orderCampaign_bySlug = async () => {
            var data = await GetActivePreorderCampaignBySlug(record.slug);
            setDetailPre_orderCampaign(data);
        };
        getDetailPre_orderCampaign_bySlug();
        console.log(pagination);
    }, [data]);

    const handleAddPre_orderCampaign = () => {
        navigate("create");
        // setIsCreateModalVisible(true);
    };

    const handleEditPre_orderCampaign = () => {
        setIsEditModalVisible(true);
    };
    const handleViewPre_orderCampaign = (record) => {
        navigate(`/admin/pre-ordercampaign-details/${record.slug}`);
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
            title: <span style={{ fontSize: "18px" }}>{"No."}</span>,
            key: "index",
            align: "center",
            render: (_, __, index) =>
                <div className="text-center text-red-500 font-bold">{(pagination.current - 1) * pagination.pageSize + index + 1}</div>,
        },

        {
            title: <span style={{ fontSize: "18px" }}>{"Image"}</span>,
            key: "mainImage",
            align: "center",
            render: (_, record) => (
                <div className="justify-center ml-10">
                    <img
                        src={record.blindBox?.mainImages?.url || noThumbnailImage}
                        alt="Main"
                        className="w-32 h-32 object-contain rounded-md shadow-md"
                    />
                </div>
            ),
        },

        {
            title: <span style={{ fontSize: "18px" }}>{"Name"}</span>,
            key: "name",
            align: "center",
            render: (_, record) => <div className="text-center">{record.blindBox?.name || "N/A"}</div>,
        },

        {
            title: <span style={{ fontSize: "18px" }}>{"Type"}</span>,
            key: "type",
            align: "center",
            render: (_, record) => <div className="text-center">{record.type || "Unknown"}</div>,

        },

        {
            title: <span style={{ fontSize: "18px" }}>{"Size"}</span>,
            key: "size",
            align: "center",
            render: (_, record) => <div className="text-center">{record.blindBox?.size || "Unknown"}</div>,
        },

        {
            title: <span style={{ fontSize: "18px" }}>{"Start Date"}</span>,
            key: "startDate",
            align: "center",
            render: (_, record) =>
                <div className="text-center">{
                    record.startDate
                        ? new Date(record.startDate).toLocaleDateString("vi-VN", {
                            day: "2-digit",
                            month: "2-digit",
                            year: "numeric",
                        })
                        : "N/A"}</div>,
        },

        {
            title: <span style={{ fontSize: "18px" }}>{"Action"}</span>,
            key: "action",
            align: "center",
            render: (_, record) => (
                <Space size="middle" className="justify-between ml-1">

                    <EyeOutlined style={{ color: "blue", fontSize: "25px" }} onClick={() => handleViewPre_orderCampaign(record)} />
                    <EditOutlined style={{ color: "orange", fontSize: "25px" }} onClick={() => handleEditPre_orderCampaign(record)} />
                    <DeleteOutlined style={{ color: "red", fontSize: "25px" }} onClick={() => handleDeletePre_orderCampaign(record)} />

                </Space>
            ),
        },
    ];

    const filteredData = data.filter((item) =>
        item.blindBox?.name?.toLowerCase().includes(search.toLowerCase())
    );




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
