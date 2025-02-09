import { Button, Pagination, Space, Spin, Table } from "antd";
import React, { useCallback, useState } from "react";
import { useEffect } from "react";
import { GetTheActiveBlindBox } from "../../../api/BlindBox/ApiBlindBox";
import useFetchData from "../../../hooks/useFetchData";
import noThumbnailImage from "../../../assets/noThumbnailImage.jpg"
const ProductsView = () => {
  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(1);

  // useCallback để truyền fetchCallback có tham số
  const fetchBlindBoxes = useCallback(
    () => GetTheActiveBlindBox(pageSize, pageIndex),
    [pageSize, pageIndex]
  );
  const { data, loading, refetch, pagination, updatePagination } = useFetchData(
    fetchBlindBoxes,
    [pageSize, pageIndex]
  );
  const columns = [
    {
      title: "STT",
      key: "index",
      render: (_, __, index) =>
        (pagination.current - 1) * pagination.pageSize + index + 1,
    },
    {
      title: "Ảnh chính",
      dataIndex: ["images", "mainImage", "url"], // Lấy ảnh từ nested object
      key: "mainImage",
      render: (url) => (
        <img
          src={url ? url : noThumbnailImage}
          alt="Main"
          className="w-32 h-32 object-contain rounded-md shadow-md"
        />
      ),
    },

    { title: "Name", dataIndex: "name", key: "name" },
    { title: "Description", dataIndex: "description", key: "description" },
    { title: "Size", dataIndex: "size", key: "size" },
    {
      title: "Created At",
      dataIndex: "createdAt",
      key: "createdAt",
      render: (date) => new Date(date).toLocaleString(),
    },
    {
      title: "Action",
      key: "action",
      render: (_, record) => (
        <Space size="middle">
          <a href={"product/edit/" + record.blindBoxId}>Update</a>
          <a>Delete</a>
        </Space>
      ),
    },
  ];
  useEffect(() => {
    console.log(pagination);
  }, [data]);
  return (
    <div className="w-full mx-auto mt-5 p-5 bg-white shadow-lg rounded-lg">
      <div className="flex justify-between">
        <h2 className="text-2xl font-bold mb-4">Quản lý Blind box</h2>
        <div>
          <Button href="product/create/">Thêm sản phẩm</Button>
        </div>
      </div>

      {/* Hiển thị khi đang tải dữ liệu */}
      {loading ? (
        <div className="flex justify-center">
          <Spin size="large" />
        </div>
      ) : (
        <>
          {/* Bảng dữ liệu */}
          <Table
            bordered={true}
            columns={columns}
            dataSource={data}
            pagination={false} // Ẩn phân trang mặc định của Ant Design
            rowKey="id"
            className="border rounded-md"
          />

          {/* Phân trang custom */}
          <div className="flex justify-end mt-4">
            <Pagination
              current={pagination.current}
              total={pagination.total}
              pageSize={pagination.pageSize}
              onChange={(page) => {
                setPageIndex(page);
              }}
              
              className="shadow-md p-2 rounded-md"
            />
          </div>
        </>
      )}
    </div>
  );
};

export default ProductsView;
