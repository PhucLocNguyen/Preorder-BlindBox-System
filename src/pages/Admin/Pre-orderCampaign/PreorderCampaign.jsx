import React, { useState, useEffect } from "react";
import { Table, Tag, Space, Input, Button, Modal, Checkbox, message } from "antd";
import { EyeOutlined, EditOutlined, DeleteOutlined, SearchOutlined, PlusOutlined } from "@ant-design/icons";
import Pre_orderCampaignEdit from "./Pre-orderCampaignEdit";
import Pre_orderCampaignCreate from "./Pre-orderCampaignCreate";
import Mofusandimg from "../../../assets/Admin/MofusandBlindBox.png";
import Kuromiimg from "../../../assets/Admin/KuromiBlindBox.png";
import CrayonShinimg from "../../../assets/Admin/CrayonShinBlindBox.png";
const { Search } = Input;

const Pre_orderCampaign = () => {
    const [selectedProduct, setSelectedProduct] = useState(null);
    const [search, setSearch] = useState("");
    const [isCreateModalVisible, setIsCreateModalVisible] = useState(false);
    const [isEditModalVisible, setIsEditModalVisible] = useState(false);
    const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
    const [productToDelete, setProductToDelete] = useState(null);
    const [selectedProductId, setSelectedProductId] = useState(new Set());
    const [warning, setWarning] = useState('');

    const handleAddProduct = () => {
        setWarning('');
        if (selectedProductId.size === 0) {
            setWarning('Please choose a product before adding a campaign');
        } else {
            const selectedKey = Array.from(selectedProductId)[0];
            setSelectedProduct(selectedKey);
            setIsCreateModalVisible(true);
        }
    };

    const handleEditProduct = (record) => {
        setSelectedProduct(record.key);
        setIsEditModalVisible(true);
    };

    const handleCancelCreate = () => {
        setIsCreateModalVisible(false);
        setSelectedProduct(null);
    };

    const handleCancelEdit = () => {
        setIsEditModalVisible(false);
    };

    const handleDeleteProduct = (record) => {
        setProductToDelete(record);
        setIsDeleteModalVisible(true);
    };

    const confirmDeleteProduct = () => {
        setData((prevData) => prevData.filter((product) => product.key !== productToDelete.key));
        setIsDeleteModalVisible(false);
        message.success(`Product "${productToDelete.product}" has been deleted.`);
        setProductToDelete(null);
    };

    const handleCheckbox = (key) => {
        setWarning('');
        setSelectedProductId(prev => {
            const updated = new Set(prev);
            if (updated.has(key)) {
                updated.delete(key);
            } else {
                updated.clear();
                updated.add(key);
            }

            return updated;
        });
    };


    const columns = [
        {
            title: "Product",
            dataIndex: "product",
            key: "product",
            render: (text, record) => (
                <Space size="middle">
                    <Checkbox
                        checked={selectedProductId.has(record.key)}
                        onChange={() => handleCheckbox(record.key)}
                    >
                        <img src={record.image} alt={text} style={{ width: 40, height: 40, borderRadius: 4 }} />
                    </Checkbox>
                    {text}
                    {console.log("Rendering product with key:", record.key)}
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
            render: (price) => `${price.toLocaleString()}VND`,
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
                    <DeleteOutlined style={{ color: "red", fontSize: "18px" }} onClick={() => handleDeleteProduct(record)} />
                </Space>
            ),
        },
    ];

    const [data, setData] = useState([
        { key: "1", product: "Mofusand Blind Box", category: "Doll", price: 300000, quantity: 0, status: "OUT OF STOCK", image: Mofusandimg },
        { key: "2", product: "Blind Box Kuromi", category: "Toy", price: 230000, quantity: 0, status: "OUT OF STOCK", image: Kuromiimg },
        { key: "3", product: "Crayon Shin Blind Box", category: "Toy", price: 155000, quantity: 978, status: "IN STOCK", image: CrayonShinimg },
        { key: "4", product: "Mofusand Blind Box", category: "Doll", price: 480000, quantity: 0, status: "OUT OF STOCK", image: Mofusandimg },
        { key: "5", product: "Crayon Shin Blind Box", category: "Toy", price: 256000, quantity: 30, status: "IN STOCK", image: CrayonShinimg },
    ]);

    return (
        <div className="p-5 bg-white rounded-lg">
            <div className="flex justify-between items-center mb-4">
                <Search placeholder="Search products..." allowClear enterButton={<SearchOutlined />} style={{ width: 300 }} onChange={(e) => setSearch(e.target.value)} />
                <Button className="bg-blue-500 text-white px-4 py-2 rounded mt-8" icon={<PlusOutlined />} onClick={handleAddProduct}>
                    Add New Campaign
                </Button>
            </div>
            {warning && <div className="w-full text-right text-red-500 mt-2">{warning}</div>}
            <Table columns={columns} dataSource={data.filter((item) => item.product.toLowerCase().includes(search.toLowerCase()))} pagination={{ pageSize: 10 }} />

            <Modal open={isCreateModalVisible} onCancel={handleCancelCreate} footer={null} width={720} closable={false}>
                <Pre_orderCampaignCreate onSuccess={handleCancelCreate} selectedProduct={selectedProduct} />
            </Modal>
            <Modal open={isEditModalVisible} onCancel={handleCancelEdit} footer={null} width={720} closable={false}>
                <Pre_orderCampaignEdit onSuccess={handleCancelEdit} selectedProduct={selectedProduct} />
            </Modal>

            <Modal
                title="Delete product"
                open={isDeleteModalVisible}
                onCancel={() => setIsDeleteModalVisible(false)}
                footer={[
                    <Button key="cancel" onClick={() => setIsDeleteModalVisible(false)}>
                        Cancel
                    </Button>,
                    <Button key="delete" type="primary" danger onClick={confirmDeleteProduct}>
                        Delete
                    </Button>,
                ]}
            >
                <p>Do you want to delete the product "{productToDelete?.product}"?</p>
            </Modal>
        </div>
    );
};

export default Pre_orderCampaign;
