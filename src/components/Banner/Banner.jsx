import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/css/effect-fade';
import 'swiper/css';
import { Autoplay, EffectFade } from 'swiper/modules';

export default function Banner() {
	return (
		<div className='w-full text-center'>
			<Swiper
				spaceBetween={0}
				effect={'fade'}
				autoplay={{ delay: 2500 }}
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
	);
}
