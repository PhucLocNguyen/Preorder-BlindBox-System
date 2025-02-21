import Footer from '../Footer/Footer';
import Header from '../Header/Header';

function DefaultLayout({ children }) {
	return (
		<div className='relative'>
			<div>
				<Header/>
				<div className="bg-[url('//theme.hstatic.net/1000069970/1001119059/14/bg-vth.jpg?v=6844')]">
					{children}
				</div>
				<Footer />
			</div>
		</div>
	);
}

export default DefaultLayout;
