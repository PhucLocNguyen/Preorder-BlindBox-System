import React, { useState } from "react";
import { Table, Tag, Space, Button, DatePicker, Dropdown, Menu } from "antd";
import { DownOutlined, CalendarOutlined } from "@ant-design/icons";

const { RangePicker } = DatePicker;

const transactions = [
    {
        name: "William Mardoch",
        id: "25014287",
        date: "21 March 2021",
        time: "At 8:45 PM",
        invoice: "OP01214784",
        amount: "$250 USD",
        status: "Receive",
        statusColor: "green",
    },
    {
        name: "Jack Dawson",
        id: "22154879",
        date: "20 March 2021",
        time: "At 9:28 AM",
        invoice: "OP01214784",
        amount: "- $20 USD",
        status: "Transfer",
        statusColor: "red",
    },
    {
        name: "Mailchimp",
        id: "Subscription service",
        date: "19 March 2021",
        time: "At 7:21 PM",
        invoice: "OP87452148",
        amount: "- $80 USD",
        status: "Payment",
        statusColor: "orange",
    },
    {
        name: "Fiverr",
        id: "Marketplace",
        date: "18 March 2021",
        time: "At 8:45 PM",
        invoice: "OP32201425",
        amount: "$100 USD",
        status: "Receive",
        statusColor: "green",
    },
];

const filterOptions = ["All", "Received", "Transfer", "Payment", "Withdraw"];
const dateRanges = ["Past 7 Days", "Past 30 Days", "Past 90 Days"];

const HistoryTransactions = () => {
    const [selectedFilter, setSelectedFilter] = useState("All");
    const [selectedDate, setSelectedDate] = useState("Past 90 Days");

    return (
        <div className="p-6 bg-white rounded-lg shadow-lg">
            {/* Navbar */}
            <div className="flex justify-between items-center pb-4 border-b">
                <div className="flex space-x-6">
                    {filterOptions.map((filter) => (
                        <button
                            key={filter}
                            onClick={() => setSelectedFilter(filter)}
                            className={`text-sm font-medium ${selectedFilter === filter ? "text-orange-500" : "text-gray-500"
                                }`}
                        >
                            {filter}
                        </button>
                    ))}
                </div>
                <div className="flex space-x-4 items-center">
                    <Dropdown
                        overlay={
                            <Menu>
                                {dateRanges.map((range) => (
                                    <Menu.Item key={range} onClick={() => setSelectedDate(range)}>
                                        {range}
                                    </Menu.Item>
                                ))}
                            </Menu>
                        }
                        trigger={["click"]}
                    >
                        <Button icon={<CalendarOutlined />}>
                            {selectedDate} <DownOutlined />
                        </Button>
                    </Dropdown>
                    <RangePicker />
                </div>
            </div>

            {/* Transaction Table */}
            <div className="overflow-x-auto mt-4">
                <Table dataSource={transactions} pagination={{ pageSize: 5 }} rowKey="invoice">
                    <Table.Column title="Name/Business" dataIndex="name" key="name" render={(text, record) => (
                        <div>
                            <p className="font-semibold">{text}</p>
                            <p className="text-gray-500 text-sm">{record.id}</p>
                        </div>
                    )} />
                    <Table.Column title="Date" dataIndex="date" key="date" render={(text, record) => (
                        <div>
                            <p className="font-medium">{text}</p>
                            <p className="text-gray-500 text-sm">{record.time}</p>
                        </div>
                    )} />
                    <Table.Column title="Invoice ID" dataIndex="invoice" key="invoice" />
                    <Table.Column title="Amount" dataIndex="amount" key="amount" />
                    <Table.Column title="Status" dataIndex="status" key="status" render={(text, record) => (
                        <Tag color={record.statusColor}>{text}</Tag>
                    )} />
                    <Table.Column title="Action" key="action" render={() => (
                        <Space>
                            <Button type="primary">Details</Button>
                        </Space>
                    )} />
                </Table>
            </div>
        </div>
    );
};

export default HistoryTransactions;
