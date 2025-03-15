import React, { useState } from "react";
import Chart from "react-apexcharts";
import { SearchOutlined } from "@ant-design/icons";
import { Input } from 'antd';
import { DatePicker } from "antd";
import dayjs from "dayjs";
import { FaMedal } from "react-icons/fa";
const { RangePicker } = DatePicker;
const Dashboard = () => {
    const { Search } = Input;

    // Bar Chart (Monthly Sales)
    const barChartOptions = {
        chart: { id: "monthly-sales" },
        xaxis: {
            categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
        },
    };

    const barChartSeries = [
        { name: "Sales", data: [100, 300, 150, 250, 170, 200, 260, 120, 180, 320, 220, 90] },
    ];

    // Line Chart (Statistics)
    const [dateRange, setDateRange] = useState([null, null]);

    // Generate sample data for last 5 years to next 5 years
    const startDate = dayjs().subtract(5, "year");
    const endDate = dayjs().add(5, "year");
    const allDays = [];
    let currentDate = startDate;

    while (currentDate.isBefore(endDate)) {
        allDays.push({
            date: currentDate.format("YYYY-MM-DD"),
            revenue: Math.floor(Math.random() * 1000) + 100,
        });
        currentDate = currentDate.add(1, "day");
    }

    // Filter data based on selected date range
    const filteredData = dateRange[0] && dateRange[1]
        ? allDays.filter(({ date }) =>
            dayjs(date).isAfter(dateRange[0]) &&
            dayjs(date).isBefore(dateRange[1].add(1, "day"))
        )
        : allDays.slice(-30); // Default to last 30 days if no range selected

    // Line Chart Configuration
    const lineChartOptions = {
        chart: { id: "revenue-statistics" },
        xaxis: { categories: filteredData.map(d => d.date) },
    };

    const lineChartSeries = [
        { name: "Revenue", data: filteredData.map(d => d.revenue) },
    ];
    // Sample data for top 3 campaigns

    const topCampaigns = [
        { name: "Disney Mickey", orders: 430, color: "linear-gradient(135deg, #C0C0C0, #A9A9A9)", rank: 2 }, // Silver
        { name: "Hộp mù Natra 2 Ma đồng náo hải", orders: 520, color: "linear-gradient(135deg, #FFD700, #FFC000)", rank: 1 }, // Gold
        { name: "Toroto", orders: 390, color: "linear-gradient(135deg, #CD7F32, #B87333)", rank: 3 }, // Bronze
    ];

    return (
        <div className="dashboard-container overflow-auto h-screen pr-4 scrollbar-thin scrollbar-thumb-gray-400 scrollbar-track-gray-200 scrollbar-thumb-opacity-80">
            {/* Header */}
            <div className="flex justify-between items-center mb-6">
                <h2 className="text-2xl font-bold">Dashboard</h2>
            </div>

            {/* Main Content */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                {/* Left Side */}
                <div className="space-y-6">
                    <div className="grid grid-cols-2 gap-4">
                        <div className="p-4 bg-white rounded-lg shadow">
                            <p className="text-gray-600">Wallet Balance</p>
                            <h3 className="text-2xl font-bold">3,782,000 VNĐ</h3>
                            <span className="text-green-500">↑ 11.01%</span>
                        </div>

                        <div className="p-4 bg-white rounded-lg shadow">
                            <p className="text-gray-600">Orders</p>
                            <h3 className="text-2xl font-bold">5,359</h3>
                            <span className="text-red-500">↓ 9.05%</span>
                        </div>
                    </div>

                    {/* Monthly Sales Chart */}
                    <div className="p-4 bg-white rounded-lg shadow">
                        <h3 className="text-lg font-bold">Monthly Sales</h3>
                        <Chart options={barChartOptions} series={barChartSeries} type="bar" height={300} />
                    </div>
                </div>

                {/* Right Side - Monthly Target with Radial Chart */}
                <div className="p-6 bg-white rounded-lg shadow text-center">
                    <h2 className="text-4xl font-bold mb-4">Top 3 Campaigns</h2>
                    <p className="text-gray-600 mb-4">Campaigns with the highest number of orders</p>

                    <div className="grid grid-cols-3 gap-10 justify-center items-end">
                        {topCampaigns.map((campaign) => (
                            <div key={campaign.name} className="relative flex flex-col items-center">
                                <FaMedal
                                    className={`text-4xl mb-3 ${campaign.rank === 1 ? 'text-yellow-500' : campaign.rank === 2 ? 'text-gray-400' : 'text-orange-600'}`}
                                />
                                <p className="mt-2 text-lg font-bold text-center">{campaign.name}</p>
                                <p className="text-gray-600 text-lg mb-2">{campaign.orders} orders</p>
                                <div
                                    className="w-28 flex items-center justify-center text-white font-bold text-2xl rounded-lg shadow-lg"
                                    style={{ height: `${campaign.orders / 4}px`, background: campaign.color }}
                                >
                                    {campaign.rank}
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>

            {/* Revenue Statistics */}
            <div className="p-4 bg-white rounded-lg shadow mt-6">
                <h3 className="text-lg font-bold">Revenue Statistics</h3>
                <p className="text-gray-600">View revenue within a specific date range</p>

                {/* Date Range Picker */}
                <div className="my-4">
                    <RangePicker
                        onChange={(dates) => setDateRange(dates)}
                        format="YYYY-MM-DD"
                    />
                </div>

                {/* Revenue Line Chart */}
                <Chart options={lineChartOptions} series={lineChartSeries} type="line" height={300} />
            </div>
        </div>
    );
};

export default Dashboard;
