import { Fragment, lazy } from "react";

const PreorderCampaignCreate = lazy(() => import("../pages/Admin/Pre-orderCampaign/PreorderCampaignCreate"));
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
const Pre_orderCampaignDetails = lazy(() => import("../pages/Admin/Pre-orderCampaign/Pre_orderCampaignDetails"));
const VoucherManagement = lazy(() => import("../pages/Admin/VoucherCampaign/Voucher"));
const VoucherDetails = lazy(() => import("../pages/Admin/VoucherCampaign/VoucherDetails"));
const VoucherCreate = lazy(() => import("../pages/Admin/VoucherCampaign/VoucherCreate"));
const VoucherUpdate = lazy(() => import("../pages/Admin/VoucherCampaign/VoucherEdit"));
const ConfirmEmailAccount = lazy(() => import("../pages/ConfirmEmailAccount/ConfirmEmailAccount"));
const Cart = lazy(() => import("../pages/Customer/Cart"));
const ProductList = lazy(() => import('../pages/Home/ProductList/ProductList'));
const ProductDetail = lazy(() => import('../pages/Products/ProductDetail/ProductDetail'));
const DepositPage = lazy(() => import("../pages/Wallet/Deposit"));
const Wallet = lazy(() => import("../pages/Wallet/Wallet"));
const Withdraw = lazy(() => import("../pages/Wallet/Withdraw"));
const WalletRechargeResponse = lazy(() => import("../pages/Wallet/WalletRechargeResponse"));
const SearchResultPage = lazy(() => import("../pages/SearchResultPage/SearchResultPage"));

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
    path: '/cart',
    component: Cart,
    layout: DefaultLayout,
  },
  {
		path: '/preordercampaign',
    layout:DefaultLayout,
		children: [
			{ index: true, component: ProductList },
			{ path: ':slug', component: ProductDetail },
		],
		layout: DefaultLayout,
	},
  {
    path: '/campaign/search',
    component: SearchResultPage,
    layout: DefaultLayout
  },
  {
    path: '/test',
    component: SearchResultPage,
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
    role: ['staff']
  },
  {
    path: "/admin",
    component: AdminView,
    children: [
      { index: true, component: UserManagement },
      { path: "usermanagement", component: UserManagement },
      { path: "usermanagement/details/:id", component: UserManagementDetails },
      { path: "preordermilestone", component: PreorderMilestone },
      { path: "pre-ordercampaign", component: Pre_orderCampaign },
      { path: "pre-ordercampaign/details/:slug", component: Pre_orderCampaignDetails },
      { path: "pre-ordercampaign/create", component: PreorderCampaignCreate },

      { path: "notifications", component: NotificationsView },
      { path: "dashboard", component: Dashboard },
      { path: "voucher", component: VoucherManagement },
      { path: "voucher/add", component: VoucherCreate },
      { path: "voucher-details/:id", component: VoucherDetails },
      { path: "voucher/update/:id", component: VoucherUpdate },
    ],
    role: ['admin']
  },
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
  {
    path: '/wallet',
    component: Wallet,
    layout: DefaultLayout,
    children: [
      { index: true, component: DepositPage, layout: null },
      { path: 'deposit', component: Withdraw, layout: null },
    ],
    role: ['Customer']
  },
  {
    path: '/wallet/paymentResponse',
    component: WalletRechargeResponse,
    layout: DefaultLayout,
    role: ['Customer']
  },
 
];
export { publicRoutes, privateRoutes };
