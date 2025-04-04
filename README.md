# Preorder Blind Box Website

Dự án này cung cấp nền tảng web để khách hàng có thể đặt trước (pre-order) các sản phẩm Blind Box. Hệ thống tích hợp nhiều chức năng như quản lý chiến dịch pre-order, quản lý đơn hàng, ví điện tử, báo cáo doanh thu, và các tính năng thông báo cho người dùng.

---

## Mục lục
1. [Giới thiệu](#giới-thiệu)
2. [Các chức năng chính](#các-chức-năng-chính)
3. [Ảnh minh họa](#ảnh-minh-họa)
    - [1. Tạo chiến dịch Preorder Blindbox](#1-tạo-chiến-dịch-preorder-blindbox)
    - [2. Xác nhận thanh toán](#2-xác-nhận-thanh-toán)
    - [3. Trang Dashboard](#3-trang-dashboard)
    - [4. Giao diện trang chủ cho người dùng](#4-giao-diện-trang-chủ-cho-người-dùng)
    - [5. Quản lý đơn hàng dành cho Staff](#5-quản-lý-đơn-hàng-dành-cho-staff)
    - [6. Preorder Campaign (Bulk Order)](#6-preorder-campaign-bulk-order)
    - [7. Preorder Campaign (Time Pricing)](#7-preorder-campaign-time-pricing)
    - [8. Giao dịch ví điện tử](#8-giao-dịch-ví-điện-tử)
4. [Cài đặt & Sử dụng](#cài-đặt--sử-dụng)
5. [Nhóm phát triển](#nhóm-phát-triển)
6. [Giấy phép](#giấy-phép)

---

## Giới thiệu

Dự án **Preorder Blind Box Website** hướng đến việc cung cấp cho người bán và người mua một nền tảng chuyên biệt để đặt trước các sản phẩm “blind box” (hộp bí ẩn). Người dùng có thể:
- Đăng ký, đăng nhập tài khoản
- Nạp tiền vào ví, thanh toán đơn hàng
- Theo dõi tình trạng đơn hàng và nhận thông báo
- Quản lý chiến dịch pre-order với nhiều mức giá (theo thời gian hoặc theo số lượng)

Đội ngũ quản trị (admin, staff) có thể:
- Quản lý danh sách sản phẩm, banner, voucher
- Tạo các chiến dịch pre-order khác nhau (Time Pricing, Bulk Order)
- Theo dõi báo cáo doanh thu và tình trạng đơn hàng

---

## Các chức năng chính

- **Quản lý người dùng**: Đăng ký, đăng nhập, quên mật khẩu, phân quyền (Admin, Staff, Customer).
- **Quản lý chiến dịch pre-order**: Tạo mới, chỉnh sửa, xóa, phân loại chiến dịch (theo số lượng, theo thời gian).
- **Quản lý sản phẩm (Blind Box)**: Thêm, sửa, xóa sản phẩm; quản lý hình ảnh đính kèm.
- **Ví điện tử và giao dịch**: Nạp/rút tiền thông qua VNPay, MoMo; theo dõi lịch sử giao dịch.
- **Quản lý đơn hàng**: Tạo đơn pre-order, theo dõi và cập nhật trạng thái đơn hàng.
- **Báo cáo & Thống kê**: Thống kê doanh thu, số lượng đơn hàng, báo cáo sản phẩm bán chạy.
- **Thông báo (Notifications)**: Gửi thông báo đến người dùng khi có cập nhật đơn hàng hoặc chiến dịch.

---

## Ảnh minh họa

### 1. Tạo chiến dịch Preorder Blindbox
![CreatePreorderBlindboxCampaign](./images/CreatePreorderBlindboxCampaign.png)

Màn hình cho phép admin/staff tạo một chiến dịch pre-order mới cho sản phẩm Blind Box. Admin có thể nhập thông tin, thiết lập thời gian bắt đầu/kết thúc, hoặc thiết lập mức giá theo số lượng.

---

### 2. Xác nhận thanh toán
![ConfirmBy](./images/ConfirmBuy.png)

Giao diện xác nhận thanh toán, hiển thị chi tiết giao dịch và trạng thái thanh toán của người dùng (qua ví điện tử hoặc VNPay/MoMo).

---

### 3. Trang Dashboard
![Dashboard1](./images/Dashboard1.png)

Trang tổng quan (Dashboard) cho phép admin theo dõi nhanh các thông tin về doanh thu, số lượng đơn hàng, và những sản phẩm bán chạy.
![Dashboard2](./images/Dashboard2.png)
---

### 4. Giao diện trang chủ cho người dùng
![Homepage](./images//HomepageScreenshotUI.png)

Giao diện trang chủ hiển thị các chiến dịch và sản phẩm Blind Box nổi bật, người dùng có thể dễ dàng tìm kiếm và lựa chọn để đặt hàng.

---

### 5. Quản lý đơn hàng dành cho Staff
![OrderManagementForStaff](./images/OrderManagmentForStaff.png)

Staff có thể xem danh sách đơn hàng, cập nhật trạng thái (Processing, Confirmed, Canceled, Shipped, Completed), và xem chi tiết thông tin đơn hàng.

---

### 6. Preorder Campaign (Bulk Order)
![PreorderCampaignBulkOrder](./images/PreorderCampaignBulkOrder.png)

Ví dụ về giao diện chiến dịch pre-order loại “Bulk Order”, cho phép người dùng đặt hàng trước khi sản phẩm chính thức có hàng, giá sẽ giảm nếu số lượng đặt trước tăng.

---

### 7. Preorder Campaign (Time Pricing)
![PreorderCampaignTimePricing](./images/PreorderCampaignTimedPricing.png)

Ví dụ về giao diện chiến dịch pre-order loại “Time Pricing”, thiết lập nhiều mức giá theo khung thời gian hoặc số lượng giới hạn.

---

### 8. Giao dịch ví điện tử
![TransactionOfWallet](./images/TransactionOfWallet.png)

Người dùng có thể theo dõi chi tiết giao dịch nạp/rút tiền trong ví, cũng như các giao dịch thanh toán đơn hàng.

---

## Cài đặt & Sử dụng

1. **Clone dự án**  
   ```bash
   git clone https://github.com/PhucLocNguyen/Preorder-BlindBox-System.git
