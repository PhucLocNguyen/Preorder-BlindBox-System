import React, { useCallback, useEffect, useState } from 'react';
import { CartItem } from '../Customer/CartItem';
import { VoucherModal } from '../Customer/VoucherModal';
import { Link } from "react-router-dom";
import { GetPriceInCart, UpdateQuantityInCart, ClearAllCart } from "../../api/Cart/ApiCart";

function Cart() {
  const [cartItems, setCartItems] = useState([]);
  const fetchCarts = useCallback(async () => {
    try {
      const result = await GetPriceInCart();
      setCartItems([...result]);
    } catch (error) {
      console.error("Fetch Carts Error:", error);
      setCartItems([]);
    }
  });
  useEffect(() => {
    fetchCarts();
  }, []);

  // Quản lý trạng thái modal voucher
  const [showVoucherModal, setShowVoucherModal] = useState(false);

  // Danh sách voucher giả lập
  const [vouchers] = useState([
    { id: 1, title: 'Freeship 70k', description: 'Miễn phí vận chuyển cho đơn từ 70k' },
    { id: 2, title: 'Giảm 10%', description: 'Giảm 10% cho đơn từ 100k' },
  ]);

  // Lưu voucher đã áp dụng (có thể là string hoặc object)
  const [appliedVoucher, setAppliedVoucher] = useState('');

  // State để lưu input khi người dùng nhập mã thủ công
  const [inputVoucherCode, setInputVoucherCode] = useState('');

  // Hàm gọi API để cập nhật số lượng
  const updateQuantity = async (id, delta) => {
    // Tìm item dựa vào preorderCampaignId
    const itemToUpdate = cartItems.find(item => item.preorderCampaignId === id);
    if (!itemToUpdate) return;

    // Tính số lượng mới (ít nhất là 1)
    const newQuantity = Math.max(itemToUpdate.quantity + delta, 1);

    try {
      // Gọi API PUT cập nhật số lượng ở backend
      await UpdateQuantityInCart({
        PreorderCampaignId: id,
        Quantity: newQuantity,
      });
      // Nếu API thành công, cập nhật lại state
      setCartItems(prevItems =>
        prevItems.map(item =>
          item.preorderCampaignId === id 
            ? { 
                ...item, 
                quantity: newQuantity, 
                amount: item.price * newQuantity  // cập nhật lại amount nếu cần
              }
            : item
        )
      );
    } catch (error) {
      console.error("Error updating quantity:", error);
      // Có thể thông báo lỗi cho người dùng tại đây
    }
  };

  // Xóa sản phẩm
  // const removeItem = (id) => {
  //   setCartItems((prevItems) => prevItems.filter((item) => item.id !== id));
  // };
  const removeItem = async (id) => {
    try {
      await UpdateQuantityInCart({
        PreorderCampaignId: id,
        Quantity: 0,
      });
      // Nếu API thành công, cập nhật lại state loại bỏ item
      setCartItems(prevItems => prevItems.filter(item => item.preorderCampaignId !== id));
    } catch (error) {
      console.error("Error removing item:", error);
    }
  };

  // Clear toàn bộ giỏ hàng
  // const clearCart = () => {
  //   setCartItems([]);
  // };
  const clearCart = async () => {
    try {
      await ClearAllCart();
      setCartItems([]); // Cập nhật state rỗng
    } catch (error) {
      console.error("Error clearing cart:", error);
    }
  };

  // Người dùng chọn voucher từ danh sách
  const applyVoucherFromList = (voucherId) => {
    const chosenVoucher = vouchers.find((v) => v.id === voucherId);
    if (chosenVoucher) {
      setAppliedVoucher(chosenVoucher.title);
    }
    setShowVoucherModal(false);
  };

  // Người dùng nhấn "APPLY" khi nhập mã
  const handleApplyInputVoucher = () => {
    if (inputVoucherCode.trim() !== '') {
      setAppliedVoucher(inputVoucherCode);
      setInputVoucherCode('');
      setShowVoucherModal(false);
    }
  };

  // Tính tổng (subtotal)
  const subtotal = cartItems.reduce(
    // (sum, item) => sum + item.price * item.quantity,
    (sum, item) => sum + item.amount,
    0
  );

  // Tính số tiền giảm dựa theo voucher áp dụng (bạn có thể điều chỉnh logic này)
  const discountAmount = appliedVoucher
    ? appliedVoucher === "Freeship 70k"
      ? 70
      : appliedVoucher === "Giảm 10%"
      ? subtotal * 0.1
      : 0
    : 0;

  return (
    <section className="py-24">
      <div className="container mx-auto px-4">
        <div className="bg-white shadow rounded-lg">
          <table className="table-fixed w-full divide-y divide-gray-200">
            <thead className="bg-gray-100">
              <tr>
                <th className="w-[40%] px-6 py-3 text-left text-sm font-medium text-gray-500 uppercase tracking-wider">
                  Sản phẩm
                </th>
                <th className="w-[15%] px-6 py-3 text-left text-sm font-medium text-gray-500 uppercase tracking-wider">
                  Giá
                </th>
                <th className="w-[20%] px-6 py-3 text-left text-sm font-medium text-gray-500 uppercase tracking-wider">
                  Số lượng
                </th>
                <th className="w-[15%] px-6 py-3 text-left text-sm font-medium text-gray-500 uppercase tracking-wider">
                  Tổng
                </th>
                <th className="w-[10%] px-6 py-3"></th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200 ">
              {cartItems.map((item) => (
                <CartItem
                  key={item.id}
                  item={item}
                  onUpdateQuantity={updateQuantity}
                  onRemoveItem={removeItem}
                />
              ))}

              {/* Clear Cart & Nút Voucher */}
              <tr>
                <td className="px-6 py-4">
                  <button
                    className="bg-gray-200 text-gray-800 px-4 py-2 rounded uppercase hover:bg-gray-300"
                    onClick={clearCart}
                  >
                    Xóa giỏ hàng
                  </button>
                </td>
                <td></td>
                {/* Gộp 3 cột cuối để chứa nút "Chọn hoặc nhập mã" */}
                <td className="px-6 py-4" colSpan={3}>
                  <button
                    className="bg-yellow-400 text-white w-48 h-10 rounded uppercase inline-flex items-center justify-center hover:bg-yellow-600"
                    onClick={() => setShowVoucherModal(true)}
                  >
                    Chọn hoặc nhập mã
                  </button>
                </td>
              </tr>

              {/* Hiển thị voucher đã áp dụng */}
              {appliedVoucher && (
                <tr>
                  <td className="px-6 py-4 font-medium text-green-600" colSpan={5}>
                    Đã áp dụng voucher: <strong>{appliedVoucher}</strong>
                  </td>
                </tr>
              )}

              {/* Dòng tổng cộng ban đầu */}
              <tr>
                <td></td>
                <td></td>
                <td className="px-6 py-4 font-medium">Tổng cộng</td>
                <td className="px-6 py-4 font-medium">{subtotal.toFixed(2)}</td>
                <td></td>
              </tr>

              {/* Nếu đã áp dụng voucher thì hiển thị dòng tiền sau khi trừ voucher */}
              {appliedVoucher && (
                <tr>
                  <td></td>
                  <td></td>
                  <td className="px-6 py-4 font-medium">Tổng sau voucher</td>
                  <td className="px-6 py-4 font-medium">
                    {(subtotal - discountAmount).toFixed(2)}
                  </td>
                  <td></td>
                </tr>
              )}

              {/* Checkout */}
              <tr>
                <td colSpan={4} className="px-6 py-4 text-right">
                  <div className="flex flex-col md:flex-row justify-end items-stretch md:space-x-4 space-y-2 md:space-y-0">
                    <Link
                      to="/"
                      className="inline-block bg-gray-200 text-gray-800 px-6 py-2 rounded uppercase font-medium text-center hover:bg-gray-300 transition-colors duration-200"
                    >
                      <span className="text-[#333] text-[16px]">Tiếp tục mua sắm</span>
                    </Link>
                    <Link
                      to="/"
                      className="inline-block bg-yellow-400 text-white px-6 py-2 rounded uppercase font-medium text-center hover:bg-yellow-600 transition-colors duration-200"
                    >
                      <span className="text-[#f0ecec] text-[16px]">Thanh Toán</span>
                    </Link>
                  </div>
                </td>
                <td></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      {/* Modal voucher */}
      {showVoucherModal && (
        <VoucherModal
          vouchers={vouchers}
          inputVoucherCode={inputVoucherCode}
          setInputVoucherCode={setInputVoucherCode}
          handleApplyInputVoucher={handleApplyInputVoucher}
          applyVoucherFromList={applyVoucherFromList}
          onClose={() => setShowVoucherModal(false)}
        />
      )}
    </section>
  );
}

export default Cart;
