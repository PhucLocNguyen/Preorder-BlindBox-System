import React, { useEffect, useState } from 'react';
import { GetSimilarCampaigns } from '../../api/SimilarCampaign/ApiSimilarCampaign';

const SimilarCampaign = () => {
  const [campaigns, setCampaigns] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchSimilarCampaigns = async () => {
      try {
        const data = await GetSimilarCampaigns(1); // Replace with dynamic ID if needed
        if (data.length > 0) {
          setCampaigns(data);
        } else {
          setError('No similar campaigns found.');
        }
      } catch (err) {
        setError('Error fetching similar campaigns!');
        console.error(err); // Log error for debugging
      } finally {
        setLoading(false);
      }
    };

    fetchSimilarCampaigns();
  }, []);

  const formatPrice = (price) => {
    return price ? price.toLocaleString() : '0';
  };

  if (loading) return <p className="text-center">Đang tải dữ liệu...</p>;
  if (error) return <p className="text-red-500 text-center">{error}</p>;

  return (
    <div className="p-6 bg-yellow-100">
      <h2 className="text-2xl font-bold mb-4">Chiến Dịch Tương Tự</h2>

      <div className="flex gap-6 overflow-x-auto whitespace-nowrap pb-4">
        {campaigns.map((item) => (
          <div key={item.id} className="bg-white rounded-lg shadow-lg overflow-hidden flex-shrink-0 w-72">
            <img
              src={item.blindBox.images.mainImage.url}
              alt={item.blindBox.name}
              className="w-full h-48 object-cover"
            />
            <div className="p-4">
              <h3 className="font-semibold text-lg truncate">{item.blindBox.name}</h3>
              <p className="text-sm text-gray-600 truncate">{item.blindBox.description}</p>

              <div className="flex items-center justify-between mt-3">
                <div>
                  {item.blindBox.listedPrice && (
                    <span className="text-gray-500 line-through text-sm">
                      {formatPrice(item.blindBox.listedPrice)}đ
                    </span>
                  )}
                  <div className="mt-1">
                    <span className="text-red-600 font-bold text-lg">
                      {formatPrice(item.priceFrom)}đ - {formatPrice(item.priceTo)}đ
                    </span>
                  </div>
                </div>
              </div>

              <div className="mt-4">
                {item.status === 'coming_soon' ? (
                  <span className="bg-green-500 text-white text-xs px-3 py-1 rounded-full">COMING SOON</span>
                ) : item.status === 'preorder' ? (
                  <button className="w-full bg-red-600 text-white font-bold py-2 rounded-lg">
                    ĐĂNG KÝ ĐẶT TRƯỚC
                  </button>
                ) : (
                  <button className="w-full bg-red-600 text-white font-bold py-2 rounded-lg">
                    THAM KHẢO
                  </button>
                )}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default SimilarCampaign;
