import React, { useEffect, useState } from 'react';
// import { Button } from '../../../components/ui/button';
import { GetTheActiveBlindBox } from '../../../api/BlindBox/ApiBlindBox';
import NoThumb from '../../../assets/noThumbnailImage.jpg';
import { motion } from 'framer-motion';
import { Link } from 'react-router';

const tabs = [
	{ id: 'DỰ ÁN THỊNH HÀNH', label: 'DỰ ÁN THỊNH HÀNH' },
	{ id: 'MỚI RA MẮT', label: 'MỚI RA MẮT' },
	{ id: 'MỞ BÁN ĐỢT 2', label: 'MỞ BÁN ĐỢT 2' },
	{ id: 'SẮP KẾT THÚC', label: 'SẮP KẾT THÚC' },
	{ id: 'SẮP VỀ HÀNG', label: 'SẮP VỀ HÀNG' },
];

const ProductList = ({ title = 'ĐẶT HÀNG VỀ TAY SỚM NHẤT' }) => {
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
		<div className='relative'>
			<div className="bg-[url('//theme.hstatic.net/1000069970/1001119059/14/preorder-bg.png?v=6834')] bg-no-repeat bg-cover pb-64">
				<div className='sec-com'>
					<div className='container-lg'>
						<div className='flex flex-col gap-3 md:gap-6'>
							<h2 className='font-bold text-center text-3xl'>{title}</h2>
							<div className='flex space-x-1 justify-center bg-white w-fit mx-auto rounded-full p-1'>
								{tabs.map((tab) => (
									<button
										key={tab.id}
										onClick={() => setActiveTab(tab.id)}
										className={`${
											activeTab === tab.id ? '' : 'hover:bg-yellow-300'
										} relative rounded-full px-3 py-1.5 text-sm font-medium text-black outline-sky-400 transition focus-visible:outline-2`}
										style={{
											WebkitTapHighlightColor: 'transparent',
										}}
									>
										{activeTab === tab.id && (
											<motion.span
												layoutId='bubble'
												className='absolute inset-0 z-10 bg-yellow-400'
												style={{ borderRadius: 9999 }}
												transition={{ type: 'spring', bounce: 0.2, duration: 1 }}
											/>
										)}
										<span
											className={`${
												activeTab === tab.id ? 'text-black' : 'text-gray-500'
											} relative z-20`}
										>
											{tab.label}
										</span>
									</button>
								))}
							</div>

							{/* Product Grid */}
							<div className='grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6'>
								{blindList?.map((product) => (
									<Link
										to={`/product/${product.blindBoxId}`}
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
									</Link>
								))}
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
};

export default ProductList;
