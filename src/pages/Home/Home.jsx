import { Steps } from 'antd';
import CountdownSection from '../../components/Sections/CountdownSection';
import VoucherCampaignSection from '../../components/Sections/VoucherCampaignSection';
import Navbar from '../../components/Navbar/Navbar';
import ProductList from './ProductList/ProductList';
import ProductHot from './ProductHot/ProductHot';
import Header from '../../components/Header/Header';
const description = 'This is a description.';

function Home() {
	return (
		<div>
			<Header />
			<Navbar />
			<ProductList />
			<ProductHot />
			<Steps
				current={1}
				items={[
					{
						title: 'Finished',
						description,
					},
					{
						title: 'In Progress',
						description,
						subTitle: 'Left 00:00:08',
					},
					{
						title: 'Waiting',
						description,
					},
				]}
			/>
			<CountdownSection />
			<VoucherCampaignSection />
		</div>
	);
}

export default Home;
