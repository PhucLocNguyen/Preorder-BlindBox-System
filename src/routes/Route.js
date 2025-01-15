import { Fragment, lazy } from "react";
const AdminLayout = lazy(() => import("../components/layouts/AdminLayout") );
const DefaultLayout = lazy(() => import("../components/layouts/DefaultLayout") );
const Home = lazy(() => import("../pages/Home/Home") );
const Login = lazy(()=>import("../pages/Account/Login"))
const publicRoutes = [
    {
      index: true,
      component: Home,
      layout: DefaultLayout
    },
    {
      path: "/login",
      component: Login,
    }
  ];
  
  const privateRoutes = [
    {
      path: "/editor",
      component: Fragment,
      layout: AdminLayout
    },
    {
      path: "/projects",
      component: Fragment,
    },
  ];
  export { publicRoutes, privateRoutes };