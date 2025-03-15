import { Link } from 'react-router';

export default function Footer() {
	return (
		<footer className='relative w-full bg-black text-white sec-com'>
			<div className='relative z-10 container-lg'>
				<div className='grid grid-cols-1 md:grid-cols-3 gap-8 text-center md:text-left'>
					{/* Logo Section */}
					<div className='space-y-4'>
						<h2 className='text-2xl font-bold'>Logo</h2>
						<p className='text-gray-300 text-sm'>
						Preorder Blindbox System - the place for blind box collectors! We offer the opportunity to pre-order the hottest collections at discounted prices and in limited quantities.
						</p>
						<p className='text-gray-400 text-sm'>Pre-order today to enjoy the discount</p>
					</div>

					{/* Links Section */}
					<div className='space-y-4'>
						<h3 className='text-lg font-semibold mb-4'>FOLLOW US</h3>
						<nav className='space-y-2'>
							{['Facebook', 'Twitter', 'Instagram', 'Tiktok'].map((item) => (
								<Link
									key={item}
									href='#'
									className='block text-gray-300 hover:text-white transition-colors'
								>
									{item}
								</Link>
							))}
						</nav>
					</div>

					

					{/* About Us Section */}
					<div className='space-y-4'>
						<h3 className='text-lg font-semibold'>About us</h3>
						<p className='text-gray-300'>Committed to providing an independent shopping experience, each product in the store is carefully selected, ensuring surprise and excitement for customers. Join now to not miss the opportunity to own exclusive items!</p>
					</div>
				</div>
			</div>
		</footer>
	);
}
