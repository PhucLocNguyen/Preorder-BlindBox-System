import React, { useCallback, useEffect, useState } from "react";
import { Dropdown, Pagination } from 'antd';
import { MoreOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons'
import { Link } from "react-router";
import { GetAllOrder } from "../../../api/Order/ApiOrder";
const OrdersView = () => {
    const [orders, setOrders] = useState([]);
    const [search, setSearch] = useState("");
    const [orderBy, setOrderBy] = useState("increase");
    const [pageIndex, setPageIndex] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalPage, setTotalPage] = useState(0);
    const handlePageChange = (page) => {
        setPageIndex(page);
    };

    const handleTotalPage = useCallback(async () => {
        try {
            const result = await GetAllOrder();
            setTotalPage(result.length);
        } catch (error) {

        }

    }, [])
    const handleNextPage = () => {
        setPageIndex(() => pageIndex + 1);
    };
    const handlePrevPage = () => {
        setPageIndex(() => Math.max(1, pageIndex - 1));  // Đảm bảo pageIndex không nhỏ hơn 1
    };
    const fetchOrders = useCallback(async () => {
        try {
            const result = await GetAllOrder(pageIndex, pageSize, search, orderBy);
            setOrders([...result]);
        } catch (error) {
            console.error("Fetch Orders Error:", error);
            setOrders([]);
        }
    }, [pageSize, pageIndex, search, orderBy]);

    useEffect(() => {
        fetchOrders();
        handleTotalPage();
    }, [fetchOrders])
    const itemRender = (_, type, originalElement) => {
        if (type === 'prev') {
            return <a onClick={handlePrevPage}>Previous</a>;
        }
        if (type === 'next') {
            return <a onClick={handleNextPage}>Next</a>;
        }
        return originalElement;
    };


    const handleSearchKeyWord = (e) => {
        console.log(e.target.value);
        setSearch(e.target.value);
    }

    return (
        <div className="flex-1 h-full p-1 flex flex-col">
            <div className="flex justify-between items-center mb-2">
                <h1 className="text-2xl font-bold">Order Management</h1>
            </div>

            <div className="bg-white p-4 rounded shadow flex-1 flex flex-col">
                <div className="flex justify-between items-center mb-1">
                    <div className="relative">
                        <input
                            type="text"
                            className="bg-gray-100 border border-gray-300 rounded py-2 px-4 w-64"
                            placeholder="Search anything"
                            value={search}
                            onChange={(e) => handleSearchKeyWord(e)}
                        />
                        <i className="fas fa-search absolute top-3 right-3 text-gray-500"></i>
                    </div>
                    <div className="flex items-center space-x-4">
                        <button className="flex items-center text-gray-500" onClick={() => {
                            orderBy === "increase" ? setOrderBy("decrease") : setOrderBy("increase");
                        }}
                        >
                            <i className="fas fa-filter mr-2"></i> Sort
                        </button>
                    </div>
                </div>
                <div className="flex-1 overflow-x-auto">
                    <table className="table-auto w-full border-collapse text-left">
                        <thead>
                            <tr className="text-gray-500">
                                <th className="px-2 py-1">#</th>
                                <th className="px-2 py-1">Receiver</th>
                                <th className="px-2 py-1">Address</th>
                                <th className="px-2 py-1">Phone</th>
                                <th className="px-2 py-1">Items</th>
                                <th className="px-2 py-1">Price</th>
                                <th className="px-2 py-1">Date</th>
                                <th className="px-2 py-1">Status</th>
                                <th className="px-2 py-1">Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            {orders
                                // .filter(order => order.orderId.toString().includes(search))
                                .map((data, index) => (
                                    <tr key={index} className="border-t">
                                        <td className="px-2 py-2">{index + 1}</td>
                                        <td className="px-2 py-2">{data.receiver}</td>
                                        <td className="px-2 py-2">{data.receiverAddress}</td>
                                        <td className="px-2 py-2">{data.receiverPhone}</td>
                                        <td className="px-2 py-2">{
                                            data.totalItems
                                        }</td>
                                        <td className="px-2 py-2">{data.amount} VND</td>
                                        <td className="px-2 py-2">{data.createdDate}</td>

                                        <td
                                            className={`px-2 py-2 ${data.status === 'Completed' ? 'text-green-500' : 'text-red-500'
                                                }`}
                                        >
                                            {data.status}
                                        </td>
                                        <td className="px-2 py-2 text-center align-middle">
                                            <Dropdown
                                                menu={{
                                                    items: [
                                                        {
                                                            key: "0",
                                                            label: (
                                                                <Link
                                                                    to={`${data.orderId}`}
                                                                    className="px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                                                                >
                                                                    <EditOutlined /> <span>Edit order</span>
                                                                </Link>
                                                            ),
                                                        },
                                                        {
                                                            key: "2",
                                                            label: (
                                                                <div className="px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">
                                                                    <DeleteOutlined /> <span>Delete order</span>
                                                                </div>
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
                                        </td>
                                    </tr>
                                ))}

                        </tbody>
                    </table>
                </div>


                <div className="flex justify-between items-center mt-4 p-4 bg-white shadow rounded">
                    <Pagination
                        current={pageIndex}
                        total={totalPage}
                        pageSize={pageSize}
                        onChange={handlePageChange
                        }
                        className="shadow-md p-2 rounded-md"
                    />
                </div>
            </div>
        </div>
    );
};

export default OrdersView;