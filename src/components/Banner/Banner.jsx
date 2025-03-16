import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/css/effect-fade';
import 'swiper/css';
import { Autoplay, EffectFade } from 'swiper/modules';
import { useState, useEffect } from 'react';
import { GetAllBanner } from '../../api/Banner/ApiBanner';

export default function Banner() {
  const [banners, setBanners] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchBanners = async () => {
      const { data } = await GetAllBanner();
      setBanners(data);
      setLoading(false);
    };

    fetchBanners();
  }, []);

  if (loading) return <p className='text-center'>Đang tải dữ liệu...</p>;
  if (error) return <p className='text-red-500 text-center'>{error}</p>;

  return (
    <div className='w-full text-center'>
      {banners != null ? <Swiper
        spaceBetween={0}
        effect={'fade'}
        autoplay={{ delay: 2500, disableOnInteraction: false }}
        modules={[Autoplay, EffectFade]}
        className='mySwiper w-full h-[350px] object-cover'
        loop
      >
        {banners.map((banner) => (
          <SwiperSlide key={banner.bannerId} className='flex justify-center items-center'>
            <img
              src={banner.imageUrl}
              alt={banner.title}
              className='w-full h-full object-cover'
            />
          </SwiperSlide>
        ))}
      </Swiper> : null}

    </div>
  );
}