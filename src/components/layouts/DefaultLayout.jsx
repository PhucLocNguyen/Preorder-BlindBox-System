import Footer from '../Footer/Footer';

function DefaultLayout({ children }) {
	return (
		<div className='relative'>
			<div>
				<div className="bg-[url('//theme.hstatic.net/1000069970/1001119059/14/bg-vth.jpg?v=6844')]">
					{children}
				</div>
				<Footer />
			</div>
		</div>
	);
}

export default DefaultLayout;
