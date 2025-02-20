import React, { useState } from 'react';
import { CartItem } from '../Customer/CartItem';
import { VoucherModal } from '../Customer/VoucherModal';

function Cart() {
  // Dữ liệu giỏ hàng
  const [cartItems, setCartItems] = useState([
    {
      id: 1,
      image:
        'https://storage.googleapis.com/a1aa/image/kM3j556XTtvDd4LOTmxbWNbBsAFLf5Z8KU-caJBPWw4.jpg',
      description: 'Minimalistic shop for multipurpose use',
      price: 360.0,
      quantity: 1,
    },
    {
      id: 2,
      image:
        'https://storage.googleapis.com/a1aa/image/kM3j556XTtvDd4LOTmxbWNbBsAFLf5Z8KU-caJBPWw4.jpg',
      description: 'Minimalistic shop for multipurpose use',
      price: 360.0,
      quantity: 1,
    },
    {
      id: 3,
      image:
        'https://storage.googleapis.com/a1aa/image/kM3j556XTtvDd4LOTmxbWNbBsAFLf5Z8KU-caJBPWw4.jpg',
      description: 'Minimalistic shop for multipurpose use',
      price: 360.0,
      quantity: 1,
    },
  ]);

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

  // Tăng/giảm số lượng
  const updateQuantity = (id, delta) => {
    setCartItems((prevItems) =>
      prevItems.map((item) =>
        item.id === id
          ? { ...item, quantity: Math.max(item.quantity + delta, 1) }
          : item
      )
    );
  };

  // Xóa sản phẩm
  const removeItem = (id) => {
    setCartItems((prevItems) => prevItems.filter((item) => item.id !== id));
  };

  // Clear toàn bộ giỏ hàng
  const clearCart = () => {
    setCartItems([]);
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
    (sum, item) => sum + item.price * item.quantity,
    0
  );

  return (
    <section className="py-24">
      <div className="container mx-auto px-4">
        <div className="bg-white shadow rounded-lg">
          <table className="table-fixed w-full divide-y divide-gray-200">
            <thead className="bg-gray-100">
              <tr>
                <th className="w-[40%] px-6 py-3 text-left text-sm font-medium text-gray-500 uppercase tracking-wider">
                  Product
                </th>
                <th className="w-[15%] px-6 py-3 text-left text-sm font-medium text-gray-500 uppercase tracking-wider">
                  Price
                </th>
                <th className="w-[20%] px-6 py-3 text-left text-sm font-medium text-gray-500 uppercase tracking-wider">
                  Quantity
                </th>
                <th className="w-[15%] px-6 py-3 text-left text-sm font-medium text-gray-500 uppercase tracking-wider">
                  Total
                </th>
                <th className="w-[10%] px-6 py-3 text-left text-sm font-medium text-gray-500 uppercase tracking-wider">
                  Action
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
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
                    className="bg-gray-200 text-gray-800 px-4 py-2 rounded uppercase"
                    onClick={clearCart}
                  >
                    Clear Cart
                  </button>
                </td>
                <td></td>
                {/* Gộp 3 cột cuối để chứa nút "Chọn hoặc nhập mã" */}
                <td className="px-6 py-4" colSpan={3}>
                  <button
                    className="bg-yellow-500 text-white w-48 h-10 rounded uppercase inline-flex items-center justify-center"
                    onClick={() => setShowVoucherModal(true)}
                  >
                    Chọn hoặc nhập mã
                  </button>
                </td>
              </tr>

              {/* Nếu đã áp dụng voucher thì hiển thị */}
              {appliedVoucher && (
                <tr>
                  <td className="px-6 py-4 font-medium text-green-600" colSpan={5}>
                    Đã áp dụng voucher: <strong>{appliedVoucher}</strong>
                  </td>
                </tr>
              )}

              {/* Subtotal */}
              <tr>
                <td></td>
                <td></td>
                <td className="px-6 py-4 font-medium">Subtotal</td>
                <td className="px-6 py-4 font-medium">${subtotal.toFixed(2)}</td>
                <td></td>
              </tr>

              {/* Checkout */}
              <tr>
                <td colSpan={4} className="px-6 py-4 text-right">
                  <div className="flex flex-col md:flex-row justify-end items-stretch md:space-x-4 space-y-2 md:space-y-0">
                    <button className="bg-gray-200 text-gray-800 px-4 py-2 rounded uppercase">
                      Continue Shopping
                    </button>
                    <button className="bg-yellow-500 text-white px-4 py-2 rounded uppercase">
                      Proceed to checkout
                    </button>
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
