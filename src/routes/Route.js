import { Fragment, lazy } from "react";

const OrderDetailView = lazy(() => import("../pages/Staff/OrderManagement/OrderDetailView"));
const AdminLayout = lazy(() => import("../components/layouts/AdminLayout"));
const DefaultLayout = lazy(() => import("../components/layouts/DefaultLayout"));
const Home = lazy(() => import("../pages/Home/Home"));
const LoginPage = lazy(() => import("../pages/Account/Login"));
const RegisterPage = lazy(() => import("../pages/Account/Register"));
const StaffView = lazy(() => import("../pages/Staff/StaffView"));
const ProductsView = lazy(() => import("../pages/Staff/ProductManagement/ProductsView"));
const OrdersView = lazy(() => import("../pages/Staff/OrderManagement/OrdersView"));
const NotificationsView = lazy(() => import("../pages/Staff/NotificationManagement/NotificationView"));
const ProductCreatePage = lazy(() => import("../pages/Staff/ProductManagement/ProductCreate"));
const ProductEditPage = lazy(() => import("../pages/Staff/ProductManagement/ProductEdit"));
const AdminView = lazy(() => import("../pages/Admin/AdminView"));
const Dashboard = lazy(() => import("../pages/Admin/Dashboard"));
const UserManagement = lazy(() => import("../pages/Admin/UserManagement/UserManagement"));
const UserManagementDetails = lazy(() => import("../pages/Admin/UserManagement/UserManagementDetails"));
const PreorderMilestone = lazy(() => import("../pages/Admin/PreorderMilestone"));
const Pre_orderCampaign = lazy(() => import("../pages/Admin/Pre-orderCampaign/PreorderCampaign"));
const VoucherManagement = lazy(() => import("../pages/Admin/VoucherCampaign/Voucher"));
const VoucherDetails = lazy(() => import("../pages/Admin/VoucherCampaign/VoucherDetails"));
const VoucherCreate = lazy(() => import("../pages/Admin/VoucherCampaign/VoucherCreate"));
const VoucherUpdate = lazy(() => import("../pages/Admin/VoucherCampaign/VoucherEdit"));
const ConfirmEmailAccount = lazy(() => import("../pages/ConfirmEmailAccount/ConfirmEmailAccount"));
import DepositPage from "../pages/Wallet/Deposit";
import Wallet from "../pages/Wallet/Wallet";
import Withdraw from "../pages/Wallet/Withdraw";
import { path } from "framer-motion/client";
const publicRoutes = [
  {
    index: true,
    component: Home,
    layout: DefaultLayout,
  },
  {
    path: "/login",
    component: LoginPage,
    layout: DefaultLayout,
  },
  {
    path: "/staff",
    component: StaffView,
    children: [
      { index: true, component: ProductsView },
      { path: "products", component: ProductsView },
      { path: "orders", component: OrdersView, },
      { path: "orders/:id", component: OrderDetailView },
      { path: "notifications", component: NotificationsView },
      { path: "product/create", component: ProductCreatePage },
      { path: "product/edit/:id", component: ProductEditPage },

    ],
  },
  {
    path: "/admin",
    component: AdminView,
    children: [
      { index: true, component: UserManagement },
      { path: "usermanagement", component: UserManagement },
      { path: "usermanagerment-details/:id", component: UserManagementDetails },
      { path: "preordermilestone", component: PreorderMilestone },
      { path: "pre-ordercampaign", component: Pre_orderCampaign },
      { path: "notifications", component: NotificationsView },
      { path: "dashboard", component: Dashboard },
      { path: "voucher", component: VoucherManagement },
      { path: "voucher/add", component: VoucherCreate },
      { path: "voucher-details/:id", component: VoucherDetails },
      { path: "voucher/update/:id", component: VoucherUpdate },
    ],
  },
  {
    path: "/account",
    layout: DefaultLayout,
    children: [
      { index: true, component: LoginPage },
      { path: "register", component: RegisterPage },
    ],
  },
  {
    path: "/register",
    component: RegisterPage,
    layout: DefaultLayout,
  },
  {
    path: '/confirmemail',
    component: ConfirmEmailAccount
  },
  {
    path: '/wallet',
    component: Wallet,
    layout: DefaultLayout,
    children: [
      { index: true, component: DepositPage, layout: null},
      { path: 'deposit', component: Withdraw, layout: null },
    ]
  },
  {
    path: '/test',
    component: DepositPage,
    layout: DefaultLayout,
  }
];

const privateRoutes = [
  {
    path: "/editor",
    component: Fragment,
    layout: AdminLayout,
  },
  {
    path: "/projects",
    component: Fragment,
  },
];
export { publicRoutes, privateRoutes };
