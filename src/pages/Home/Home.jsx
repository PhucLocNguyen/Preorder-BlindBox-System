import CountdownSection from '../../components/Sections/CountdownSection';
import VoucherCampaignSection from '../../components/Sections/VoucherCampaignSection';
import ProductList from './ProductList/ProductList';
import ProductHot from './ProductHot/ProductHot';

function Home() {
	return (
		<div>
			<ProductList />
			<ProductHot />	
			<CountdownSection />
			<VoucherCampaignSection />
		</div>
	);
}

export default Home;
