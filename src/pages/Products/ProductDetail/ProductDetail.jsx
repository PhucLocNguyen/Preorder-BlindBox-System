import ProductImages from '../../../components/ProductImage/ProductImage';
import CountdownTimer from '../../../components/CountDown/CountDown';
import { useParams } from 'react-router';
import { useEffect, useState } from 'react';
import { GetActivePreorderCampaignBySlug } from '../../../api/Pre_orderCampaign/ApiPre_orderCampaign';
const ProductDetail = () => {
	const params = useParams();
	const { slug } = params;
	console.log('slug', slug);

	const [data, setData] = useState();

	const productDetailBlind = async () => {
		const res = await GetActivePreorderCampaignBySlug(slug);
		setData(res);
	};

	useEffect(() => {
		productDetailBlind();
	}, []);

	console.log('data', data);

	return (
		<div className='sec-com'>
			<div className='container-lg'>
				<div className='relative flex flex-col justify-between gap-6 lg:flex-row'>
					{/* IMG */}
					<div className='top-0 w-full lg:w-1/2 lg:sticky h-max'>
						{data?.blindBox?.images?.galleryImages.length > 0 && (
							<ProductImages items={data?.blindBox?.images?.galleryImages} />
						)}
					</div>
					{/* TEXTS */}
					<div className='flex flex-col w-full gap-3 lg:w-1/2'>
						<div className='w-full p-6 mx-auto bg-white rounded-lg shadow-sm '>
							<div className='inline-block px-4 py-1 mb-6 text-white rounded-full bg-emerald-400'>
								Coming Soon
							</div>

							<h1 className='mb-6 text-3xl font-bold'>{data?.blindBox.name}</h1>

							<div className='flex items-center gap-2 mb-4'>
								<span className='inline-flex items-center gap-2 px-4 py-2 bg-yellow-100 rounded-full'>
									{/* <img src='/calendar.svg' alt='' className='w-4 h-4' /> */}
									<span>Dự kiến ra mắt: 31/01/2025</span>
								</span>
							</div>
							<CountdownTimer />

							<div className='flex items-center gap-2 my-4'>
								<span className='inline-flex items-center gap-2 px-4 py-2 bg-yellow-100 rounded-full'>
									{/* <img src='/target.svg' alt='' className='w-4 h-4' /> */}
									<span>Mục tiêu dự kiến: 120 sản phẩm</span>
								</span>
							</div>

							<div className='mb-8 space-y-6'>
								{/* <h2 className='text-lg font-bold text-gray-800'>TÌM HIỂU THƯƠNG HIỆU LIUP</h2> */}

								<p className='leading-relaxed text-gray-600'>{data?.blindBox.description}</p>

								<div className='space-y-4'>
									<div>
										<h3 className='mb-2 text-gray-700'>Size:</h3>
										<div className='flex gap-2'>
											<span className='px-4 py-2 bg-yellow-100 rounded-full'>
												{data?.blindBox.size}
											</span>
											{/* <span className='px-4 py-2 text-gray-600 rounded-full'>LP2 (Open-Ear)</span> */}
										</div>
									</div>

									{/* <div>
										<h3 className='mb-2 text-gray-700'>Màu sắc:</h3>
										<div className='flex gap-2'>
											<span className='px-4 py-2 bg-yellow-100 rounded-full'>Cam</span>
											<span className='px-4 py-2 text-gray-600 rounded-full'>Xanh dương</span>
										</div>
									</div> */}
								</div>
							</div>

							<button className='w-full py-4 font-medium transition-colors bg-yellow-400 rounded-full hover:bg-yellow-500'>
								ĐĂNG KÝ ĐẶT TRƯỚC
							</button>
						</div>
						{/* <h1 className='text-xl font-medium'>{product.name}</h1>
						<div className='h-[2px] bg-gray-100' />
						{product.price?.price === product.price?.discountedPrice ? (
							<h2 className='text-2xl font-medium'>${product.price?.price?.toLocaleString()}</h2>
						) : (
							<div className='flex items-center gap-2'>
								<div className='flex items-center line-through'>
									<h3 className='text-lg text-gray-500'>{product.price?.price?.toLocaleString()}</h3>
									<h3 className='text-lg text-gray-500'>{product.price?.currency}</h3>
								</div>
								<div className='flex items-center gap-1 text-red-500'>
									<h2 className='text-xl font-medium'>
										{product.price?.discountedPrice?.toLocaleString()}
									</h2>
									<h2 className='text-xl font-medium'>{product.price?.currency}</h2>
								</div>
								<div className='p-1 text-xs font-semibold text-white rounded-md bg-primary'>
									-{product.discount?.value}%
								</div>
							</div>
						)}
						<div className='h-[2px] bg-gray-100' />
						{product.additionalInfoSections?.map((section) => (
							<div className='text-sm bg-white rounded-md shadow-lg' key={section.title}>
								<div className='flex items-center gap-1 p-2 font-medium text-white bg-primary rounded-tl-md rounded-tr-md'>
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
