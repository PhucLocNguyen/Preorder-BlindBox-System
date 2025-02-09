import React, { useState } from "react";
import { DownOutlined, UserOutlined } from '@ant-design/icons';
import { Menu, Dropdown } from 'antd';
import { MoreOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons'
import { Link } from "react-router";

const NavBarStaff = () => {
    const orders = [
        { id: 1, orderId: 675902, date: '17 Jan, 2024', items: 10, price: '$376.00', paid: 'Yes', address: 'Beaverton, OR 97005', status: 'Complete' },
        { id: 2, orderId: 675909, date: '1 Feb, 2024', items: 22, price: '$210.00', paid: 'No', address: 'Savannah, GA 31404', status: 'Pending' },
        { id: 3, orderId: 675912, date: '2 Feb, 2024', items: 12, price: '$320.00', paid: 'No', address: 'Tucson, AZ 85756', status: 'Cancelled' },
        { id: 4, orderId: 675588, date: '17 Mar, 2024', items: 10, price: '$510.00', paid: 'Yes', address: '1201 Geneva', status: 'Complete' },
        { id: 5, orderId: 675978, date: '23 Mar, 2024', items: 22, price: '$120.00', paid: 'Yes', address: 'Morrisville, NC 27560', status: 'Complete' },
        { id: 6, orderId: 675979, date: '12 Apr, 2024', items: 13, price: '$420.00', paid: 'Yes', address: 'Beachside, NC 444', status: 'Pending' },
        { id: 7, orderId: 675925, date: '22 Apr, 2024', items: 12, price: '$120.00', paid: 'No', address: 'Orchard Park, CO 333', status: 'Pending' },
        { id: 8, orderId: 675125, date: '21 May, 2024', items: 15, price: '$140.00', paid: 'Yes', address: 'Maplewood, NJ 678', status: 'Complete' },
        { id: 9, orderId: 675879, date: '22 Jun, 2024', items: 16, price: '$152.00', paid: 'Yes', address: 'Mountainview, WA 598', status: 'Complete' },
        { id: 10, orderId: 675369, date: '24 Jul, 2024', items: 17, price: '$421.00', paid: 'Yes', address: 'Anytown, USA 201', status: 'Cancelled' },
    ];

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
                                <th className="px-2 py-1">Order ID</th>
                                <th className="px-2 py-1">Date</th>
                                <th className="px-2 py-1">Items</th>
                                <th className="px-2 py-1">Price</th>
                                <th className="px-2 py-1">Paid</th>
                                <th className="px-2 py-1">Address</th>
                                <th className="px-2 py-1">Status</th>
                                <th className="px-2 py-1">Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            {orders
                                .filter(order => order.orderId.toString().includes(search))
                                .map((order) => (
                                    <tr key={order.id} className="border-t">
                                        <td className="px-2 py-2">{order.id}</td>
                                        <td className="px-2 py-2">{order.orderId}</td>
                                        <td className="px-2 py-2">{order.date}</td>
                                        <td className="px-2 py-2">{order.items}</td>
                                        <td className="px-2 py-2">{order.price}</td>
                                        <td
                                            className={`px-2 py-2 ${order.paid === 'Yes' ? 'text-green-500' : 'text-red-500'
                                                }`}
                                        >
                                            {order.paid}
                                        </td>
                                        <td className="px-2 py-2">{order.address}</td>
                                        <td
                                            className={`px-2 py-2 ${order.status === 'Complete' ? 'text-green-500' : 'text-red-500'
                                                }`}
                                        >
                                            {order.status}
                                        </td>
                                        <td className="px-2 py-2 text-center align-middle">
                                            <Dropdown
                                                menu={{
                                                    items: [
                                                        {
                                                            key: "0",
                                                            label: (
                                                                <Link
                                                                    to={`${order.id}`}
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
                    <div className="text-gray-500">Showing 1 to 10 of 256 entries</div>
                    <div className="flex items-center space-x-2">
                        <button className="px-3 py-1 border rounded text-gray-500">Back</button>
                        <button className="px-3 py-1 border rounded bg-blue-500 text-white">1</button>
                        <button className="px-3 py-1 border rounded text-gray-500">2</button>
                        <button className="px-3 py-1 border rounded text-gray-500">3</button>
                        <button className="px-3 py-1 border rounded text-gray-500">4</button>
                        <button className="px-3 py-1 border rounded text-gray-500">Next</button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default NavBarStaff;
