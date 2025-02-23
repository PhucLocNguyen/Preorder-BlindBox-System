import React from 'react';
import { PlusCircleOutlined, MinusCircleOutlined, DeleteOutlined } from '@ant-design/icons';

export function CartItem({ item, onUpdateQuantity, onRemoveItem }) {
  // Lấy thông tin ảnh và mô tả từ blindBox
  const imageUrl = item.blindBox?.images?.mainImage?.url || '';
  const description = item.blindBox?.description || '';

  return (
    <tr>
      <td className="px-6 py-4 whitespace-nowrap">
        <div className="flex items-center">
          <img
            className="w-16 h-16 border border-gray-200 rounded"
            //src={item.image}
            src={imageUrl}
            alt="Product"
          />
          <div className="ml-4">
            <p className="text-gray-700">{description}</p>
          </div>
        </div>
      </td>
      <td className="px-6 py-4 whitespace-nowrap">
        <span className="text-gray-900">{item.price.toFixed(2)}</span>
      </td>
      <td className="px-6 py-4 whitespace-nowrap">
        <div className="flex items-center space-x-2">
          <button
            //onClick={() => onUpdateQuantity(item.id, -1)}
            onClick={() => onUpdateQuantity(item.preorderCampaignId, -1)}
            className="px-2 py-1 border border-gray-300 rounded"
          >
            <MinusCircleOutlined />
          </button>
          <input
            type="text"
            name="qty"
            value={item.quantity}
            readOnly
            className="h-full py-2 w-16 text-center border-t border-b border-gray-300"
          />
          <button
            //onClick={() => onUpdateQuantity(item.id, 1)}
            onClick={() => onUpdateQuantity(item.preorderCampaignId, 1)}
            className="px-2 py-1 border border-gray-300 rounded"
          >
            <PlusCircleOutlined />
          </button>
        </div>
      </td>
      <td className="px-6 py-4 whitespace-nowrap">
        <span className="text-gray-900">
          {/* {(item.price * item.quantity).toFixed(2)} */}
          {item.amount.toFixed(2)}
        </span>
      </td>
      <td className="px-6 py-4 whitespace-nowrap">
        <DeleteOutlined
          className="cursor-pointer"
          //onClick={() => onRemoveItem(item.id)}
          onClick={() => onRemoveItem(item.preorderCampaignId)}
        />
      </td>
    </tr>
  );
}
