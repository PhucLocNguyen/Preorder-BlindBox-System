import React, { useState } from "react";
import { Table, Button, Modal, message } from "antd";
import { PlusOutlined, EditOutlined, DeleteOutlined } from "@ant-design/icons";
import Toroto from "../../../assets/Admin/Toroto.png";
import Labubu from "../../../assets/Admin/Labubu.png";
import BannerCreate from "./BannerCreate";
import BannerEdit from "./BannerEdit";

const BannerView = () => {
    const [banners, setBanners] = useState([
        { id: 1, title: "Spring Sale", image: Toroto, status: true },
        { id: 2, title: "New Collection", image: Labubu, status: false },
    ]);

    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
    const [isEditModalOpen, setIsEditModalOpen] = useState(false);
    const [editingBanner, setEditingBanner] = useState(null);

    const handleOpenCreateModal = () => {
        setIsCreateModalOpen(true);
    };

    const handleOpenEditModal = (banner) => {
        setEditingBanner(banner);
        setIsEditModalOpen(true);
    };

    const handleSaveCreate = (values) => {
        const newBanner = { ...values, id: banners.length + 1 };
        setBanners([...banners, newBanner]);
        message.success("Banner added successfully!");
        setIsCreateModalOpen(false);
    };

    const handleSaveEdit = (values) => {
        setBanners((prev) => prev.map((b) => (b.id === editingBanner.id ? { ...b, ...values } : b)));
        message.success("Banner updated successfully!");
        setIsEditModalOpen(false);
    };

    const handleDelete = (id) => {
        setBanners(banners.filter((b) => b.id !== id));
        message.success("Banner deleted successfully!");
    };

    const columns = [
        {
            title: "Image",
            dataIndex: "image",
            render: (src) => <img src={src} alt="Banner" className="w-16 h-16 rounded-lg" />,
        },
        {
            title: "Title",
            dataIndex: "title",
        },
        {
            title: "Status",
            dataIndex: "status",
            render: (status) => (
                <span className={`px-2 py-1 rounded ${status ? "bg-green-200 text-green-800" : "bg-red-200 text-red-800"}`}>
                    {status ? "Active" : "Inactive"}
                </span>
            ),
        },
        {
            title: "Actions",
            render: (_, record) => (
                <div className="flex gap-2">
                    <Button icon={<EditOutlined />} onClick={() => handleOpenEditModal(record)}>Edit</Button>
                    <Button icon={<DeleteOutlined />} danger onClick={() => handleDelete(record.id)}>Delete</Button>
                </div>
            ),
        },
    ];

    return (
        <div className="p-6 bg-white shadow-md rounded-lg">
            <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-bold">Banner Management</h2>
                <Button type="primary" icon={<PlusOutlined />} onClick={handleOpenCreateModal}>
                    Add Banner
                </Button>
            </div>

            <Table columns={columns} dataSource={banners} rowKey="id" />

            {/* Modal for Creating Banner */}
            <Modal
                title="Add Banner"
                open={isCreateModalOpen}
                onCancel={() => setIsCreateModalOpen(false)}
                footer={null}
            >
                <BannerCreate onSave={handleSaveCreate} />
            </Modal>

            {/* Modal for Editing Banner */}
            <Modal
                title="Edit Banner"
                open={isEditModalOpen}
                onCancel={() => setIsEditModalOpen(false)}
                footer={null}
            >
                <BannerEdit onSave={handleSaveEdit} editingBanner={editingBanner} />
            </Modal>
        </div>
    );
};

export default BannerView;
