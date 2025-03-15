import { Steps } from 'antd';
import CountdownSection from '../../components/Sections/CountdownSection';
import VoucherCampaignSection from '../../components/Sections/VoucherCampaignSection';
import Banner from '../../components/Banner/Banner';
import ProductList from './ProductList/ProductList';
import ProductHot from './ProductHot/ProductHot';
import CustomerSupport from './CustomerSupport/CustomerSupport';
import Header from '../../components/Header/Header';
const description = 'This is a description.';

function Home() {
	return (
		<div>
			<Banner />
			<ProductList />	
			<ProductHot />
			
			<VoucherCampaignSection />
			<CustomerSupport/>
		</div>
	);
}

export default Home;
