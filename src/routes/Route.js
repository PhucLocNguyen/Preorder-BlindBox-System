import { Fragment, lazy } from "react";
const OrderDetailView = lazy(() => import("../pages/Staff/OrderDetailView"));
const AdminLayout = lazy(() => import("../components/layouts/AdminLayout"));
const DefaultLayout = lazy(() => import("../components/layouts/DefaultLayout"));
const Home = lazy(() => import("../pages/Home/Home"));
const LoginPage = lazy(() => import("../pages/Account/Login"));
const Register = lazy(() => import("../pages/Account/Register"));
const StaffView = lazy(() => import("../pages/Staff/StaffView"));
const ProductsView = lazy(() => import("../pages/Staff/ProductsView"));
const OrdersView = lazy(() => import("../pages/Staff/OrdersView"));
const NotificationsView = lazy(() => import("../pages/Staff/NotificationView"));

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
    ],
  },

  {
    path: "/account",
    layout: DefaultLayout,
    children: [
      { index: true, component: LoginPage },
      { path: "register", component: Register },
    ],
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
];
export { publicRoutes, privateRoutes };
