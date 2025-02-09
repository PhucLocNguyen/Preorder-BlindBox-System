import React, { useState } from 'react';
import {
    Space, Table, Tag, Input, Button, Modal, message
} from 'antd';
import {
    EditOutlined,
    EyeOutlined,
    DeleteOutlined,
    SearchOutlined,
    PlusOutlined,
} from '@ant-design/icons';
import UserManagermentCreate from "./UserManagermentCreate";
import UserManagermentEdit from "./UserManagermentEdit";

const { Search } = Input;

const UserManagerment = () => {
    const [selectedUser, setSelectedUser] = useState(null);
    const [search, setSearch] = useState("");
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
    const [userToDelete, setUserToDelete] = useState(null);
    const [data, setData] = useState([
        { key: '1', fullname: 'John Brown', phone: '0123456789', address: 'New York No. 1 Lake Park', email: 'john123@gmail.com', tags: ['active'], },
        { key: '2', fullname: 'Jim Green', phone: '0987654321', address: 'London No. 1 Lake Park', email: 'jimcrazyguy@gmail.com', tags: ['inactive'], },
        { key: '4', fullname: 'Alice White', phone: '0933344556', address: 'Los Angeles No. 2 Lake Park', email: 'alicewhite@gmail.com', tags: ['inactive'], },
        { key: '5', fullname: 'Bob Yellow', phone: '0922233445', address: 'Chicago No. 3 Lake Park', email: 'bobyellow@gmail.com', tags: ['active'], },
        { key: '6', fullname: 'Carol Blue', phone: '0911122334', address: 'Houston No. 4 Lake Park', email: 'carolblue@gmail.com', tags: ['inactive'], },
        { key: '7', fullname: 'David Pink', phone: '0903344557', address: 'Phoenix No. 5 Lake Park', email: 'davidpink@gmail.com', tags: ['active'], },
        { key: '8', fullname: 'Eva Orange', phone: '0892233445', address: 'San Antonio No. 6 Lake Park', email: 'evaorange@gmail.com', tags: ['inactive'], },
        { key: '9', fullname: 'Frank Purple', phone: '0881122334', address: 'San Diego No. 7 Lake Park', email: 'frankpurple@gmail.com', tags: ['active'], },
        { key: '10', fullname: 'Grace Gray', phone: '0873344556', address: 'Dallas No. 8 Lake Park', email: 'gracegray@gmail.com', tags: ['inactive'], }
    ]);

    const handleAddUser = () => {
        setIsModalVisible(true);
    };

    const handleCancel = () => {
        setIsModalVisible(false);
    };

    const handleEditUser = (record) => {
        setSelectedUser(record);
        setIsModalVisible(true);
    };

    const handleDeleteUser = (key) => {
        setUserToDelete(key);
        setIsDeleteModalVisible(true);
    };

    const confirmDeleteUser = () => {
        setData((prevData) => prevData.filter((user) => user.key !== userToDelete.key));
        setIsDeleteModalVisible(false);
        message.success(`User "${userToDelete.fullname}" has been deleted.`);
        setUserToDelete(null);
    };

    const columns = [
        {
            title: 'Full Name',
            dataIndex: 'fullname',
            key: 'fullname',
        },
        {
            title: 'Phone',
            dataIndex: 'phone',
            key: 'phone',
        },
        {
            title: 'Email',
            dataIndex: 'email',
            key: 'email',
        },
        {
            title: 'Address',
            dataIndex: 'address',
            key: 'address',
        },
        {
            title: 'Status',
            key: 'status',
            dataIndex: 'tags',
            render: (tags) => (
                <>
                    {tags.map((tag) => (
                        <Tag color={tag === 'active' ? 'green' : 'red'} key={tag}>
                            {tag.toUpperCase()}
                        </Tag>
                    ))}
                </>
            ),
        },
        {
            title: 'Action',
            key: 'action',
            render: (_, record) => (
                <Space size="middle">
                    <EyeOutlined style={{ color: "blue", fontSize: '20px' }} />
                    <EditOutlined
                        style={{ color: "orange", fontSize: '20px' }}
                        onClick={() => handleEditUser(record)}
                    />
                    <DeleteOutlined
                        style={{ color: "red", fontSize: '20px' }}
                        onClick={() => handleDeleteUser(record)}
                    />
                </Space>
            ),
        }
    ];
    return (
        <div className="p-5 bg-white rounded-lg">
            <div className="flex justify-between items-center mb-4">
                <Search
                    placeholder="Search here..."
                    allowClear
                    enterButton={<SearchOutlined />}
                    style={{ width: 300 }}
                    onChange={(e) => setSearch(e.target.value)}
                />
                <Button type="primary" icon={<PlusOutlined />} onClick={handleAddUser}>
                    Add New User
                </Button>
            </div>
            <div className='text-center'>
                <Table
                    columns={columns}
                    dataSource={data.filter((item) =>
                        item.fullname.toLowerCase().includes(search.toLowerCase())
                    )}
                    pagination={{ pageSize: 10 }}
                />
            </div>

            {/* Modal hiển thị form tạo mới người dùng */}
            <Modal
                open={isModalVisible}
                onCancel={handleCancel}
                footer={null}
                width={720}
                height={250}
                forceRender
                closable={false}
            >
                <UserManagermentCreate onSuccess={() => setIsModalVisible(false)} />
            </Modal>
            {/* Modal hiển thị form cập nhật người dùng */}
            <Modal
                open={isModalVisible}
                onCancel={handleCancel}
                footer={null}
                width={720}
                height={250}
                forceRender
                closable={false}
            >
                <UserManagermentEdit onSuccess={() => setIsModalVisible(false)} />
            </Modal>
            {/* Modal hiển thị xác nhận xóa người dùng */}
            <Modal
                title=" Delete user"
                open={isDeleteModalVisible}
                onCancel={() => setIsDeleteModalVisible(false)}
                footer={[
                    <Button key="cancel" onClick={() => setIsDeleteModalVisible(false)}>
                        Cancel
                    </Button>,
                    <Button key="delete" type="primary" danger onClick={confirmDeleteUser}>
                        Delete
                    </Button>,
                ]}
            >
                <p>Do you want to delete  user "{userToDelete?.fullname}"?</p>
            </Modal>
        </div>
    );
};

export default UserManagerment;
