import React, { useState } from 'react';
import { Button, Pagination } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import BigSale from '../../../assets/Admin/BigSale.png';
const projects = [
    {
        name: 'Slack Bot',
        description: "If everything I did failed - which it doesn't, I think that it actually succeeds.",
        participants: 5,
        dueDate: '02.03.22',
        icon: BigSale
    },
    {
        name: 'Premium support',
        description: "Pink is obviously a better color. Everyone's born confident, and everything's taken away from you.",
        participants: 3,
        dueDate: '22.11.21',
        icon: BigSale
    },
    {
        name: 'Design tools',
        description: "Constantly growing. We're constantly making mistakes from which we learn and improve.",
        participants: 4,
        dueDate: '06.03.20',
        icon: BigSale
    },
    {
        name: 'Looking great',
        description: "You have the opportunity to play this game of life you need to appreciate every moment.",
        participants: 6,
        dueDate: '14.03.24',
        icon: BigSale
    },
    {
        name: 'Developer First',
        description: "For standing out. But the time is now to be okay to be the greatest you.",
        participants: 4,
        dueDate: '16.01.22',
        icon: BigSale
    },
    {
        name: 'Product Development',
        description: "We strive to embrace and drive change in our industry. We are happy to work at such a project.",
        participants: 4,
        dueDate: '16.01.22',
        icon: BigSale
    },
    {
        name: 'Marketing Campaign',
        description: "We strive to embrace and drive change in our industry. We are happy to work at such a project.",
        participants: 8,
        dueDate: '16.05.22',
        icon: BigSale
    }
];

const ProjectCard = ({ project }) => (
    <div className="bg-white shadow-md rounded-lg p-4 m-4">
        <div className="flex items-center">
            <img src={project.icon} alt={project.name} className="w-12 h-12 mr-4" />
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

const Voucher = () => {
    const [currentPage, setCurrentPage] = useState(1);
    const pageSize = 6;

    const handlePageChange = (page) => {
        setCurrentPage(page);
    };

    const paginatedProjects = projects.slice((currentPage - 1) * pageSize, currentPage * pageSize);

    return (
        <div className="bg-gray-100 min-h-screen p-8">
            <h1 className="text-3xl font-bold mb-8">Some of Our Awesome Projects</h1>
            <p className="text-gray-600 mb-8">This is the paragraph where you can write more details about your projects. Keep your user engaged by providing meaningful information.</p>
            <Button type="primary" icon={<PlusOutlined />}>
                Add New Project
            </Button>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {paginatedProjects.map((project, index) => (
                    <ProjectCard key={index} project={project} />
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
