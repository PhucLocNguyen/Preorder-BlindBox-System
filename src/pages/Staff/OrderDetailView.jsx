import React, { useState } from 'react'
import { useNavigate, useParams } from 'react-router'
import {
    ArrowLeftOutlined,
    ShoppingOutlined,
    MoneyCollectOutlined,
    TruckOutlined,
    InboxOutlined,
    ProjectOutlined,
    UserOutlined,
    HomeOutlined
} from '@ant-design/icons';

const OrderDetailView = () => {
    const navigate = useNavigate();
    const { id } = useParams();
    const initialProgress = () => {
        const progressArray = [
            { stateProcessing: 'Confirming', state: 0 },
            { stateProcessing: 'Payment', state: 0 },
            { stateProcessing: 'Processing', state: 0 },
            { stateProcessing: 'Shipping', state: 0 },
            { stateProcessing: 'Delivered', state: 0 },
        ];

        const step = 4;
        for (let index = 0; index < step && index < progressArray.length; index++) {
            if (index === (step - 1))
                progressArray[index] = { ...progressArray[index], state: 1 };
            else progressArray[index] = { ...progressArray[index], state: 2 };
        }
        return progressArray;
    };

    const [progress, setProgress] = useState(initialProgress);
    const getWidthFromState = (state) => {
        if (state === 0) return '0%';
        if (state === 1) return '50%';
        if (state === 2) return '100%';
        return '0%';
    };

    const getBgClassFromState = (state) => {
        if (state === 0) return 'bg-white';
        if (state === 1) return 'bg-orange-400';
        if (state === 2) return 'bg-blue-500';
        return 'bg-white';
    };

    const products = [
        {
            id: 1,
            image: "https://placehold.co/50x50",
            name: "iPhone 15 Pro",
            category: "Electronics - Small",
            status: "Ready",
            statusClass: "bg-green-100 text-green-600",
            quantity: 3,
            price: "$20.00",
            tax: "$3.00",
            amount: "$57.00",
        },
        {
            id: 2,
            image: "https://placehold.co/50x50",
            name: "ASUS ZenBook",
            category: "Electronics - Large",
            status: "Packaging",
            statusClass: "bg-yellow-100 text-yellow-600",
            quantity: 1,
            price: "$2,499.99",
            tax: "$187.50",
            amount: "$2,687.49",
        },
        {
            id: 3,
            image: "https://placehold.co/50x50",
            name: "Modern Toaster",
            category: "Kitchen - Small",
            status: "Packaging",
            statusClass: "bg-yellow-100 text-yellow-600",
            quantity: 2,
            price: "$129.99",
            tax: "$9.74",
            amount: "$269.72",
        },
        {
            id: 4,
            image: "https://placehold.co/50x50",
            name: "Kindle Paperwhite",
            category: "Electronics - Large",
            status: "Ready",
            statusClass: "bg-green-100 text-green-600",
            quantity: 2,
            price: "$139.99",
            tax: "$21.00",
            amount: "$300.98",
        },
    ];


    return (
        <>
            <div className='h-screen overflow-y-auto'>
                <div className="sticky top-0 z-10 bg-white shadow-md rounded-lg p-2 flex justify-between items-center m-2 w-full max-w-[1176px] mx-auto">
                    <div>
                        <h1 className="text-2xl font-bold"><button className="bg-white p-4 rounded-lg shadow mb-4 border-2 border-black w-14" onClick={() => navigate(-1)}><ArrowLeftOutlined /></button>  #S-10242002</h1>
                        <p className="text-gray-500">Order History / Order Details / S-10242002 - April, 05 2024 at 9:48 pm</p>
                    </div>
                    <div className="flex space-x-2">
                        <button className="bg-red-100 text-red-600 px-4 py-2 rounded-lg border border-red-600">Delete Order</button>
                        <button className="bg-gray-100 text-gray-600 px-4 py-2 rounded-lg border border-gray-600">Track Order</button>
                        <button className="bg-gray-100 text-gray-600 px-4 py-2 rounded-lg border border-gray-600">Edit Order</button>
                    </div>
                </div>
                <div className="mx-[5px]">

                    <div className='flex justify-around'>
                        <div className='w-[70%]'>
                            <div className="bg-white p-4 rounded-lg shadow mb-4">
                                <h2 className="text-lg font-bold mb-2">Progress</h2>
                                <div className="flex justify-between items-stretch ">
                                    <div className="flex-1 text-left rounded-lg shadow p-2 m-1">
                                        <i className="fas fa-check-circle text-gray-500 text-2xl"></i>
                                        <span><ShoppingOutlined /></span>
                                        <p className="text-sm">Order Confirming</p>
                                        <div className="w-full max-w-xl">
                                            <div className="relative pt-1">
                                                <div className="overflow-hidden h-2 text-xs flex rounded bg-gray-200">
                                                    <div style={{ width: getWidthFromState(progress[0].state) }} className={`shadow-none flex flex-col text-center whitespace-nowrap text-white justify-center ${getBgClassFromState(progress[0].state)}`}></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="flex-1 text-left rounded-lg shadow p-2 m-1">
                                        <i className="fas fa-check-circle text-green-500 text-2xl"></i>
                                        <span><MoneyCollectOutlined /></span>
                                        <p className="text-sm">Payment Pending</p>
                                        <div className="w-full max-w-xl">
                                            <div className="relative pt-1">
                                                <div className="overflow-hidden h-2 text-xs flex rounded bg-gray-200">
                                                    <div style={{ width: getWidthFromState(progress[1].state) }} className={`shadow-none flex flex-col text-center whitespace-nowrap text-white justify-center ${getBgClassFromState(progress[1].state)}`}></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="flex-1 text-left rounded-lg shadow p-2 m-1">
                                        <i className="fas fa-check-circle text-yellow-500 text-2xl"></i>
                                        <span><ProjectOutlined /></span>
                                        <p className="text-sm">Processing</p>
                                        <div className="w-full max-w-xl">
                                            <div className="relative pt-1">
                                                <div className="overflow-hidden h-2 text-xs flex rounded bg-gray-200">
                                                    <div style={{ width: getWidthFromState(progress[2].state) }} className={`shadow-none flex flex-col text-center whitespace-nowrap text-white justify-center ${getBgClassFromState(progress[2].state)}`}></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="flex-1 text-left rounded-lg shadow p-2 m-1">
                                        <i className="fas fa-truck text-gray-500 text-2xl"></i>
                                        <span><InboxOutlined /></span>
                                        <p className="text-sm">Shipping</p>
                                        <div className="w-full max-w-xl">
                                            <div className="relative pt-1">
                                                <div className="overflow-hidden h-2 text-xs flex rounded bg-gray-200">
                                                    <div style={{ width: getWidthFromState(progress[3].state) }} className={`shadow-none flex flex-col text-center whitespace-nowrap text-white justify-center ${getBgClassFromState(progress[3].state)}`}></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="flex-1 text-left rounded-lg shadow p-2 m-1">
                                        <i className="fas fa-box text-gray-500 text-2xl"></i>
                                        <span><TruckOutlined /></span>
                                        <p className="text-sm">Delivered</p>
                                        <div className="w-full max-w-xl">
                                            <div className="relative pt-1">
                                                <div className="overflow-hidden h-2 text-xs flex rounded bg-gray-200">
                                                    <div style={{ width: getWidthFromState(progress[4].state) }} className={`shadow-none flex flex-col text-center whitespace-nowrap text-white justify-center ${getBgClassFromState(progress[4].state)}`}></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="bg-white p-4 rounded-lg shadow mb-4">
                                <div className="flex justify-between items-center mb-4">
                                    <h2 className="text-lg font-bold">Product</h2>
                                    <button className="bg-gray-100 text-gray-600 px-4 py-2 rounded-lg border border-gray-600">Download CSV</button>
                                </div>
                                <table className="min-w-full bg-white rounded-lg overflow-hidden border border-gray-200 shadow-md">
                                    <thead>
                                        <tr className="bg-gray-100">
                                            <th className="py-3 px-4 border border-gray-200 text-center">Item</th>
                                            <th className="py-3 px-4 border border-gray-200 text-center">Status</th>
                                            <th className="py-3 px-4 border border-gray-200 text-center">Quantity</th>
                                            <th className="py-3 px-4 border border-gray-200 text-center">Price</th>
                                            <th className="py-3 px-4 border border-gray-200 text-center">Tax</th>
                                            <th className="py-3 px-4 border border-gray-200 text-center">Amount</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {products.map((product) => (
                                            <tr key={product.id} className="border-t">
                                                <td className="py-3 px-4 flex items-center">
                                                    <img src={product.image} alt={product.name} className="w-10 h-10 mr-2 rounded-md" />
                                                    <div>
                                                        <p className="font-medium">{product.name}</p>
                                                        <p className="text-gray-500 text-sm">{product.category}</p>
                                                    </div>
                                                </td>
                                                <td className="py-3 px-4 text-center">
                                                    <span className={`px-3 py-1 rounded-lg ${product.statusClass}`}>{product.status}</span>
                                                </td>
                                                <td className="py-3 px-4 text-center">{product.quantity}</td>
                                                <td className="py-3 px-4 text-center">{product.price}</td>
                                                <td className="py-3 px-4 text-center">{product.tax}</td>
                                                <td className="py-3 px-4 text-center">{product.amount}</td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>
                            <div className="bg-white p-4 rounded-lg shadow mb-4">
                                <h2 className="text-lg font-bold mb-2">Timeline</h2>
                                <div className="space-y-2">
                                    <div className="flex items-center">
                                        <i className="fas fa-box text-gray-500 text-2xl mr-2"></i>
                                        <div>
                                            <p>The Packaging has been started</p>
                                            <p className="text-gray-500 text-sm">Confirmed by Jane Doe</p>
                                        </div>
                                        <p className="ml-auto text-gray-500 text-sm">April 01, 2024, 09:40 pm</p>
                                    </div>
                                    <div className="flex items-center">
                                        <i className="fas fa-envelope text-gray-500 text-2xl mr-2"></i>
                                        <div>
                                            <p>The Invoice has been sent to the Customer</p>
                                            <p className="text-gray-500 text-sm">Invoice email was sent to delivery@dudud.com</p>
                                        </div>
                                        <p className="ml-auto text-gray-500 text-sm">April 01, 2024, 09:42 pm</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className='w-[25%]'>
                            <div className="grid grid-cols-1 gap-4">
                                <div className="bg-white p-4 rounded-lg shadow">
                                    <h2 className="text-lg font-bold mb-2">Payment</h2>
                                    <button className="bg-gray-100 text-gray-600 px-4 py-2 rounded-lg border border-gray-600 mb-4">Download Invoice</button>
                                    <div className="space-y-2">
                                        <div className="flex justify-between">
                                            <p>Subtotal</p>
                                            <p>$3,159.95</p>
                                        </div>
                                        <div className="flex justify-between">
                                            <p>Discount (10%)</p>
                                            <p>-$315.99</p>
                                        </div>
                                        <div className="flex justify-between">
                                            <p>Shipping Cost</p>
                                            <p>$200.00</p>
                                        </div>
                                        <div className="flex justify-between">
                                            <p>Tax (8%)</p>
                                            <p>$212.79</p>
                                        </div>
                                        <div className="flex justify-between font-bold">
                                            <p>Total</p>
                                            <p>$3,076.75</p>
                                        </div>
                                    </div>
                                </div>
                                <div className="bg-white p-3 rounded-lg shadow-md">
                                    <h2 className="text-lg font-bold mb-2">Customer</h2>
                                    <p className="text-gray-500 text-sm mb-4">Information Detail</p>

                                    <div className="space-y-4">
                                        {/* General Information */}
                                        <div className="bg-gray-100 p-4 rounded-lg">
                                            <h3 className="font-bold flex items-center">
                                                <UserOutlined className="mr-2 text-gray-500" /> General Information
                                            </h3>
                                            <p className="text-gray-700 mt-2">• Acme Corporation</p>
                                            <p className="text-gray-700">• acme@dudud.com</p>
                                            <p className="text-gray-700">• (728) 054 4928</p>
                                        </div>

                                        {/* Shipping Address */}
                                        <div className="bg-gray-100 p-4 rounded-lg">
                                            <h3 className="font-bold flex items-center">
                                                <HomeOutlined className="mr-2 text-gray-500" /> Shipping Address
                                            </h3>
                                            <p className="text-gray-700 mt-2">• 100 Main Street</p>
                                            <p className="text-gray-700">• Anytown, CA 12345</p>
                                            <p className="text-gray-700">• United States</p>
                                            <p className="text-gray-700">• (555) 555-5555</p>
                                        </div>

                                        {/* Billing Address */}
                                        <div className="bg-gray-100 p-4 rounded-lg">
                                            <h3 className="font-bold flex items-center">
                                                <HomeOutlined className="mr-2 text-gray-500" /> Billing Address
                                            </h3>
                                            <p className="text-gray-700 mt-2">• Same as shipping address</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </>

    )
}

export default OrderDetailView