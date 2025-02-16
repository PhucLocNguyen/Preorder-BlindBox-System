import { useState } from 'react';
import { ChevronRight, Clock, CreditCard, Menu, Phone, RefreshCw, ThumbsUp } from 'lucide-react';
import { Link } from 'react-router';
import { Sheet, SheetContent, SheetTrigger } from '../ui/sheet';
import { Button } from '../ui/button';
import { Collapsible, CollapsibleContent, CollapsibleTrigger } from '../ui/collapsible';
import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/css/effect-fade';
import 'swiper/css';

import { Autoplay, EffectFade } from 'swiper/modules';

const mainNavItems = ['DANH MỤC SẢN PHẨM', 'TRANG CHỦ', 'GIỚI THIỆU', 'SẢN PHẨM', 'TIN TỨC', 'LIÊN HỆ'];

const categories = [
	{ name: 'Thời trang nam', hasSubmenu: true },
	{ name: 'Thời trang nữ', hasSubmenu: true },
	{ name: 'Áo khoác da', hasSubmenu: false },
	{ name: 'Phụ kiện thời trang', hasSubmenu: false },
	{ name: 'Quần short đẹp', hasSubmenu: false },
	{ name: 'Áo thun nữ', hasSubmenu: false },
	{ name: 'Quần dài cá tính', hasSubmenu: false },
	{ name: 'Giày dép', hasSubmenu: false },
	{ name: 'Phụ kiện thời trang', hasSubmenu: false },
];

const submenuItems = {
	'Thời trang nam': ['Áo thun nữ', 'Quần dài'],
	'Thời trang nữ': ['Áo sơ mi nữ', 'Váy đầm'],
};

