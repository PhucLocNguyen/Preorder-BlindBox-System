import { Link } from 'react-router-dom';
import logo from '../../assets/Header/logo.png';

export default function Footer() {
    return (
        <footer className='relative w-full bg-[#FFFFFF] text-black py-8 border-t border-black'>
            <div className='relative z-10 container mx-auto px-6'>
                <div className='grid grid-cols-1 md:grid-cols-4 gap-8 text-center md:text-left'>
                    {/* Logo Section */}
                    <div className='space-y-4'>
                        <img src={logo} alt='Logo' className='w-32 h-auto' />
                        <p className='text-gray-700 text-sm'>
                            Preorder Blindbox System - the place for blind box collectors! We offer the opportunity to pre-order the hottest collections at discounted prices and in limited quantities.
                        </p>
                        <p className='text-gray-600 text-sm'>Pre-order today to enjoy the discount</p>
                    </div>

                    {/* Links Section */}
                    <div className='space-y-4'>
                        <h3 className='text-lg font-semibold'>FOLLOW US</h3>
                        <nav className='space-y-2'>
                            {['Facebook', 'Twitter', 'Instagram', 'Tiktok'].map((item) => (
                                <a
                                    key={item}
                                    href='#'
                                    target='_blank'
                                    rel='noopener noreferrer'
                                    className='block text-gray-700 hover:text-black transition-colors'
                                >
                                    {item}
                                </a>
                            ))}
                        </nav>
                    </div>

                    {/* Policies Section */}
                    <div className='space-y-4'>
                        <h3 className='text-lg font-semibold'>CHÍNH SÁCH</h3>
                        <nav className='space-y-2'>
                            {[
                                { name: 'Chính sách bảo hành', url: '#' },
                                { name: 'Chính sách giao hàng', url: '#' },
                                { name: 'Chính sách bảo mật', url: '#' },
                            ].map((item) => (
                                <a
                                    key={item.name}
                                    href={item.url}
                                    className='block text-gray-700 hover:text-black transition-colors'
                                >
                                    {item.name}
                                </a>
                            ))}
                        </nav>
                    </div>

                    {/* About Us Section */}
                    <div className='space-y-4'>
                        <h3 className='text-lg font-semibold'>About Us</h3>
                        <p className='text-gray-700'>
                            Committed to providing an independent shopping experience, each product in the store is carefully selected, ensuring surprise and excitement for customers. Join now to not miss the opportunity to own exclusive items!
                        </p>
                    </div>
                </div>
            </div>
        </footer>
    );
}
