import { Button, Form, Input, Radio } from 'antd';

import GoogleIcon from '../../assets/Login/GoogleIcon.png';
import FacebookIcon from '../../assets/Login/FacebookIcon.png';

function LoginPage() {
    const [form] = Form.useForm();

    return (
        <div className="bg-[#f2f7fb] ">
            <div className="w-full max-w-[540px] mx-auto min-h-[100vh] text-center flex items-center justify-center">
                <div className="flex flex-col justify-center grow-1 w-full">
                    <div className="flex flex-col bg-[white] rounded-[20px] p-[30px] text-start gap-[40px]">
                        {/* Phần chữ giới thiệu */}
                        <div>
                            <h3 className="text-[24px] font-bold leading-[37px]">
                                Login to account
                            </h3>
                            <div className="text-[#575864] text-[14px] font-normal leading-[20px]">
                                Enter your email & password to login
                            </div>
                        </div>
                        {/* Phần điền form đăng nhập */}
                        <div className="flex flex-col gap-[24px] relative">
                            <Form layout='vertical' form={form}>
                                <Form.Item
                                    className='font-bold text-[14px] leading-[20px] text-[#111] mb-[10px]'
                                    label="Email address"
                                    name="Email address"
                                    rules={[
                                        {
                                            required: true,
                                            message: 'Please input your email address!',
                                        },
                                    ]}
                                >
                                    <Input className='px-[22px] py-[14px] font-normal' />
                                </Form.Item>

                                <Form.Item
                                    className='font-bold text-[14px] leading-[20px] text-[#111] mb-[10px]'
                                    label="Password"
                                    name="password"
                                    rules={[
                                        {
                                            required: true,
                                            message: 'Please input your password!',
                                        },
                                    ]}
                                >
                                    <Input.Password className='px-[22px] py-[14px] font-normal' />
                                </Form.Item>

                                <Form.Item className='mb-[0] '>
                                    <div className='flex items-center justify-end '>
                                        <p className='text-[14px] leading-[20px] text-[#2275fc] font-normal cursor-pointer'>
                                            Forgot password?
                                        </p>
                                    </div>
                                </Form.Item>

                                <Form.Item>
                                    <Button className='mt-[1rem] h-[50px] py-[15px] px-[22px] flex items-center justify-center text-[14px] font-bold leading-[20px] w-full' type="primary"
                                        htmlType='submit'
                                    >
                                        Login
                                    </Button>
                                </Form.Item>
                            </Form>
                        </div>
                        {/* Phần đăng nhập với google */}
                        <div>
                            <div className='text-[12px] leading-[15px] text-[#95989d] mb-[16px] text-center'>
                                Or continue with social account
                            </div>
                            <div className='flex gap-[16px]'>
                                <div className='cursor-pointer group hover:bg-[#2275fc] text-[#575864] bg-[#fff] border-[#ecf0f4] h-[50px] py-[15px] px-[22px] flex items-center justify-center text-[14px] gap-[8px] font-bold leading-[20px] border-[1px] border-solid rounded-[12px] w-full '>
                                    <img src={GoogleIcon} alt='Google Icon' className='w-[20px] h-[20px]' />
                                    <span className='text-[#111] group-hover:text-[white]'>
                                        Sign in with Google
                                    </span>
                                </div>

                                <div className='cursor-pointer group hover:bg-[#2275fc] text-[#575864] bg-[#fff] border-[#ecf0f4] h-[50px] py-[15px] px-[22px] flex items-center justify-center text-[14px] gap-[8px] font-bold leading-[20px] border-[1px] border-solid rounded-[12px] w-full'>
                                    <img src={FacebookIcon} alt='Facebook Icon' className='w-[25px] h-[25px]' />
                                    <span className='text-[#111] group-hover:text-[white]'>
                                        Sign in with Facebook
                                    </span>
                                </div>
                            </div>
                        </div>
                        {/* Phần để chuyển qua đăng kí nếu chưa có tài khoản */}
                        <div className='text-[#575864] text-[14px] font-normal leading-[20px] text-center'>
                            You don't have an account yet?
                            <a href="" className='text-[#2275fc]'> Register Now</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default LoginPage;