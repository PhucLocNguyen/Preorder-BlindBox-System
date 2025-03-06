/* eslint-disable react/prop-types */

const PreorderMilestones = ({ milestones, placedOrderCount }) => {
	if (!milestones || milestones.length === 0) return null;

	// Sort milestones by milestoneNumber to ensure proper order
	const sortedMilestones = [...milestones].sort((a, b) => a.milestoneNumber - b.milestoneNumber);

	// Calculate original price (using the highest price as the reference price)
	const originalPrice = 10500000; // This should ideally come from your data

	return (
		<div className='flex flex-wrap justify-between gap-4 my-6'>
			{sortedMilestones.map((milestone) => {
				// Convert price from USD to VND (assuming price is in USD)
				const priceInVND = milestone.price * 350000;

				// Calculate discount percentage
				const discountPercentage = Math.round(((originalPrice - priceInVND) / originalPrice) * 100);

				// Determine if this milestone is active
				// const isActive = placedOrderCount >= milestone.quantity;
				const remainingItems = milestone.quantity - placedOrderCount;

				return (
					<div
						key={milestone.preorderMilestoneId}
						className='w-full sm:w-[calc(33%-0.75rem)] rounded-lg overflow-hidden shadow-md bg-white'
					>
						<div className='relative flex items-center justify-between'>
							<div className={`w-full p-3 ${discountPercentage > 50 ? 'bg-yellow-200' : 'bg-gray-300'}`}>
								<span className='text-sm font-medium'>
									Có {milestone.quantity} <br /> sản phẩm được giảm
								</span>
							</div>
							<div
								className={`py-1 px-2 text-white font-bold absolute top-0 right-0 ${
									discountPercentage < 10
										? 'bg-gray-500'
										: discountPercentage < 50
										? 'bg-gray-600'
										: 'bg-red-600'
								}`}
							>
								-{discountPercentage}%
							</div>
						</div>

						<div className='p-3'>
							<div className='text-sm text-gray-500'>Giá niêm yết</div>
							<div className='text-sm text-gray-500 line-through'>{originalPrice.toLocaleString()}đ</div>
							<div
								className={`mt-1 text-2xl font-bold ${
									discountPercentage > 50 ? 'text-red-600' : 'text-gray-300'
								} `}
							>
								{priceInVND.toLocaleString()}đ
							</div>

							<div className='mt-2 overflow-hidden bg-gray-200 rounded-full'>
								<div className='flex items-center'>
									<div
										className={`px-2 py-1 text-xs ${
											discountPercentage > 50
												? 'text-white bg-orange-500'
												: 'text-black bg-gray-300'
										} `}
									>
										<span className='relative z-10'>
											Còn {remainingItems > 0 ? remainingItems : milestone.quantity} sản phẩm
										</span>
									</div>
								</div>
							</div>
						</div>
					</div>
				);
			})}
		</div>
	);
};

export default PreorderMilestones;