import React from "react";

const DepositHistory = () => {
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
                <h2 className="text-lg font-semibold text-gray-700">Latest Transactions</h2>
                <div className="mt-4">
                    {[
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
                    ))}
                </div>
                <div className="mt-4 text-center">
                    <button className="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600">See more →</button>
                </div>
            </div>
        </div>
    );
};

export default DepositHistory;