export default function Navbar() {
	const [activeSubmenu, setActiveSubmenu] = useState('');
	const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

	return (
		<div className='flex flex-col'>
			{/* Top Navigation */}
			<nav className='bg-black text-white'>
				<div className='mx-auto container-lg flex max-w-7xl items-center justify-between px-4 py-3 lg:justify-start lg:space-x-8'>
					{/* Mobile Menu Button */}
					<Sheet>
						<SheetTrigger asChild>
							<Button variant='ghost' size='icon' className='lg:hidden'>
								<Menu className='h-6 w-6 text-white' />
							</Button>
						</SheetTrigger>
						<SheetContent side='left' className='w-[300px] p-0'>
							<div className='border-b p-4'>
								<h2 className='text-lg font-semibold'>Menu</h2>
							</div>
							{/* Mobile Navigation Links */}
							<div className='p-4'>
								{mainNavItems.map((item) => (
									<Link key={item} to='#' className='block py-2 text-sm hover:text-gray-500'>
										{item}
									</Link>
								))}
								{/* Mobile Categories */}
								<div className='mt-4 space-y-2'>
									{categories.map((category) => (
										<Collapsible key={category.name}>
											<CollapsibleTrigger className='flex w-full items-center justify-between py-2 text-sm hover:text-gray-500'>
												{category.name}
												{category.hasSubmenu && <ChevronRight className='h-4 w-4' />}
											</CollapsibleTrigger>
											{category.hasSubmenu && (
												<CollapsibleContent>
													<div className='ml-4 space-y-2'>
														{submenuItems[category.name].map((item) => (
															<Link
																key={item}
																to='#'
																className='block py-2 text-sm hover:text-gray-500'
															>
																{item}
															</Link>
														))}
													</div>
												</CollapsibleContent>
											)}
										</Collapsible>
									))}
								</div>
							</div>
						</SheetContent>
					</Sheet>

					{/* Desktop Navigation */}
					<div className='hidden lg:flex lg:space-x-8 py-4'>
						{mainNavItems.map((item) => (
							<Link key={item} to='#' className='text-sm hover:text-gray-300'>
								{item}
							</Link>
						))}
					</div>
				</div>
			</nav>

			<div className='flex flex-col lg:flex-row container-lg !px-0 md:px-4 !py-4'>
				{/* Left Sidebar - Hidden on mobile */}
				<div className='hidden lg:block lg:w-1/3'>
					<div className='space-y-1 divide-y shadow-lg'>
						{categories.map((category) => (
							<div
								key={category.name}
								className='relative'
								onMouseEnter={() => category.hasSubmenu && setActiveSubmenu(category.name)}
								onMouseLeave={() => setActiveSubmenu(null)}
							>
								<Link
									to='#'
									className='flex items-center justify-between px-4 py-2 text-sm hover:bg-gray-100'
								>
									{category.name}
									{category.hasSubmenu && <ChevronRight className='h-4 w-4' />}
								</Link>

								{/* Desktop Submenu */}
								{activeSubmenu === category.name && (
									<div className='absolute left-full top-0 z-50 w-full border bg-white p-1 shadow-lg'>
										{submenuItems[category.name].map((item) => (
											<Link
												key={item}
												to='#'
												className='block px-4 py-2 text-sm hover:bg-gray-100'
											>
												{item}
											</Link>
										))}
									</div>
								)}
							</div>
						))}
					</div>
				</div>

				{/* Main Content */}
				<main className='w-full lg:w-3/5'>
					<div className='relative text-center w-full'>
						<Swiper
							spaceBetween={0}
							effect={'fade'}
							autoplay={{
								delay: 2500,
							}}
							modules={[Autoplay, EffectFade]}
							className='mySwiper object-contain h-72 lg:h-[365px]'
							loop
						>
							<SwiperSlide>
								<img
									src='https://bizweb.dktcdn.net/100/357/932/themes/725376/assets/slider_3.jpg?1739335696080'
									className='h-full w-full'
								/>
							</SwiperSlide>
							<SwiperSlide>
								<img
									src='https://bizweb.dktcdn.net/100/357/932/themes/725376/assets/slider_5.jpg?1739335696080'
									className='h-full w-full'
								/>
							</SwiperSlide>
							<SwiperSlide>
								<img
									src='https://bizweb.dktcdn.net/100/357/932/themes/725376/assets/slider_1.jpg?1739335696080'
									className='h-full w-full'
								/>
							</SwiperSlide>
							<SwiperSlide>
								<img
									src='https://bizweb.dktcdn.net/100/357/932/themes/725376/assets/slider_2.jpg?1739335696080'
									className='h-full w-full'
								/>
							</SwiperSlide>
						</Swiper>
					</div>
				</main>

				{/* Right Sidebar */}
				<div className='lg:w-1/3'>
					<div className='h-full flex flex-col justify-between pt-4 px-4'>
						<h2 className='mb-4 text-center font-semibold text-sm'>CHẤT LƯỢNG TỐT NHẤT</h2>
						<div className='grid grid-cols-2 gap-4'>
							<ServiceCard icon={<CreditCard className='h-8 w-8' />} title='Thanh toán khi nhận hàng' />
							<ServiceCard icon={<Clock className='h-8 w-8' />} title='Giao hàng trong 24H' />
							<ServiceCard icon={<RefreshCw className='h-8 w-8' />} title='Sản phẩm đổi trả miễn phí' />
							<ServiceCard icon={<ThumbsUp className='h-8 w-8' />} title='Sản phẩm chính hãng' />
						</div>
						<div className='mt-6 flex items-center justify-center gap-2 rounded-lg border p-2'>
							<Phone className='h-5 w-5' />
							<div>
								<div className='text-sm'>Dịch vụ CSKH</div>
								<div className='font-semibold'>Hotline: 0123.456.789</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}

function ServiceCard({ icon, title }) {
	return (
		<div className='flex flex-col items-center text-center'>
			<div className='mb-2 rounded-full bg-gray-100 p-3'>{icon}</div>
			<p className='text-xs'>{title}</p>
		</div>
	);
}
