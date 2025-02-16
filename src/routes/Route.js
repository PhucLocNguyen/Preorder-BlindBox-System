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
const ProductCreatePage = lazy(()  => import("../pages/Staff/ProductManagement/ProductCreate"));
const ProductEditPage = lazy(()  => import("../pages/Staff/ProductManagement/ProductEdit"));
const AdminView = lazy(() => import("../pages/Admin/AdminView"));
const Dashboard = lazy(() => import("../pages/Admin/Dashboard"));
const UserManagement = lazy(() => import("../pages/Admin/UserManagement/UserManagement"));
const CampaignManagement = lazy(() => import("../pages/Admin/CampaignManagement"));
const Pre_orderCampaign = lazy(() => import("../pages/Admin/Pre-orderCampaign/PreorderCampaign"));
const VoucherManagerment = lazy(() => import("../pages/Admin/VoucherCampaign/Voucher"));
const VoucherDetails = lazy(() => import("../pages/Admin/VoucherCampaign/VoucherDetails"));
const VoucherCreate = lazy(() => import("../pages/Admin/VoucherCampaign/VoucherCreate"));
const ConfirmEmailAccount = lazy(() => import("../pages/ConfirmEmailAccount/ConfirmEmailAccount"));
const publicRoutes = [
  {
    index: true,
    component: Home,
    layout: DefaultLayout,
  },
  {
    path: "/login",
    component: LoginPage,
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
      { index: true, component: UserManagerment },
      { path: "usermanagement", component: UserManagement },
      { path: "campaignmanagement", component: CampaignManagement },
      { path: "pre-ordercampaign", component: Pre_orderCampaign },
      { path: "notifications", component: NotificationsView },
      { path: "dashboard", component: Dashboard },
      { path: "voucher", component: VoucherManagerment },
      { path: "voucher/add", component: VoucherCreate },
      { path: "voucher-details/:id", component: VoucherDetails },
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
    component: RegisterPage
  },
  {
    path: '/confirmemail',
    component: ConfirmEmailAccount
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
