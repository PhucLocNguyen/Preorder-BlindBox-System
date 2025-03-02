import React, { useState, useEffect} from 'react';
import CartItem from '../Customer/CartItem';
import VoucherModal from '../Customer/VoucherModal';

function CampaignBlock({ campaignId, items, vouchers, onUpdateQuantity, onRemoveItem, onApplyVoucher}) {
  // Quản lý voucher của khung này
  const [showVoucherModal, setShowVoucherModal] = useState(false);
  // Lưu cả object thay vì chỉ lưu string
  const [appliedVoucher, setAppliedVoucher] = useState(null);
  const [inputVoucherCode, setInputVoucherCode] = useState('');

  const availableVouchers = vouchers
    .map(v => ({
      id: v.userVoucherId,
      title: v.name,
      description: `Giảm ${v.percentDiscount}% tối đa ${v.maximumMoneyDiscount}`,
      //discountType: 'percentage',
      discountValue: v.percentDiscount / 100,
      quantity: v.quantity,
      maximumMoneyDiscount: v.maximumMoneyDiscount
    }));

  // Tính tạm tính (subtotal) = sum(amount)
  const subtotal = items.reduce((sum, item) => sum + item.amount, 0);

  // Tính giảm giá dựa theo voucher đã áp dụng
  const discountAmount = (() => {
    if (!appliedVoucher) return 0;
    // Kiểm tra điều kiện minOrder, nếu subtotal < minOrder thì không giảm
    if (subtotal < appliedVoucher.minOrder) {
      return 0;
    }
    // Tính theo discountType
      const calculatedDiscount = subtotal * appliedVoucher.discountValue;
      return Math.min(calculatedDiscount, appliedVoucher.maximumMoneyDiscount);
  })();

  // Tính tổng sau khi trừ giảm giá (ko âm)
  const total = Math.max(subtotal - discountAmount, 0);

  // Người dùng chọn voucher từ danh sách
  // const applyVoucherFromList = (voucherId) => {
  //   const chosenVoucher = availableVouchers.find((v) => v.id === voucherId);
  //   if (chosenVoucher) {
  //     setAppliedVoucher(chosenVoucher);
  //   }
  //   setShowVoucherModal(false);
  // };
  const applyVoucherFromList = (voucherId) => {
    const chosenVoucher = availableVouchers.find((v) => v.id === voucherId);
    if (chosenVoucher && chosenVoucher.quantity > 0) {
      setAppliedVoucher(chosenVoucher);
      onApplyVoucher(voucherId);  // Gọi callback cập nhật số lượng voucher
    }
    setShowVoucherModal(false);
  };

  // Người dùng nhập mã thủ công
  // const handleApplyInputVoucher = () => {
  //   if (inputVoucherCode.trim() !== '') {
  //     // Ở đây bạn có thể gọi API kiểm tra mã, hoặc tạm tạo object "giả"
  //     const manualVoucher = {
  //       id: -1,
  //       title: inputVoucherCode,
  //       description: 'Mã nhập tay',
  //       discountType: 'percentage', // ví dụ
  //       discountValue: 0.05,        // ví dụ: giảm 5%
  //       minOrder: 0,
  //     };
  //     setAppliedVoucher(manualVoucher);
  //     setInputVoucherCode('');
  //     setShowVoucherModal(false);
  //   }
  // };

  return (
    <div className="bg-white shadow rounded-lg mb-6 p-4">

      {/* Table hiển thị các item của campaign */}
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
          {items.map((item, index) => (
            <CartItem
              //key={`${item.preorderCampaignId}-${item.price}-${item.quantity}`}
              key={index}
              item={item}
              onUpdateQuantity={onUpdateQuantity}
              onRemoveItem={onRemoveItem}
            />
          ))}
        </tbody>
      </table>

      {/* Khu vực voucher + hiển thị tổng */}
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center mt-4 space-y-2 md:space-y-0">
        <div>
          <button
            className="bg-yellow-400 text-white px-4 py-2 rounded uppercase font-medium hover:bg-yellow-600"
            onClick={() => setShowVoucherModal(true)}
          >
            Chọn mã
          </button>
          {/* Hiển thị thông tin voucher đã chọn (nếu có) */}
          {appliedVoucher && (
            <div className="mt-2 text-sm bg-green-50 p-2 rounded">
              <p>
                <strong>Mã áp dụng:</strong> {appliedVoucher.title}
              </p>
              <p>{appliedVoucher.description}</p>
            </div>
          )}
        </div>

        <div className="text-right">
          <p className="mb-1">
            Tạm tính: <strong>{subtotal.toFixed(2)}</strong>
          </p>
          {/* Nếu không đủ điều kiện giảm giá => discountAmount = 0 */}
          {appliedVoucher && discountAmount === 0 && subtotal < appliedVoucher.minOrder && (
            <p className="text-red-500 mb-1">
              *Chưa đủ điều kiện áp dụng mã (đơn tối thiểu {appliedVoucher.minOrder})
            </p>
          )}
          {appliedVoucher && discountAmount > 0 && (
            <p className="mb-1">
              Giảm giá: -<strong>{discountAmount.toFixed(2)}</strong>
            </p>
          )}
          <p className="text-lg font-semibold">
            Tổng cộng: {total.toFixed(2)}
          </p>
        </div>
      </div>

      {/* Modal chọn voucher riêng cho khung này */}
      {showVoucherModal && (
        <VoucherModal
          vouchers={availableVouchers}
          inputVoucherCode={inputVoucherCode}
          setInputVoucherCode={setInputVoucherCode}
          //handleApplyInputVoucher={handleApplyInputVoucher}
          applyVoucherFromList={applyVoucherFromList}
          onClose={() => setShowVoucherModal(false)}
        />
      )}
    </div>
  );
}

export default CampaignBlock;
