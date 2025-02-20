import React, { useEffect, useState } from 'react';
// import { Button } from '../../../components/ui/button';
import { GetTheActiveBlindBox } from '../../../api/BlindBox/ApiBlindBox';
import NoThumb from '../../../assets/noThumbnailImage.jpg';
import CountdownHot from '../../../components/Sections/CountdownHot';

const tabs = [
	{ id: 'DỰ ÁN THỊNH HÀNH', label: 'DỰ ÁN THỊNH HÀNH' },
	{ id: 'MỚI RA MẮT', label: 'MỚI RA MẮT' },
	{ id: 'MỞ BÁN ĐỢT 2', label: 'MỞ BÁN ĐỢT 2' },
	{ id: 'SẮP KẾT THÚC', label: 'SẮP KẾT THÚC' },
	{ id: 'SẮP VỀ HÀNG', label: 'SẮP VỀ HÀNG' },
];

const ProductHot = () => {
	const [activeTab, setActiveTab] = useState(tabs[0].id);

	const [data, setData] = useState();
	const productBlind = async () => {
		const res = await GetTheActiveBlindBox();
		setData(res);
	};

	useEffect(() => {
		productBlind();
	}, []);

	const blindList = data?.data;

	return (
		<div className='-mt-14'>
			<div className='sec-com'>
				<div className='container-lg'>
					<div className='flex relative flex-col gap-3 md:gap-6 p-4 rounded-lg'>
						<div className="before:absolute before:top-0 before:left-0 before:content-[''] before:w-full before:h-1/2 before:bg-[#D62727] before:rounded-2xl"></div>

						<div className='flex items-center gap-2 relative z-20'>
							<img
								src='https://theme.hstatic.net/1000069970/1001119059/14/icon-flashsale.png?v=6834'
								alt=''
							/>
							<h2 className='font-bold text-3xl text-white'>Tháng 2 khởi sắc, bứt phá vươn xa!</h2>
						</div>
						<div className='relative z-20'>
							<CountdownHot />
						</div>

						{/* Product Grid */}
						<div className='grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6'>
							{blindList?.map((product) => (
								<div
									key={product.blindBoxId}
									className='bg-white rounded-lg overflow-hidden shadow-lg relative group'
								>
									{/* Pre-order Badge */}
									<div className='absolute top-4 left-4 z-10'>
										<div className='bg-red-500 text-white text-xs font-bold px-2 py-1 rounded'>
											PRE-ORDER
										</div>
									</div>

									{/* Product Image */}
									<div className='relative h-48 w-full'>
										<img
											src={product?.images?.mainImage?.url || NoThumb}
											alt={product.name}
											className='object-cover transition-transform duration-300 group-hover:scale-105 absolute w-full h-full inset-0'
											sizes='(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 25vw'
										/>
									</div>

									{/* Product Info */}
									<div className='p-4 flex flex-col gap-2'>
										<h3 className='font-medium text-sm line-clamp-2'>{product.name}</h3>
										<p className='text-xs text-gray-500'>{product.description}</p>
										<span className='text-xs px-2 py-1 bg-red-500 rounded-md text-white w-fit'>
											{product.size}
										</span>

										{/* Order Progress */}
										<div className='flex justify-between text-sm'>
											<span>
												Số lượng đã đặt: {product.ordered}/{product.total}
											</span>
										</div>
										<div className='w-full bg-gray-200 rounded-full h-2'>
											<div
												className='bg-red-500 h-2 rounded-full'
												style={{
													width: `${(product.ordered / product.total) * 100}%`,
												}}
											/>
										</div>
										<div className='text-sm text-gray-600'>
											{/* {product.endDate.includes('Sản phẩm') ? (
													<span className='text-blue-600'>{product.endDate}</span>
												) : (
													<span>Chiến dịch kết thúc: {product.endDate}</span>
												)} */}
										</div>
									</div>
								</div>
							))}
						</div>
					</div>
				</div>
			</div>
		</div>
	);
};

export default ProductHot;
