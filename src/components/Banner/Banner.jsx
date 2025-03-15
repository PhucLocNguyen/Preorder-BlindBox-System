import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/css/effect-fade';
import 'swiper/css';
import { Autoplay, EffectFade } from 'swiper/modules';
import { useState, useEffect } from 'react';
import { GetAllBanners } from '../../api/Banner/Banner';

export default function Banner() {
  const [banners, setBanners] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchBanners = async () => {
      const data = await GetAllBanners();
      if (data.length > 0) {
        setBanners(data);
      } else {
        setError('Lỗi khi lấy dữ liệu banner!');
      }
      setLoading(false);
    };

    fetchBanners();
  }, []);

  if (loading) return <p className='text-center'>Đang tải dữ liệu...</p>;
  if (error) return <p className='text-red-500 text-center'>{error}</p>;

  return (
    <div className='w-full text-center'>
      <Swiper
        spaceBetween={0}
        effect={'fade'}
        autoplay={{ delay: 2500, disableOnInteraction: false }}
        modules={[Autoplay, EffectFade]}
        className='mySwiper w-full h-[600px] md:h-[700px] lg:h-[800px]'
        loop
      >
        {banners.map((banner) => (
          <SwiperSlide key={banner.bannerId} className='flex justify-center items-center'>
            <img
              src={banner.imageUrl}
              alt={banner.title}
              className='w-full h-full object-contain'
            />
          </SwiperSlide>
        ))}
      </Swiper>
    </div>
  );
}
