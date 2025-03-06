import React, { useCallback, useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { GetPriceInCart, UpdateQuantityInCart, ClearAllCart } from '../../api/Cart/ApiCart';
import {GetAllUserVoucher} from '../../api/UserVoucher/ApiUserVoucher';
import CampaignBlock from '../Customer/CampaignBlock';

function Cart() {
  const [cartItems, setCartItems] = useState([]);
  const [userVouchers, setUserVoucher] = useState([]);

  // Đây là state chứa dữ liệu gửi đi
  const [checkoutData, setCheckoutData] = useState({
    cartTotal: 0,
    blocks: [],
  });

  const fetchCartsApi = useCallback(async () => {
    try {
      const result = await GetPriceInCart();
      setCartItems(result);
    } catch (error) {
      console.error('Fetch Carts Error:', error);
      setCartItems([]);
    }
  }, []);

  const fetchUserVoucherApi = async () => {
    try {
      const data = await GetAllUserVoucher();
      setUserVoucher(data || []);
    } catch (error) {
      console.error("Error fetching voucher campaigns:", error);
      setUserVoucher([]);
    }
  };

  // Hàm giảm số lượng voucher khi voucher được áp dụng
  const handleApplyVoucher = (voucherId) => {
    setUserVoucher(prevVouchers =>
      prevVouchers.map(v => {
        if (v.userVoucherId === voucherId && v.quantity > 0) {
          return { ...v, quantity: v.quantity - 1 };
        }
        return v;
      })
    );
  };

  useEffect(() => {
    fetchCartsApi();
    fetchUserVoucherApi();
  }, []);

  // console.log('Cart Items:', cartItems);
  // console.log('Voucher Campaigns:', userVouchers);

  const updateQuantityApi = async (preorderCampaignId, delta) => {
    const itemsForCampaign = cartItems.filter(
      (item) => item.preorderCampaignId === preorderCampaignId
    );
    if (!itemsForCampaign.length) return;
  
    // Calculate the total current quantity for this campaign
    const currentTotalQuantity = itemsForCampaign.reduce(
      (sum, item) => sum + item.quantity,
      0
    );
  
    // Calculate the new total quantity (ensuring it’s at least 1)
    const newTotalQuantity = Math.max(currentTotalQuantity + delta, 1);
  
    // Prepare the request data for the backend
    const requestData = {
      PreorderCampaignId: preorderCampaignId,
      Quantity: newTotalQuantity,
    };
  
    try {
      // Call the API to update the quantity
      await UpdateQuantityInCart(requestData);
  
      // Fetch the updated cart items to reflect any splits or changes made by the backend
      const updatedResult = await GetPriceInCart();
      console.log("hello and smile");
      setCartItems(updatedResult);
    } catch (error) {
      console.error('Error updating quantity:', error);
    }
  };

  // Xoá 1 item (quantity = 0)
  const removeItem = async (preorderCampaignId) => {
    try {
      await UpdateQuantityInCart({
        PreorderCampaignId: preorderCampaignId,
        Quantity: 0,
      });
      setCartItems((prevItems) =>
        prevItems.filter((item) => item.preorderCampaignId !== preorderCampaignId)
      );
    } catch (error) {
      console.error('Error removing item:', error);
    }
  };

  // Clear toàn bộ cart
  const clearCart = async () => {
    try {
      await ClearAllCart();
      setCartItems([]);
    } catch (error) {
      console.error('Error clearing cart:', error);
    }
  };

  // Gom nhóm các item theo campaignId
  const groupedItems = cartItems.reduce((acc, item) => {
    const key = item.preorderCampaignId;
    if (!acc[key]) {
      acc[key] = [];
    }
    acc[key].push(item);
    return acc;
  }, {});

  // Tính tổng tiền toàn bộ cart (chưa tính voucher ở mỗi block)
  const totalCart = cartItems.reduce((sum, item) => sum + item.amount, 0);

  return (
    <section className="py-24">
      <div className="container mx-auto px-4">

        {/* Hiển thị mỗi campaignId như 1 “khung” (CampaignBlock) */}
        {Object.keys(groupedItems).map((campaignId) => (
          <CampaignBlock
            key={campaignId}
            campaignId={campaignId}
            items={groupedItems[campaignId]}
            vouchers={userVouchers}
            onUpdateQuantity={updateQuantityApi}
            onRemoveItem={removeItem}
            onApplyVoucher={handleApplyVoucher}
          />
        ))}

        {/* Tổng tiền cả cart */}
        <div className="text-right mt-4 text-xl font-bold">
          Tổng tiền của cả giỏ hàng: {totalCart.toFixed(2)}
        </div>

        {/* Footer chung cho cart: xoá giỏ hàng, thanh toán, v.v... */}
        <div className="flex justify-between items-center mt-4">
          <button
            className="bg-gray-200 text-gray-800 px-4 py-2 rounded uppercase hover:bg-gray-300"
            onClick={clearCart}
          >
            Xóa giỏ hàng
          </button>
          <div className="flex space-x-4">
            <Link
              to="/"
              className="bg-gray-200 text-gray-800 px-6 py-2 rounded uppercase font-medium text-center hover:bg-gray-300 transition-colors duration-200"
            >
              Tiếp tục mua sắm
            </Link>
            <Link
              to="/"
              className="bg-yellow-400 text-white px-6 py-2 rounded uppercase font-medium text-center hover:bg-yellow-600 transition-colors duration-200"
            >
              Thanh Toán
            </Link>
          </div>
        </div>
      </div>
    </section>
  );
}

export default Cart;
