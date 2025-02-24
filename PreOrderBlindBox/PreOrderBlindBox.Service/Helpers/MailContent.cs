using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Helpers
{
	public class MailContent
	{
		public static string ConfirmAccountEmail(string username, string tokenConfirm)
		{
			return "<div style=\"background-color:#f8f8f8;font-family:sans-serif;padding:15px\">\n"
				+ "        <div style=\"max-width:1000px ; margin:auto\">\n"
				+ "            <div class=\"adM\">\n"
				+ "                <div style=\"background-color:#fff;padding:5px 20px;color:#000;border-radius:0px 0px 2px 2px\">\n"
				+ "                    <div style=\"padding:35px 15px\">\n"
				+ "                        <p style=\"margin:0;font-size:16px\">\n"
				+ "                            <b>Hello, " + username + " </b>\n"
				+ "                        </p>\n"
				+ "                        <br>\n"
				+ "                        <p style=\"margin:0;font-size:16px\">\n"
				+ "                            You have just registered an account, please click the button below to confirm your account at\n"
				+ "                            <a style=\"text-decoration:none\\\\\" href=\"Pre-order Blind Box\\\\\"\n"
				+ "                                target=\"_blank\">Pre-order Blind Box</a>\n"
+ "                                          </p>\n"
				+ "                        <div style=\"padding:40px;margin:auto;text-align:center\">\n"
				+ "                            <a href=\"http://localhost:5173/confirmemail?token=" + tokenConfirm + "\"\n"
				+ "                                style=\"color: #3cc892; text-decoration: none;\">\n"
				+ "                                <div\n"
				+ "                                    style=\"width:fit-content;border:#3cc892 thin solid;color:#3cc892;font-weight:bold;text-align:center;padding:7px 12px;border-radius:2px;margin:auto;font-size:large\">\n"
				+ "                                    CONFIRM\n"
				+ "                                </div>\n"
				+ "                            </a>\n"
				+ "                        </div>\n"
				+ "                        <div style=\"border-top:1px solid #dcdbdb\"></div>\n"
				+ "                        <br>\n"
				+ "                        <p style=\"margin:0;font-size:16px\">Best Regards,</p>\n"
				+ "                        <p style=\"margin:0;font-size:16px\">Pre-order Blind Box</p>\n"
				+ "                    </div>\n"
				+ "                </div>\n"
				+ "            </div>\n"
				+ "        </div>\n"
				+ "    </div>";
		}

		public static string ForgotPasswordEmail(string userName, string forgotPasswordToken, string email)
		{
			return "<div style=\"max-width: 650px; margin: 0px auto;\">\r\n" +
				"      <table style=\"width: 100%;\">\r\n" +
				"         <tbody>\r\n" +
				"            <tr>\r\n" +
				"               <td style=\"border: 1px solid #ff6e40; border-collapse: collapse;\">\r\n" +
				"                  <table style=\"width: 100%;\">\r\n" +
				"                     <tbody>\r\n" +
				"                        <tr>\r\n" +
				"                           <td\r\n" +
				"                              style=\"padding: 18px 20px 20px 20px; vertical-align: middle; line-height: 20px; font-family: Arial; background-color: #ff6e40; text-align: center;\">\r\n" +
				"                              <span style=\"color: #fff; font-size: 115%; text-transform: uppercase;\">\r\n" +
				"                                 Xác thực yêu cầu lấy lại mật khẩu đăng nhập\r\n" +
				"                              </span>\r\n" +
				"                           </td>\r\n" +
				"                        </tr>\r\n" +
				"                        <tr>\r\n" +
				"                           <td style=\"padding: 20px 20px 12px 20px;\">\r\n" +
				"                              <span style=\"font-size: 13px; color: #252525; font-family: Arial, Helvetica, sans-serif\">\r\n" +
				"                                 Chào " + userName + ",\r\n" +
				"                              </span>\r\n" +
				"                           </td>\r\n" +
				"                        </tr>\r\n" +
				"                        <tr>\r\n" +
				"                           <td style=\"padding: 4px 20px 12px 20px;\">\r\n" +
				"                              <span\r\n" +
				"                                 style=\"font-size: 13px; color: #252525; font-family: Arial, Helvetica, sans-serif; line-height: 18px;\">\r\n" +
				"                                 Bạn đang yêu cầu lấy lại mật khẩu đăng nhập tài khoản Pre-Order Blindbox. Để hoàn\r\n" +
				"                                 tất quy trình, xin vui lòng bấm vào đường link dưới đây hoặc copy vào thanh địa chỉ\r\n" +
				"                                 trình duyệt và nhấn Enter\r\n" +
				"                              </span>\r\n" +
				"                           </td>\r\n" +
				"                        </tr>\r\n" +
				"                        <tr>\r\n" +
				"                           <td style=\"padding: 10px 20px 12px 20px;\">\r\n" +
				"                              <div\r\n" +
				"                                 style=\"background: rgb(255, 248, 204); border: 1px solid rgb(255, 140, 0); padding: 10px; border-radius: 3px 3px 0px 0px; font-size: 13px; font-family: 'Courier New', Courier, monospace; word-wrap: break-word;\">\r\n" +
				"                                 <a href=\"http://localhost:5173/user-forget-password?email=" + email + "&token=" + forgotPasswordToken + "\">Xác nhận thay đổi mật khẩu</a>\r\n" +
				"                              </div>\r\n" +
				"                              <div\r\n" +
				"                                 style=\"background-color: rgb(255, 140, 0); color: #fff; font-family: Arial, Helvetica, sans-serif; font-size: 13px; padding: 8px 20px; border-radius: 0px 0px 3px 3px;\">\r\n" +
				"                                 Link hết hạn lúc: 05:34:35 25/02/2025.\r\n" +
				"                              </div>\r\n" +
				"                           </td>\r\n" +
				"                        </tr>\r\n" +
				"                     </tbody>\r\n" +
				"                  </table>\r\n" +
				"               </td>\r\n" +
				"            </tr>\r\n" +
				"         </tbody>\r\n" +
				"      </table>\r\n" +
				"   </div>";
		}
	}
}
