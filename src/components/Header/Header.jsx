import { ChevronDown, Search, ShoppingCart, User } from "lucide-react"
import { Link } from "react-router-dom"
import logo from "../../assets/react.svg"
export default function Header() {
    return (
        <header className="sticky top-0 z-50 w-full border-b bg-white">
            <div className="container-lg mx-auto flex h-16 items-center justify-between !p-3">
                <Link to="/" className="flex items-center">
                    <img
                        src={logo}
                        alt="Vaithuhay.com Logo"
                        className="h-10 w-auto"
                    />
                </Link>
                <nav className="hidden md:flex items-center space-x-8">
                    <Link to="/" className="text-sm font-medium hover:text-gray-600 flex items-center gap-2">
                        <p>Trang chủ</p> <ChevronDown className="w-4 h-4" />
                    </Link>
                    <div className="relative group">
                        <Link to="/gioi-thieu" className="text-sm font-medium hover:text-gray-600 flex items-center gap-2">
                            <p>Giới thiệu</p> <ChevronDown className="w-4 h-4" />
                        </Link>
                    </div>
                    <div className="relative group">
                        <Link to="/san-pham" className="text-sm font-medium hover:text-gray-600 flex items-center gap-2">
                            <p>Sản phẩm</p> <ChevronDown className="w-4 h-4" />
                        </Link>
                    </div>
                    <div className="relative group">
                        <Link to="/bai-viet" className="text-sm font-medium hover:text-gray-600 flex items-center gap-2">
                            <p>Bài viết</p> <ChevronDown className="w-4 h-4" />
                        </Link>
                    </div>
                </nav>

                <div className="flex items-center space-x-4">
                    <div className="relative hidden md:block">
                        <input
                            type="search"
                            placeholder="Tìm sản phẩm"
                            className="h-10 w-[300px] rounded-xl bg-yellow-300 px-4 pr-10 text-sm focus:outline-none focus:ring-2 focus:ring-yellow-200"
                        />
                        <button className="absolute right-3 top-1/2 -translate-y-1/2">
                            <Search className="h-4 w-4 text-gray-500" />
                        </button>
                    </div>

                    <button className="relative">
                        <ShoppingCart className="h-6 w-6" />
                        <span className="absolute -right-2 -top-2 flex h-5 w-5 items-center justify-center rounded-full bg-red-500 text-xs text-white">
                            1
                        </span>
                    </button>

                    <button>
                        <User className="h-6 w-6" />
                    </button>
                </div>
            </div>
        </header>
    )
}

