import { Mail } from 'lucide-react';
import { Input } from '../ui/input';
import { Link } from 'react-router';
import { Button } from '../ui/button';

export default function Footer() {
	return (
		<footer className='relative w-full bg-black text-white sec-com'>
			<div className='relative z-10 container-lg'>
				<div className='grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8'>
					{/* Logo Section */}
					<div className='space-y-4'>
						<h2 className='text-2xl font-bold'>Logo</h2>
						<p className='text-gray-300 text-sm'>
							Suspendisse henderit tellus laoreet luctus pharetra. Aliquam porttitor vitae orci nec
							ultricies. Curabitur vehicula, libero eget faucibus faucibus, purus erat eleifend enim,
							porta pellentesque ex mi ut sem.
						</p>
						<p className='text-gray-400 text-sm'>© 2014 BS3 UI Kit, All rights reserved</p>
					</div>

					{/* Menu Section */}
					<div className='space-y-4'>
						<h3 className='text-lg font-semibold mb-4'>MENU</h3>
						<nav className='space-y-2'>
							{['TRAVEL', 'NATURE', 'EXPLORES', 'SCIENCE', 'ADVICE'].map((item) => (
								<Link
									key={item}
									to='#'
									className='block text-gray-300 hover:text-white transition-colors'
								>
									{item}
								</Link>
							))}
						</nav>
					</div>

					{/* Links Section */}
					<div className='space-y-4'>
						<h3 className='text-lg font-semibold mb-4'>FOLLOW US</h3>
						<nav className='space-y-2'>
							{['Facebook', 'Twitter', 'Instagram', 'RSS'].map((item) => (
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

					{/* Newsletter Section */}
					<div className='space-y-4'>
						<h3 className='text-lg font-semibold'>NEWSLETTER</h3>
						<p className='text-gray-300'>A rover wearing a fuzzy suit doesn't alarm the real penguins</p>
						<div className='flex gap-2'>
							<Input
								type='email'
								placeholder='Search for...'
								className='bg-white/10 border-white/20 text-white placeholder:text-gray-400'
							/>
							<Button size='icon' variant='secondary'>
								<Mail className='h-4 w-4' />
								<span className='sr-only'>Subscribe</span>
							</Button>
						</div>
					</div>
				</div>
			</div>
		</footer>
	);
}
