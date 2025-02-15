import React, { useState } from 'react';
import { Button, Pagination } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';


const projects = [
    {
        key: '1',
        name: 'SPRINGSALE',
        description: "Big Sale in Lunar New Year",
        participants: 5,
        dueDate: '04.01.24',

    },
    {
        key: '2',
        name: 'SHOPNOW20',
        description: "Discount 20% for all products",
        participants: 3,
        dueDate: '22.11.25',

    },
    {
        key: '3',
        name: 'NEWUSER50',
        description: "Discount 50% for new user",
        participants: 4,
        dueDate: '06.03.25',

    },
    {
        key: '4',
        name: 'DISCOUNT15',
        description: "Discount 15% for all products",
        participants: 6,
        dueDate: '14.09.25',

    },
    {
        key: '5',
        name: 'SUMMERFUN',
        description: "Discount 20% for all products in Summer Vacation",
        participants: 4,
        dueDate: '16.06.25',

    },
    {
        key: '6',
        name: 'FIRSTPURCHASE',
        description: "Enjoy an exclusive 10% discount on products released ahead of schedule during the pre-order phase.",
        participants: 4,
        dueDate: '16.08.25',

    },
    {
        key: '7',
        name: 'DAILYSPECIAL',
        description: "Enjoy a 10% discount on all products in the first week.",
        participants: 8,
        dueDate: '12.05.25',

    }
];

const ProjectCard = ({ project }) => {
    const navigate = useNavigate();

    const handleCardClick = () => {
        navigate(`/admin/voucher-details/${project.key}`);
    };
    return (
        <div
            className="bg-white shadow-md rounded-lg p-4 m-4 cursor-pointer"
            onClick={handleCardClick}
        >
            <div className="flex items-center">

                <div>
                    <h3 className="text-xl font-bold">{project.name}</h3>
                    <p className="text-gray-600">{project.description}</p>
                </div>
            </div>
            <div className="mt-4">
                <p className="text-gray-600">Participants: {project.participants}</p>
                <p className="text-gray-600">Due date: {project.dueDate}</p>
            </div>
        </div>
    );
};

const Voucher = () => {
    const [currentPage, setCurrentPage] = useState(1);
    const pageSize = 6;
    const navigate = useNavigate();



    const handleAddVoucher = () => {
        navigate('/admin/voucher/add');
    };
    const handlePageChange = (page) => {
        setCurrentPage(page);
    };

    const paginatedProjects = projects.slice((currentPage - 1) * pageSize, currentPage * pageSize);

    return (
        <div className="bg-gray-100 min-h-screen p-8">
            <h1 className="text-3xl font-bold mb-8">Some of Our Awesome Voucher</h1>
            <p className="text-gray-600 mb-8">This is the paragraph where you can write more details about your projects. Keep your user engaged by providing meaningful information.</p>
            <Button type="primary" icon={<PlusOutlined />} onClick={handleAddVoucher}>
                Add New Project
            </Button>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {paginatedProjects.map((project) => (
                    <ProjectCard key={project.key} project={project} />
                ))}
            </div>

            <Pagination
                className="mt-8"
                current={currentPage}
                pageSize={pageSize}
                total={projects.length}
                onChange={handlePageChange}
            />
        </div>
    );
};

export default Voucher;
