import React, { useState } from "react";
import { Table, Tag, Space, Input, Button, Checkbox } from "antd";
import { EyeOutlined, EditOutlined, DeleteOutlined, SearchOutlined, PlusOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";

const { Search } = Input;

const CampaignManagerment = () => {
    const navigate = useNavigate();
    const [search, setSearch] = useState("");

    const handleAddProduct = () => {
        navigate("/ProductCreate");
    };

    const handleEditProduct = (record) => {
        navigate("/ProductEdit", { state: { product: record } });
    };
    const onChange = (e) => {
        console.log(`checked = ${e.target.checked}`);
    };

    const columns = [
        {
            title: "Product",
            dataIndex: "product",
            key: "product",
            render: (text, record) => (
                <Space size="middle">
                    <Checkbox onChange={onChange}>
                        <img src={record.image} alt={text} style={{ width: 40, height: 40, borderRadius: 4 }} />
                    </Checkbox>
                    {text}
                </Space>
            ),
        },
        {
            title: "Category",
            dataIndex: "category",
            key: "category",
        },
        {
            title: "Price",
            dataIndex: "price",
            key: "price",
            render: (price) => `$${price.toLocaleString()}`,
        },
        {
            title: "Quantity",
            dataIndex: "quantity",
            key: "quantity",
        },
        {
            title: "Status",
            key: "status",
            dataIndex: "status",
            render: (status) => (
                <Tag color={status === "IN STOCK" ? "green" : "red"}>{status.toUpperCase()}</Tag>
            ),
        },
        {
            title: "Action",
            key: "action",
            render: (_, record) => (
                <Space size="middle">
                    <EyeOutlined style={{ color: "blue", fontSize: "18px" }} />
                    <EditOutlined style={{ color: "orange", fontSize: "18px" }} onClick={() => handleEditProduct(record)} />
                    <DeleteOutlined style={{ color: "red", fontSize: "18px" }} />
                </Space>
            ),
        },
    ];

    const data = [
        {
            key: "1",
            product: "BKLGO Full Zip Hoodie",
            category: "Clothing",
            price: 1321,
            quantity: 0,
            status: "OUT OF STOCK",
            image: "/images/hoodie.png",
        },
        {
            key: "2",
            product: "MacBook Pro",
            category: "Electronics",
            price: 1869,
            quantity: 0,
            status: "OUT OF STOCK",
            image: "/images/macbook.png",
        },
        {
            key: "3",
            product: "Metro Bar Stool",
            category: "Furniture",
            price: 99,
            quantity: 978,
            status: "IN STOCK",
            image: "/images/stool.png",
        },
        {
            key: "4",
            product: "Alchimia Chair",
            category: "Furniture",
            price: 2999,
            quantity: 0,
            status: "OUT OF STOCK",
            image: "/images/chair.png",
        },
    ];

    return (
        <div className="p-5 bg-white rounded-lg">
            <div className="flex justify-between items-center mb-4">
                <Search
                    placeholder="Search products..."
                    allowClear
                    enterButton={<SearchOutlined />}
                    style={{ width: 300 }}
                    onChange={(e) => setSearch(e.target.value)}
                />
                <Button type="primary" icon={<PlusOutlined />} onClick={handleAddProduct}>
                    New Campaign
                </Button>
            </div>
            <div className='text-center'>
                <Table
                    columns={columns}
                    dataSource={data.filter((item) => item.product.toLowerCase().includes(search.toLowerCase()))}
                    pagination={{ pageSize: 10 }}
                />
            </div>
        </div>
    );
};

export default CampaignManagerment;
