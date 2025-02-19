import { ChevronDown, Search, ShoppingCart, User } from 'lucide-react';
import { Link } from 'react-router-dom';
import logo from '../../assets/react.svg';
export default function Header() {
	return (
		<header className='sticky top-0 z-50 w-full bg-white border-b'>
			<div className='container-lg mx-auto flex h-16 items-center justify-between !p-3'>
				<Link to='/' className='flex items-center'>
					<img src={logo} alt='Vaithuhay.com Logo' className='w-auto h-10' />
				</Link>
				<nav className='items-center hidden space-x-8 md:flex'>
					<Link to='/' className='flex items-center gap-2 text-sm font-medium hover:text-gray-600'>
						<p>Trang chủ</p> <ChevronDown className='w-4 h-4' />
					</Link>
					<div className='relative group'>
						<Link
							to='/kham-pha'
							className='flex items-center gap-2 text-sm font-medium hover:text-gray-600'
						>
							<p>Khám phá</p> <ChevronDown className='w-4 h-4' />
						</Link>
					</div>
					<div className='relative group'>
						<Link
							to='/tim-hieu-them'
							className='flex items-center gap-2 text-sm font-medium hover:text-gray-600'
						>
							<p>Tìm hiểu thêm</p> <ChevronDown className='w-4 h-4' />
						</Link>
					</div>
					<div className='relative group'>
						<Link
							to='/bai-viet'
							className='flex items-center gap-2 text-sm font-medium hover:text-gray-600'
						>
							<p>Bài viết</p> <ChevronDown className='w-4 h-4' />
						</Link>
					</div>
				</nav>

				<div className='flex items-center space-x-4'>
					<div className='relative hidden md:block'>
						<input
							type='search'
							placeholder='Tìm sản phẩm'
							className='h-10 w-[300px] rounded-xl bg-yellow-300 px-4 pr-10 text-sm focus:outline-none focus:ring-2 focus:ring-yellow-200'
						/>
						<button className='absolute -translate-y-1/2 right-3 top-1/2'>
							<Search className='w-4 h-4 text-gray-500' />
						</button>
					</div>

					<button className='relative'>
						<ShoppingCart className='w-6 h-6' />
						<span className='absolute flex items-center justify-center w-5 h-5 text-xs text-white bg-red-500 rounded-full -right-2 -top-2'>
							1
						</span>
					</button>

					<button>
						<User className='w-6 h-6' />
					</button>
				</div>
			</div>
		</header>
	);
}
