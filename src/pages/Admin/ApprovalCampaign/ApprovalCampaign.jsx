import { useCallback, useState } from "react";
import { Link, useNavigate } from "react-router";
import { GetTheActivePreorderCampaign } from "../../../api/Pre_orderCampaign/ApiPre_orderCampaign";
import { Button, Input, Modal, Pagination, Space, Spin, Table } from "antd";
import useFetchDataPagination from "../../../hooks/useFetchDataPagination";
import { EyeOutlined, EditOutlined, DeleteOutlined, SearchOutlined, PlusOutlined } from "@ant-design/icons";
const { Search } = Input;
function ApprovalCampaign() {
    const [search, setSearch] = useState("");
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

    const columns = [
        {
            title: <span style={{ fontSize: "18px" }}>{"No."}</span>,
            key: "index",
            align: "center",
            render: (_, __, index) =>
                <div className="text-center text-red-500 font-bold">
                    {(pagination.current - 1) * pagination.pageSize + index + 1}
                </div>,
        },
        {
            title: <span style={{ fontSize: "18px" }}>{"Image"}</span>,
            key: "mainImage",
            align: "center",
            render: (_, record) => (
                <div className="flex justify-center items-center">
                    <img
                        src={record.blindBox?.mainImages?.url || noThumbnailImage}
                        alt="Main"
                        className="w-24 h-24 object-cover rounded-md shadow-md transition-all duration-300 hover:scale-105"
                    />
                </div>
            ),
        },
        {
            title: <span style={{ fontSize: "18px" }}>{"Name"}</span>,
            key: "name",
            align: "center",
            sorter: (a, b) => (a.blindBox?.name || "").localeCompare(b.blindBox?.name || ""),
            render: (_, record) => <div className="text-center font-semibold">{record.blindBox?.name || "N/A"}</div>,
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
                <div className="text-center">
                    {record.startDate
                        ? new Date(record.startDate).toLocaleDateString("vi-VN", {
                            day: "2-digit",
                            month: "2-digit",
                            year: "numeric",
                        })
                        : "N/A"}
                </div>,
        },
        {
            title: <span style={{ fontSize: "18px" }}>{"Action"}</span>,
            key: "action",
            align: "center",
            render: (_, record) => (
                <Space size="middle" className="justify-between">
                    <Button className="" type="primary" htmlType="button">
                    <Link to={`/admin/preordercampaignApproval/confirm/${record.slug}`}>View detail</Link>
                    </Button>
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
                    <h2 className="text-2xl font-bold mb-4">Bulk order campaign waiting for approve</h2>
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
                                showSizeChanger
                                showQuickJumper
                                pageSizeOptions={["5", "10", "20"]}
                                onChange={(page, pageSize) => {
                                    setPageIndex(page);
                                    setPageSize(pageSize);
                                }}
                                className="shadow-md p-2 rounded-md bg-gray-100 transition-all duration-300 hover:bg-gray-200"
                            />

                        </div>
                    </>
                )}
        </div>
    </div> );
}

export default ApprovalCampaign;