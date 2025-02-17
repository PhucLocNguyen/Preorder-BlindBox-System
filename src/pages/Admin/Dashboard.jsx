import React from "react";
import Chart from "react-apexcharts";
import { SearchOutlined } from "@ant-design/icons";
import { Input } from 'antd';

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
    const lineChartOptions = {
        chart: { id: "statistics" },
        xaxis: {
            categories: ["May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
        },
    };

    const lineChartSeries = [
        { name: "Target", data: [180, 170, 190, 210, 230, 220, 240, 235] },
        { name: "Actual", data: [50, 40, 80, 100, 120, 130, 150, 140] },
    ];

    // ** Radial Chart (Monthly Target) **
    const radialChartOptions = {
        chart: {
            type: "radialBar",
        },
        plotOptions: {
            radialBar: {
                hollow: {
                    size: "60%",  // Adjust inner circle size
                },
                track: {
                    background: "#E6E6E6", // Light gray track for missing portion
                },
                dataLabels: {
                    name: { show: false },
                    value: {
                        fontSize: "24px",
                        fontWeight: "bold",
                        color: "#000",
                        offsetY: 5,
                    },
                },
            },
        },
        colors: ["#3b82f6"], // Blue color for progress
        labels: ["Progress"],
    };

    const radialChartSeries = [75.55]; // Progress percentage

    return (
        <div className="dashboard-container overflow-auto h-screen pr-4 scrollbar-thin scrollbar-thumb-gray-400 scrollbar-track-gray-200 scrollbar-thumb-opacity-80">
            {/* Header */}
            <div className="flex justify-between items-center mb-6">
                <h2 className="text-2xl font-bold">Dashboard</h2>
                <Search
                    allowClear
                    enterButton={<SearchOutlined />}
                    placeholder="Search or type command..."
                    className="p-2 border rounded-lg w-72"
                />
            </div>

            {/* Main Content */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                {/* Left Side */}
                <div className="space-y-6">
                    <div className="grid grid-cols-2 gap-4">
                        <div className="p-4 bg-white rounded-lg shadow">
                            <p className="text-gray-600">Customers</p>
                            <h3 className="text-2xl font-bold">3,782</h3>
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
                <div className="p-4 bg-white rounded-lg shadow">
                    <h3 className="text-lg font-bold">Monthly Target</h3>
                    <p className="text-gray-600">Target you've set for each month</p>
                    <div className="flex justify-center py-6">
                        <Chart options={radialChartOptions} series={radialChartSeries} type="radialBar" height={200} />
                    </div>
                    <p className="text-center text-gray-600 mt-2">You earned $3287 today, higher than last month.</p>

                    <div className="grid grid-cols-3 text-center text-gray-600 border-t pt-4">
                        <div>
                            <p className="text-lg font-bold text-red-500">$20K ↓</p>
                            <p>Target</p>
                        </div>
                        <div>
                            <p className="text-lg font-bold text-green-500">$20K ↑</p>
                            <p>Revenue</p>
                        </div>
                        <div>
                            <p className="text-lg font-bold text-green-500">$20K ↑</p>
                            <p>Today</p>
                        </div>
                    </div>
                </div>
            </div>

            {/* Statistics */}
            <div className="p-4 bg-white rounded-lg shadow mt-6">
                <h3 className="text-lg font-bold">Statistics</h3>
                <p className="text-gray-600">Target you've set for each month</p>
                <Chart options={lineChartOptions} series={lineChartSeries} type="line" height={300} />
            </div>
        </div>
    );
};

export default Dashboard;
