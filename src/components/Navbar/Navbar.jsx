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

	return (
		<div className='flex flex-col'>
			{/* Top Navigation */}
			<nav className='text-white bg-black'>
				<div className='flex items-center justify-between px-4 py-3 mx-auto container-lg max-w-7xl lg:justify-start lg:space-x-8'>
					{/* Mobile Menu Button */}
					<Sheet>
						<SheetTrigger asChild>
							<Button variant='ghost' size='icon' className='lg:hidden'>
								<Menu className='w-6 h-6 text-white' />
							</Button>
						</SheetTrigger>
						<SheetContent side='left' className='w-[300px] p-0'>
							<div className='p-4 border-b'>
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
											<CollapsibleTrigger className='flex items-center justify-between w-full py-2 text-sm hover:text-gray-500'>
												{category.name}
												{category.hasSubmenu && <ChevronRight className='w-4 h-4' />}
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
					<div className='hidden py-4 lg:flex lg:space-x-8'>
						{mainNavItems.map((item) => (
							<Link key={item} to='#' className='text-sm hover:text-gray-300'>
								{item}
							</Link>
						))}
					</div>
				</div>
			</nav>

			<div className='flex flex-col lg:flex-row container-lg !px-0 md:px-4 !py-4'>
				{/* Main Content */}
				<main className='w-full'>
					<div className='relative w-full text-center'>
						<Swiper
							spaceBetween={0}
							effect={'fade'}
							autoplay={{
								delay: 2500,
							}}
							modules={[Autoplay, EffectFade]}
							className='mySwiper object-contain h-72 lg:h-[500px]'
							loop
						>
							<SwiperSlide>
								<img
									src='https://bizweb.dktcdn.net/100/357/932/themes/725376/assets/slider_3.jpg?1739335696080'
									className='w-full h-full'
								/>
							</SwiperSlide>
							<SwiperSlide>
								<img
									src='https://bizweb.dktcdn.net/100/357/932/themes/725376/assets/slider_5.jpg?1739335696080'
									className='w-full h-full'
								/>
							</SwiperSlide>
							<SwiperSlide>
								<img
									src='https://bizweb.dktcdn.net/100/357/932/themes/725376/assets/slider_1.jpg?1739335696080'
									className='w-full h-full'
								/>
							</SwiperSlide>
							<SwiperSlide>
								<img
									src='https://bizweb.dktcdn.net/100/357/932/themes/725376/assets/slider_2.jpg?1739335696080'
									className='w-full h-full'
								/>
							</SwiperSlide>
						</Swiper>
					</div>
				</main>
			</div>
		</div>
	);
}

function ServiceCard({ icon, title }) {
	return (
		<div className='flex flex-col items-center text-center'>
			<div className='p-3 mb-2 bg-gray-100 rounded-full'>{icon}</div>
			<p className='text-xs'>{title}</p>
		</div>
	);
}
