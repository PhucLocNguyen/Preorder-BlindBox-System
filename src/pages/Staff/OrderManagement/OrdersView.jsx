import React, { useState } from "react";
import { Dropdown, Pagination } from 'antd';
import { MoreOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons'
import { Link } from "react-router";
import { GetAllOrder } from "../../../api/Order/ApiOrder";
import useFetchData from "../../../hooks/useFetchData";
const NavBarStaff = () => {
    const [pageSize, setPageSize] = useState(10);
    const [pageIndex, setPageIndex] = useState(1);

    const { data: orders, loading, refetch } = useFetchData(GetAllOrder);
    const handleNextPage = () => {
        setPageIndex(prev => prev + 1);
    };
    const handleChangePageSize = (size) => {
        setPageSize(size);
        setPageIndex(1); // Reset về trang đầu khi đổi kích thước trang
    };
    const itemRender = (_, type, originalElement) => {
        if (type === 'prev') {
            return <a>Previous</a>;
        }
        if (type === 'next') {
            return <a>Next</a>;
        }
        return originalElement;
    };

    const [search, setSearch] = useState("");

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
                            onChange={(e) => setSearch(e.target.value)}
                        />
                        <i className="fas fa-search absolute top-3 right-3 text-gray-500"></i>
                    </div>
                    <div className="flex items-center space-x-4">
                        <button className="flex items-center text-gray-500">
                            <i className="fas fa-filter mr-2"></i> Filters
                        </button>
                        <button className="flex items-center text-gray-500">
                            <i className="fas fa-sort mr-2"></i> Sort
                        </button>
                        <button className="flex items-center text-gray-500">
                            <i className="fas fa-ellipsis-h"></i>
                        </button>
                    </div>
                </div>
                <div className="flex-1 overflow-x-auto">
                    <table className="table-auto w-full border-collapse text-left">
                        <thead>
                            <tr className="text-gray-500">
                                <th className="px-2 py-1">#</th>
                                <th className="px-2 py-1">Date</th>
                                <th className="px-2 py-1">Items</th>
                                <th className="px-2 py-1">Price</th>
                                <th className="px-2 py-1">Receiver</th>
                                <th className="px-2 py-1">Address</th>
                                <th className="px-2 py-1">Status</th>
                                <th className="px-2 py-1">Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            {loading ? (<tr>
                                <th>loading</th>
                                <th></th>
                            </tr>) : orders
                                // .filter(order => order.orderId.toString().includes(search))
                                .map((orders, index) => (
                                    <tr key={index} className="border-t">
                                        <td className="px-2 py-2">{index + 1}</td>
                                        <td className="px-2 py-2">{orders.createdDate}</td>
                                        <td className="px-2 py-2">{orders.totalItems}</td>
                                        <td className="px-2 py-2">{orders.amount} VND</td>
                                        <td className="px-2 py-2">{orders.receiver}</td>
                                        <td className="px-2 py-2">{orders.receiverAddress}</td>
                                        <td
                                            className={`px-2 py-2 ${orders.status === 'Completed' ? 'text-green-500' : 'text-red-500'
                                                }`}
                                        >
                                            {orders.status}
                                        </td>
                                        <td className="px-2 py-2 text-center align-middle">
                                            <Dropdown
                                                menu={{
                                                    items: [
                                                        {
                                                            key: "0",
                                                            label: (
                                                                <Link
                                                                    to={`${orders.orderId}`}
                                                                    className="px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                                                                >
                                                                    <EditOutlined /> <span>Edit order</span>
                                                                </Link>
                                                            ),
                                                        },
                                                        {
                                                            key: "1",
                                                            label: (
                                                                <div className="px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">
                                                                    Menu Item Two
                                                                </div>
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
                    {/* <div className="text-gray-500">Showing 1 to 10 of 256 entries</div>
                    <div className="flex items-center space-x-2">
                        <button className="px-3 py-1 border rounded text-gray-500">Back</button>
                        <button className="px-3 py-1 border rounded bg-blue-500 text-white">1</button>
                        <button className="px-3 py-1 border rounded text-gray-500">2</button>
                        <button className="px-3 py-1 border rounded text-gray-500">3</button>
                        <button className="px-3 py-1 border rounded text-gray-500">4</button>
                        <button className="px-3 py-1 border rounded text-gray-500">Next</button>
                    </div> */}
                    <Pagination total={60} itemRender={itemRender} />;
                </div>
            </div>
        </div>
    );
};

export default NavBarStaff;
