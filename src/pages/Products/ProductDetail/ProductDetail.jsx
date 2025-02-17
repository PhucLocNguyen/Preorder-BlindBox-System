import React from 'react';
import { fakeProduct } from '../../../../fakeData';
import ProductImages from '../../../components/ProductImage/ProductImage';
import CountdownTimer from '../../../components/CountDown/CountDown';

const ProductDetail = () => {
	const product = fakeProduct;

	const addAnchorStyles = (htmlContent) => {
		return htmlContent.replace(/<a([^>]+)>/g, (match) => {
			return match.replace('<a', '<a class="text-blue-500"');
		});
	};
	return (
		<div className='sec-com'>
			<div className='container-lg'>
				<div className='relative flex flex-col lg:flex-row justify-between gap-6'>
					{/* IMG */}
					<div className='w-full lg:w-1/2 lg:sticky top-0 h-max'>
						<ProductImages items={product.media?.items} />
					</div>
					{/* TEXTS */}
					<div className='w-full lg:w-1/2 flex flex-col gap-3'>
						<div className='max-w-2xl mx-auto bg-white rounded-lg shadow-sm p-6'>
							<div className='inline-block px-4 py-1 rounded-full bg-emerald-400 text-white mb-6'>
								Coming Soon
							</div>

							<h1 className='text-3xl font-bold mb-6'>
								Tai Nghe Bluetooth LIUP LP1 & LP2 Luminous Quicksand - Phát Sáng Trong Bóng Tối
							</h1>

							<div className='flex items-center gap-2 mb-4'>
								<span className='inline-flex items-center gap-2 px-4 py-2 rounded-full bg-yellow-100'>
									<img src='/calendar.svg' alt='' className='w-4 h-4' />
									<span>Dự kiến ra mắt: 31/01/2025</span>
								</span>
							</div>

							<CountdownTimer />

							<div className='flex items-center gap-2 my-4'>
								<span className='inline-flex items-center gap-2 px-4 py-2 rounded-full bg-yellow-100'>
									<img src='/target.svg' alt='' className='w-4 h-4' />
									<span>Mục tiêu dự kiến: 120 sản phẩm</span>
								</span>
							</div>

							<div className='space-y-6 mb-8'>
								<h2 className='text-lg font-bold text-gray-800'>TÌM HIỂU THƯƠNG HIỆU LIUP</h2>

								<p className='text-gray-600 leading-relaxed'>
									LIUP là thương hiệu chuyên về các sản phẩm âm thanh, đặc biệt là tai nghe không dây,
									kết hợp giữa chất lượng âm thanh cao cấp và thiết kế thẩm mỹ đặc biệt. Nổi bật với
									các tính năng phát sáng và ánh sáng lấp lánh.
								</p>

								<div className='space-y-4'>
									<div>
										<h3 className='text-gray-700 mb-2'>Phiên bản:</h3>
										<div className='flex gap-2'>
											<span className='px-4 py-2 rounded-full bg-yellow-100'>LP1 (In-Ear)</span>
											<span className='px-4 py-2 rounded-full text-gray-600'>LP2 (Open-Ear)</span>
										</div>
									</div>

									<div>
										<h3 className='text-gray-700 mb-2'>Màu sắc:</h3>
										<div className='flex gap-2'>
											<span className='px-4 py-2 rounded-full bg-yellow-100'>Cam</span>
											<span className='px-4 py-2 rounded-full text-gray-600'>Xanh dương</span>
										</div>
									</div>
								</div>
							</div>

							<button className='w-full py-4 bg-yellow-400 rounded-full font-medium hover:bg-yellow-500 transition-colors'>
								ĐĂNG KÝ ĐẶT TRƯỚC
							</button>
						</div>
						{/* <h1 className='text-xl font-medium'>{product.name}</h1>
						<div className='h-[2px] bg-gray-100' />
						{product.price?.price === product.price?.discountedPrice ? (
							<h2 className='font-medium text-2xl'>${product.price?.price?.toLocaleString()}</h2>
						) : (
							<div className='flex items-center gap-2'>
								<div className='flex items-center line-through'>
									<h3 className='text-lg text-gray-500'>{product.price?.price?.toLocaleString()}</h3>
									<h3 className='text-lg text-gray-500'>{product.price?.currency}</h3>
								</div>
								<div className='flex items-center gap-1 text-red-500'>
									<h2 className='font-medium text-xl'>
										{product.price?.discountedPrice?.toLocaleString()}
									</h2>
									<h2 className='font-medium text-xl'>{product.price?.currency}</h2>
								</div>
								<div className='p-1 bg-primary rounded-md text-white font-semibold text-xs'>
									-{product.discount?.value}%
								</div>
							</div>
						)}
						<div className='h-[2px] bg-gray-100' />
						{product.additionalInfoSections?.map((section) => (
							<div className='text-sm rounded-md shadow-lg bg-white' key={section.title}>
								<div className='font-medium flex items-center gap-1 p-2 bg-primary text-white rounded-tl-md rounded-tr-md'>
									{section.title === 'Điều kiện bảo hành' && <ShieldCheckIcon />}
									{section.title === 'Khuyến mãi' && <GiftIcon />}
									<span>{section.title}</span>
								</div>
								<div
									dangerouslySetInnerHTML={{
										__html: addAnchorStyles(section.description ?? ''),
									}}
									className='p-3'
								/>
							</div>
						))} */}
					</div>
				</div>
			</div>
		</div>
	);
};

export default ProductDetail;
