import Header from "../Header/Header";

function DefaultLayout({ children }) {
  return (
    <>
      <Header />
      <div className="relative">
        <div>{children}</div>
      </div>
    </>
  );
}

export default DefaultLayout;