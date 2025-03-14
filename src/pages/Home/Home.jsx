import CountdownSection from '../../components/Sections/CountdownSection';
import VoucherCampaignSection from '../../components/Sections/VoucherCampaignSection';
import ProductList from './ProductList/ProductList';
import ProductHot from './ProductHot/ProductHot';
import BulkOrder from './BulkOrder/BulkOrder';
import TimePricing from './TimePricing/TimePricing';

function Home() {
	return (
		<div>
			<BulkOrder />
			<TimePricing />

			{/* <ProductList /> */}
			{/* <ProductHot /> */}
			
			<VoucherCampaignSection />
		</div>
	);
}

export default Home;
