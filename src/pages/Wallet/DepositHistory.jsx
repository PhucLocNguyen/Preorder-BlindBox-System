import React, { useState, useCallback } from "react";
import { Table, Space, Button, Pagination, Spin, DatePicker, Dropdown } from "antd";
import useFetchDataPagination from "../../hooks/useFetchDataPagination";
import { MoreOutlined, EyeOutlined } from '@ant-design/icons'
import { GetListOfAllTransactionByUser } from "../../api/Transaction/ApiTransaction";
import { Link } from "react-router-dom";
const DepositHistory = () => {
    const [pageSize, setPageSize] = useState(5);
    const [pageIndex, setPageIndex] = useState(1);
    // Fetch dữ liệu giao dịch
    const fetchListOfAllTransactionByUser = useCallback(async () => {
        const authData = document.cookie
            .split("; ")
            .find((row) => row.startsWith("auth="))
            ?.split("=")[1];

        if (!authData) {
            console.error("Không tìm thấy accessToken!");
            return { data: [], pagination: null };
        }

        const { accessToken } = JSON.parse(decodeURIComponent(authData));

        return await GetListOfAllTransactionByUser(accessToken, pageIndex, pageSize,
        );
    }, [pageSize, pageIndex]);

    const { data: transactions, loading, pagination } = useFetchDataPagination(fetchListOfAllTransactionByUser, [
        pageSize, pageIndex
    ]);


    const columns = [
        {
            title: "STT",
            key: "index",
            align: "center",
            render: (_, __, index) => (
                <div className="text-center font-bold text-red-500">
                    {(pagination.current - 1) * pagination.pageSize + index + 1}
                </div>
            ),
        },
        {
            title: "Nội Dung",
            dataIndex: "description",
            key: "description",
            align: "center",
            render: (text) => <div className="text-center">{text || "N/A"}</div>,
        },
        {
            title: "Số tiền",
            dataIndex: "money",
            key: "money",
            align: "center",
            render: (money) => (
                <div className="text-center font-semibold">
                    {money.toLocaleString("vi-VN")} VNĐ
                </div>
            ),
        },
        {
            title: "Loại giao dịch",
            dataIndex: "type",
            key: "type",
            align: "center",
            render: (type) => <div className="text-center">{type}</div>,
        },
        {
            title: "Thời gian giao dịch",
            dataIndex: "createdDate",
            key: "createdDate",
            align: "center",
            render: (date) =>
                date
                    ? new Date(date).toLocaleString("vi-VN", {
                        day: "2-digit",
                        month: "2-digit",
                        year: "numeric",
                        hour: "2-digit",
                        minute: "2-digit",
                        second: "2-digit",
                        hour12: false, // Hiển thị giờ ở dạng 24h
                    })
                    : "N/A",
        },
        {
            title: "Hành động",
            key: "action",
            align: "center",
            render: (_, record) => (
                <Dropdown
                    menu={{
                        items: [
                            {
                                key: "0",
                                label: (
                                    <Link
                                        to={`details/${record.transactionId}`}
                                        className="px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                                    >
                                        <EyeOutlined /> <span>Xem chi tiết giao dịch</span>
                                    </Link>
                                ),
                            },
                        ]
                    }}
                    trigger={['click']}
                    placement="bottomLeft"
                >
                    <button
                        onClick={(e) => e.preventDefault()}
                        className="inline-flex items-center justify-center text-gray-400 text-2xl gap-1"
                    >
                        <i className="fas fa-ellipsis-v" />
                        <MoreOutlined />
                    </button>
                </Dropdown>
            ),
        },
    ];

    return (
        <div className="p-6 grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Cashflow Section */}
            <div className="bg-white p-6 rounded-2xl shadow-lg w-full">
                <div className="flex justify-between items-center">
                    <h2 className="text-xl font-semibold text-gray-800">Cashflow</h2>
                    <button className="bg-blue-500 text-white p-2 rounded-lg hover:bg-blue-600">
                        ➝
                    </button>
                </div>

                {/* Daily, Weekly, Monthly Stats */}
                <div className="mt-4 grid grid-cols-3 text-center text-gray-600">
                    <div>
                        <p className="text-sm">Daily</p>
                        <p className="text-lg font-bold">285,39 $</p>
                    </div>
                    <div>
                        <p className="text-sm">Weekly</p>
                        <p className="text-lg font-bold">1.487,79 $</p>
                    </div>
                    <div>
                        <p className="text-sm">Monthly</p>
                        <p className="text-lg font-bold">7.400,00 $</p>
                    </div>
                </div>

                {/* Dropdown */}
                <div className="mt-4 text-gray-700">
                    <p className="inline-block font-medium">Last Month</p>
                    <span className="ml-2 text-gray-500">▼</span>
                </div>

                {/* Circular Chart */}
                <div className="mt-6 flex items-center justify-center relative">
                    <div className="relative w-40 h-40 flex items-center justify-center">
                        <svg className="absolute w-full h-full" viewBox="0 0 100 100">
                            <circle cx="50" cy="50" r="45" stroke="#E67E22" strokeWidth="5" fill="none" strokeDasharray="40,100" />
                            <circle cx="50" cy="50" r="45" stroke="#3498DB" strokeWidth="5" fill="none" strokeDasharray="60,100" strokeDashoffset="-40" />
                        </svg>
                        <div className="text-center">
                            <p className="text-2xl font-bold text-gray-900">7.400 $</p>
                            <p className="text-sm text-gray-500">Total Cashflow</p>
                        </div>
                    </div>
                </div>

                {/* Income & Expense */}
                <div className="mt-6 flex justify-between items-center">
                    <div className="flex items-center bg-blue-100 text-blue-700 px-4 py-2 rounded-lg">
                        <span className="text-xl">↓</span>
                        <div className="ml-2">
                            <p className="text-lg font-bold">4.441,20 $</p>
                            <p className="text-sm">Income</p>
                        </div>
                    </div>
                    <div className="flex items-center bg-orange-100 text-orange-700 px-4 py-2 rounded-lg">
                        <span className="text-xl">↑</span>
                        <div className="ml-2">
                            <p className="text-lg font-bold">2.959,80 $</p>
                            <p className="text-sm">Expense</p>
                        </div>
                    </div>
                </div>
            </div>

            {/* Latest Transactions Section */}
            <div className="bg-white p-6 rounded-2xl shadow-md w-full">
                <h2 className="text-lg font-semibold text-gray-700">Các giao dịch gần nhất</h2>
                <div className="mt-4">
                    {/* {[
                        { date: "Today", category: "Fashion", type: "Card payment", item: "Clothes", amount: "- 15,00 $", color: "text-red-500" },
                        { date: "20/05", category: "Food", type: "Card payment", item: "Food", amount: "- 8,00 $", color: "text-red-500" },
                        { date: "18/05", category: "Invest Money", type: "Transfer", item: "Business", amount: "+ 249,00 $", color: "text-blue-500" },
                        { date: "18/05", category: "My Company", type: "Transfer", item: "Salary", amount: "+ 1300,00 $", color: "text-blue-500" },
                        { date: "08/05", category: "Medical Checkup", type: "Card payment", item: "Pharmacy", amount: "- 39,50 $", color: "text-red-500" },
                        { date: "08/05", category: "Food", type: "Card payment", item: "Food", amount: "- 6,00 $", color: "text-red-500" },
                        { date: "04/05", category: "Financial", type: "Transfer", item: "Trading", amount: "+ 150,00 $", color: "text-blue-500" },
                    ].map((transaction, index) => (
                        <div key={index} className="flex justify-between py-2 border-b border-gray-200 last:border-none">
                            <span className="text-gray-600">{transaction.date}</span>
                            <span className="text-gray-700 font-medium">{transaction.category}</span>
                            <span className="text-gray-500 text-sm">{transaction.type}</span>
                            <span className={transaction.color}>{transaction.amount}</span>
                        </div>
                    ))} */}
                    {loading ? (
                        <div className="flex justify-center">
                            <Spin size="large" />
                        </div>
                    ) : (
                        <>
                            <Table
                                bordered={true}
                                columns={columns}
                                dataSource={transactions}
                                pagination={false}
                                rowKey="transactionId"
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
            </div>
        </div>
    );
};

export default DepositHistory;
