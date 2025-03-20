using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using PreOrderBlindBox.CM.Helpers;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.Repositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.MomoModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.TransactionRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.VnPayModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.PaymentModel;
using PreOrderBlindBox.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Services
{
    public class PaymentService : IPaymentSerivce
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionRepository _transaction;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionService _transactionService;
        public PaymentService(IUserRepository userRepository, IConfiguration configuration, IUnitOfWork unitOfWork, ITransactionRepository transaction, IWalletRepository walletRepository, ITransactionService transactionService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _transaction = transaction;
            _walletRepository = walletRepository;
            _transactionService = transactionService;
        }
        //MOMO payment
        public async Task<string> CreatePaymentInMomoAsync(int userId, decimal amount)
        {
            try
            {
                User user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("Người dùng không tồn tại.");
                }
                if (user.WalletId == null)
                {
                    throw new Exception("Người dùng chưa có ví.");
                }

                Transaction transaction = new Transaction()
                {
                    Money = amount,
                    CreatedDate = DateTime.Now,
                    Description = "Recharge with Momo",
                    Type = TypeOfTransactionEnum.Recharge.ToString(),
                    Status = TransactionStatusEnum.Pending.ToString(),
                    WalletId = user.WalletId,
                };
                _transaction.AddTransaction(transaction);
                // Đọc cấu hình từ _configuration
                string endpoint = _configuration["MomoPayment:BaseUrl"];
                string partnerCode = _configuration["MomoPayment:PartnerCode"];
                string accessKey = _configuration["MomoPayment:AccessKey"];
                string secretKey = _configuration["MomoPayment:SecretKey"];
                string redirectUrl = _configuration["MomoPayment:ReturnUrl"];
                string ipnUrl = _configuration["MomoPayment:NotifyUrl"];
                string requestType = _configuration["MomoPayment:RequestType"];

                // Lấy thông tin user (nếu cần thiết)
                string extraData = $"email={user.Email}";
                string transactionId = transaction.TransactionId.ToString();

                string orderInfo = $"Pay with MoMo_{user.UserId}_{transactionId}";
                string orderId = Guid.NewGuid().ToString();
                string requestId = Guid.NewGuid().ToString();

                // Xây dựng chuỗi rawSignature
                string rawSignature = $"accessKey={accessKey}" +
                                      $"&amount={amount}" +
                                      $"&extraData={extraData}" +
                                      $"&ipnUrl={ipnUrl}" +
                                      $"&orderId={orderId}" +
                                      $"&orderInfo={orderInfo}" +
                                      $"&partnerCode={partnerCode}" +
                                      $"&redirectUrl={redirectUrl}" +
                                      $"&requestId={requestId}" +
                                      $"&requestType={requestType}";

                // Tạo chữ ký (signature)
                Console.WriteLine("Create URL Signature raw:\n" + rawSignature);

                string signature = ComputeHmacSha256(rawSignature, secretKey);

                if (!string.IsNullOrEmpty(signature))
                {
                    // Tạo nội dung body JSON cho request
                    var requestData = new
                    {
                        partnerCode = partnerCode,
                        partnerName = "Test",
                        storeId = "MomoTestStore",
                        requestId = requestId,
                        amount = amount,
                        orderId = orderId,
                        orderInfo = orderInfo,
                        redirectUrl = redirectUrl,
                        ipnUrl = ipnUrl,
                        lang = "vi",
                        extraData = extraData,
                        requestType = requestType,
                        signature = signature
                    };

                    var jsonReq = JsonConvert.SerializeObject(requestData);

                    // Gửi yêu cầu đến MoMo
                    using var client = new HttpClient();
                    var content = new StringContent(jsonReq, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(endpoint, content);

                    // Đọc nội dung phản hồi
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                    // Lấy URL thanh toán
                    string payUrl = jsonResponse?.payUrl;

                    if (!string.IsNullOrEmpty(payUrl))
                    {
                        return payUrl;
                    }
                    else
                    {
                        throw new Exception("payUrl không được trả về từ phản hồi.");
                    }
                }

                throw new Exception("Không thể tạo chữ ký (signature).");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo yêu cầu MoMo: {ex.Message}");
            }
        }


        public bool VerifySignatureFromMomo(MomoReturnModel momoResponse)
        {
            string secretKey = _configuration["MomoPayment:SecretKey"];

            string accessKey = _configuration["MomoPayment:AccessKey"];

            // Lấy thông tin user (nếu cần thiết)
            string rawSignature = $"accessKey={accessKey}" +
                      $"&amount={momoResponse.amount}" +
                      $"&extraData={momoResponse.extraData}" +
                      $"&message={momoResponse.message}" +
                      $"&orderId={momoResponse.orderId}" +
                      $"&orderInfo={momoResponse.orderInfo}" +
                      $"&orderType={momoResponse.orderType}" +
                      $"&partnerCode={momoResponse.partnerCode}" +
                      $"&payType={momoResponse.payType}" +
                      $"&requestId={momoResponse.requestId}" +
                      $"&responseTime={momoResponse.responseTime}" +
                      $"&resultCode={momoResponse.resultCode}" +
                      $"&transId={momoResponse.transId}";

            try
            {
                Console.WriteLine("Verify Signature raw:\n" + rawSignature);
                string hashSignature = ComputeHmacSha256(rawSignature, secretKey);
                if (momoResponse.signature.Equals(hashSignature, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xác thực chữ ký MoMo: {ex.Message}");
            }

        }
        public async Task<string> CreatePaymentInVnPayAsync(int userId, decimal amount)
        {
            try
            {
                // Lấy thông tin user từ repository
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("Người dùng không tồn tại.");
                }
                if (user.WalletId == null)
                {
                    throw new Exception("Người dùng chưa có ví.");
                }
                // Lấy thông tin cấu hình từ _configuration
                string version = _configuration["Vnpay:Version"];
                string command = _configuration["Vnpay:Command"];
                string tmnCode = _configuration["Vnpay:TmnCode"];
                string currCode = _configuration["Vnpay:CurrCode"];
                string vnpayBank = _configuration["Vnpay:BankCode"];
                string locale = _configuration["Vnpay:Locale"];
                string returnUrl = _configuration["Vnpay:ReturnUrl"];
                string baseUrl = _configuration["Vnpay:BaseUrl"];
                string hashSecret = _configuration["Vnpay:HashSecret"];

                // Lấy thời gian hiện tại và IP người dùng
                DateTime dateTime = DateTime.UtcNow.AddHours(7);
                HttpContext context = new HttpContextAccessor().HttpContext;
                var ipAddress = VnpayUtils.GetIpAddress(context);
                var pay = new VnPayLibrary();

                // Tạo thông tin giao dịch
                string orderInfo = $"Thanh toan cho user: {userId}";
                // Đúng: Phải await để nhận về Transaction thật
                var transaction = _transaction.AddTransaction(new Transaction
                {
                    Money = amount,
                    Description = "Recharge with Vnpay",
                    Type = TypeOfTransactionEnum.Recharge.ToString(),
                    Status = TransactionStatusEnum.Pending.ToString(),
                    WalletId = user.WalletId,
                });

                // Lúc này transaction.Id mới chính xác
                string transactionId = transaction.TransactionId.ToString();

                // Thêm dữ liệu vào request VNPay
                pay.AddRequestData("vnp_Version", version);
                pay.AddRequestData("vnp_Command", command);
                pay.AddRequestData("vnp_TmnCode", tmnCode);
                pay.AddRequestData("vnp_Amount", ((int)(amount * 100)).ToString());
                pay.AddRequestData("vnp_BankCode", vnpayBank);
                pay.AddRequestData("vnp_CreateDate", dateTime.ToString("yyyyMMddHHmmss"));
                pay.AddRequestData("vnp_CurrCode", currCode);
                pay.AddRequestData("vnp_IpAddr", ipAddress);
                pay.AddRequestData("vnp_Locale", locale);
                pay.AddRequestData("vnp_OrderInfo", orderInfo);
                pay.AddRequestData("vnp_OrderType", "topup");
                pay.AddRequestData("vnp_TxnRef", transactionId);
                pay.AddRequestData("vnp_ReturnUrl", returnUrl);


                // Tạo URL thanh toán
                string paymentUrl = pay.CreateRequestUrl(baseUrl, hashSecret);

                if (string.IsNullOrEmpty(paymentUrl))
                {
                    throw new Exception("Không thể tạo URL thanh toán VNPay.");
                }

                // Trả về URL thanh toán
                return paymentUrl;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo yêu cầu VNPay: {ex.Message}");
            }
        }

        public async Task<ResponsePaymentResult> DepositConfirmFromVnPay(IQueryCollection request)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in request)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_SecureHash = request.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["Vnpay:HashSecret"]);

            //_cache.Remove(vnpay.GetResponseData("vnp_TxnRef"));
            if (checkSignature)
            {
                var transactionDetail = await _transaction.GetByIdAsync(int.Parse(vnpay.GetResponseData("vnp_TxnRef")));
                var walletDetail = await _walletRepository.GetByIdAsync(transactionDetail.WalletId.Value);
                if (transactionDetail != null)
                {
                    if (transactionDetail.Status.Equals("Success"))
                        return new ResponsePaymentResult()
                        {
                            IsSuccess = true,
                            Message = "Giao dịch đã được xử lý trước đó",
                        };

                    if (vnpay.GetResponseData("vnp_ResponseCode") == "00")
                    {
                        var admin = (await _userRepository.GetAll(filter: x => x.Role.RoleName == "Admin" && x.WalletId != null, includes: [x => x.Role, x => x.Wallet])).FirstOrDefault();
                        Wallet systemWallet = await _walletRepository.GetByIdAsync(admin.WalletId.Value);
                        var systemTransaction = new RequestTransactionCreateModel()
                        {
                            Money = transactionDetail.Money,
                            WalletId = systemWallet.WalletId,
                            Description = "Recharge money to system wallet after customer recharge VNPAY",
                            Type = TypeOfTransactionEnum.Recharge,
                            BalanceAtTime = systemWallet.Balance,
                            Status = TransactionStatusEnum.Success
                        };
                        await _transactionService.CreateTransaction(systemTransaction);

                        transactionDetail.Status = "Success";
                        transactionDetail.BalanceAtTime = walletDetail.Balance;
                        walletDetail.Balance += transactionDetail.Money;
                        await _transaction.UpdateAsync(transactionDetail);
                        await _walletRepository.UpdateAsync(walletDetail);
                        await _unitOfWork.SaveChanges();
                        return new ResponsePaymentResult()
                        {
                            IsSuccess = true,
                            Message = "Giao dịch thành công",
                        };
                    }
                    transactionDetail.Status = "Failed";
                    await _transaction.UpdateAsync(transactionDetail);
                }
                else
                {
                    return new ResponsePaymentResult()
                    {
                        IsSuccess = false,
                        Message = "Có lỗi xảy ra trong quá trình xử lý",
                    };
                }
            }
            await _unitOfWork.SaveChanges();

            return new ResponsePaymentResult()
            {
                IsSuccess = false,
                Message = "Có lỗi xảy ra trong quá trình xử lý",
            };
        }
        public async Task<ResponsePaymentResult> DepositMomoAsync(MomoReturnModel momoResponse)
        {
            try
            {
                string orderInfo = momoResponse.orderInfo;
                int userId = int.Parse(orderInfo.Split("_")[1]);
                User userDetail = await _userRepository.GetByIdAsync(userId);
                int transactionId = int.Parse(orderInfo.Split("_")[2]);
                if (userDetail == null)
                {
                    return new ResponsePaymentResult()
                    {
                        IsSuccess = false,
                        Message = "Không tồn tại người dùng này",
                    };
                }
                if (userDetail.WalletId == null)
                {
                    return new ResponsePaymentResult()
                    {
                        IsSuccess = false,
                        Message = "Ví không hợp lệ để nạp tiền",
                    };
                }

                // validate payment cua momo truoc roi moi add payment
                bool resultValidate = VerifySignatureFromMomo(momoResponse);
                if (!resultValidate)
                {
                    return new ResponsePaymentResult()
                    {
                        IsSuccess = false,
                        Message = "Xác thực không thành công",
                    };
                }
                // Them tien vao wallet
                if (momoResponse.resultCode != 0)
                {
                    throw new Exception("Momo payment failed");
                }
                var transactionDetail = await _transaction.GetByIdAsync(transactionId);
                if (transactionDetail == null)
                {
                    throw new Exception("Transaction does not exist !");
                }
                if (transactionDetail.Status.Equals("Success"))
                {
                    return new ResponsePaymentResult()
                    {
                        IsSuccess = true,
                        Message = "Giao dịch đã được xử lý trước đó",
                    };
                }
                try
                {
                    var admin = (await _userRepository.GetAll(filter: x => x.Role.RoleName == "Admin" && x.WalletId != null, includes: [x => x.Role, x => x.Wallet])).FirstOrDefault();
                    Wallet systemWallet = await _walletRepository.GetByIdAsync(admin.WalletId.Value);
                    var systemTransaction = new RequestTransactionCreateModel()
                    {
                        Money = transactionDetail.Money,
                        WalletId = systemWallet.WalletId,
                        Description = "Recharge money to system wallet after customer recharge Momo",
                        Type = TypeOfTransactionEnum.Recharge,
                        BalanceAtTime = systemWallet.Balance,
                        Status = TransactionStatusEnum.Success
                    };
                    await _transactionService.CreateTransaction(systemTransaction);

                    Wallet wallet = await _walletRepository.GetByIdAsync(userDetail.WalletId);
                    decimal walletBalance = wallet.Balance;
                    wallet.Balance += momoResponse.amount;
                    await _unitOfWork.BeginTransactionAsync();
                    await _walletRepository.UpdateAsync(wallet);
                    Transaction transaction = _transaction.GetById(transactionId);
                    transaction.Status = "Success";
                    transaction.BalanceAtTime = walletBalance;
                    await _unitOfWork.SaveChanges();
                    await _unitOfWork.CommitTransactionAsync();
                    return new ResponsePaymentResult()
                    {
                        IsSuccess = true,
                        Message = "Xác thực Giao dịch thành công",
                    }; ;


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    await _unitOfWork.RollbackTransactionAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            };

            return new ResponsePaymentResult()
            {
                IsSuccess = false,
                Message = "Có lỗi xảy ra trong việc xác thực",
            };

        }
        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }

    }
}