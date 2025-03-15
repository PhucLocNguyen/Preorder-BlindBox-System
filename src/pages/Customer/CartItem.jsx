import React, { useState, useEffect } from 'react';
import { PlusCircleOutlined, MinusCircleOutlined, DeleteOutlined } from '@ant-design/icons';
import { formatMoney } from '../../utils/FormatMoney';

function CartItem({ item, onUpdateQuantity, onRemoveItem }) {
  const [quantity, setQuantity] = useState(item.quantity);

  // Mỗi khi props.item.quantity thay đổi (do fetchCarts() trả về kết quả mới),
  // ta cập nhật lại state local.
  useEffect(() => {
    setQuantity(item.quantity);
  }, [item.quantity]);

  const handleInputChange = (e) => {
    const value = e.target.value;
    const num = parseInt(value, 10);
    if (!isNaN(num)) {
      setQuantity(num);
    } else {
      setQuantity(''); // Xóa nếu nhập không phải số
    }
  };

  const handleInputBlur = () => {
    // Sử dụng giá trị từ state, tức là số mới mà người dùng đã nhập.
    const newQuantity = Math.max(quantity, 1);
    console.log(newQuantity);
    if (newQuantity !== item.quantity) {
      // Gọi hàm updateQuantity ở parent với delta là hiệu giữa newQuantity và item.quantity
      onUpdateQuantity(item.preorderCampaignId, newQuantity - item.quantity);
    }
    // Cập nhật lại state local, nếu backend trả về dữ liệu mới thì useEffect sẽ đồng bộ
    setQuantity(newQuantity);
  };

  const imageUrl = item.blindBox?.images?.mainImage?.url || '';
  const name = item.blindBox?.name || '';

  return (
    <tr>
      <td className="px-6 py-4 whitespace-nowrap">
        <div className="flex items-center">
          <img
            className="w-16 h-16 border border-gray-200 rounded"
            src={imageUrl}
            alt="Product"
          />
          <div className="ml-4">
            <p className="text-gray-700">{name}</p>
          </div>
        </div>
      </td>
      <td className="px-6 py-4 whitespace-nowrap">
        <span className="text-gray-900">{formatMoney(item.price)}</span>
      </td>
      <td className="px-6 py-4 whitespace-nowrap">
        <div className="flex items-center space-x-2">
          <button
            onClick={() => onUpdateQuantity(item.preorderCampaignId, -1)}
            className="px-2 py-1 border border-gray-300 rounded"
          >
            <MinusCircleOutlined />
          </button>
          <input
            type="text"
            name="qty"
            value={quantity}
            readOnly
            // onChange={handleInputChange}
            // onBlur={handleInputBlur}
            className="h-full py-2 w-16 text-center border-t border-b border-gray-300"
          />
          <button
            onClick={() => onUpdateQuantity(item.preorderCampaignId, 1)}
            className="px-2 py-1 border border-gray-300 rounded"
          >
            <PlusCircleOutlined />
          </button>
        </div>
      </td>
      <td className="px-6 py-4 whitespace-nowrap">
        <span className="text-gray-900">
          {formatMoney(item.amount)}
        </span>
      </td>
      <td className="px-6 py-4 whitespace-nowrap">
        <DeleteOutlined
          className="cursor-pointer"
          onClick={() => onRemoveItem(item.preorderCampaignId)}
        />
      </td>
    </tr>
  );
}

export default CartItem;
