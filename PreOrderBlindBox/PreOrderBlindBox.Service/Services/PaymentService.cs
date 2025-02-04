using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PreOrderBlindBox.CM.Helpers;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.Repositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.MomoModel;
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

        public PaymentService(IUserRepository userRepository, IConfiguration configuration, IUnitOfWork unitOfWork, ITransactionRepository transaction, IWalletRepository walletRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _transaction = transaction;
            _walletRepository = walletRepository;
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
                string orderInfo = $"Pay with MoMo_{user.UserId}_{user.WalletId}";
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


        public Task<bool> VerifySignatureFromMomo(User user, RequestMomoConfirm request)
        {
            string endpoint = _configuration["MomoPayment:BaseUrl"];
            string partnerCode = _configuration["MomoPayment:PartnerCode"];
            string accessKey = _configuration["MomoPayment:AccessKey"];
            string secretKey = _configuration["MomoPayment:SecretKey"];
            string redirectUrl = _configuration["MomoPayment:ReturnUrl"];
            string ipnUrl = _configuration["MomoPayment:NotifyUrl"];
            string requestType = _configuration["MomoPayment:RequestType"];

            // Lấy thông tin user (nếu cần thiết)
            string extraData = $"email={user.Email}";
            string orderInfo = $"Pay with MoMo_{user.UserId}_{user.WalletId}";
            string orderId = request.OrderId;
            string requestId = request.RequestId;

            // Xây dựng chuỗi rawSignature
            string rawSignature = $"accessKey={accessKey}" +
                                  $"&amount={request.Amount}" +
                                  $"&extraData={extraData}" +
                                  $"&ipnUrl={ipnUrl}" +
                                  $"&orderId={orderId}" +
                                  $"&orderInfo={orderInfo}" +
                                  $"&partnerCode={partnerCode}" +
                                  $"&redirectUrl={redirectUrl}" +
                                  $"&requestId={requestId}" +
                                  $"&requestType={requestType}";
            try
            {
                if (request.Signature.Equals(ComputeHmacSha256(rawSignature, secretKey), StringComparison.Ordinal))
                {
                    return Task.FromResult(true);
                }

                return Task.FromResult(true);

            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xác thực chữ ký MoMo: {ex.Message}");
            }
            return Task.FromResult(false);

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
                    Description = "Deposit",
                    Status = "Pending",
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

        public async Task<ResponsePaymentResult> VerifySignatureFromVnPay(IQueryCollection request)
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
            var transactionDetail = await _transaction.GetByIdAsync(int.Parse(vnpay.GetResponseData("vnp_TxnRef")));
            var walletDetail = await _walletRepository.GetByIdAsync(transactionDetail.WalletId.Value);
            //_cache.Remove(vnpay.GetResponseData("vnp_TxnRef"));
            if (checkSignature)
            {
                if (transactionDetail != null)
                {
                    if (vnpay.GetResponseData("vnp_ResponseCode") == "00")
                    {
                        transactionDetail.Status = "Completed";
                        transactionDetail.BalanceAtTime = walletDetail.Balance;
                        walletDetail.Balance += transactionDetail.Money;
                        await _transaction.UpdateAsync(transactionDetail);
                        await _walletRepository.UpdateAsync(walletDetail);
                        await _unitOfWork.SaveChanges();
                        return new ResponsePaymentResult()
                        {
                            ResponseCode = vnpay.GetResponseData("vnp_ResponseCode"),
                            Message = "Confirm Success",
                        };
                    }
                    transactionDetail.Status = "Failed";
                    await _transaction.UpdateAsync(transactionDetail);
                }
                else
                {
                    return new ResponsePaymentResult()
                    {
                        ResponseCode = vnpay.GetResponseData("vnp_ResponseCode"),
                        Message = "Có lỗi xảy ra trong quá trình xử lý",
                    };
                }
            }
            await _unitOfWork.SaveChanges();

            return new ResponsePaymentResult()
            {
                ResponseCode = vnpay.GetResponseData("vnp_ResponseCode"),
                Message = "Có lỗi xảy ra trong quá trình xử lý",
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

        //public string RequestVNPay(int transactionId, decimal price, HttpContext context)
        //{
        //    IConfiguration _configuration = new ConfigurationBuilder()
        //            .SetBasePath(Directory.GetCurrentDirectory())
        //            .AddJsonFile("appsettings.json")
        //            .Build();

        //    DateTime dateTime = DateTime.UtcNow.AddHours(7);
        //var pay = new VnPayLibrary();
        //    var ipAddress = pay.GetIpAddress(context);


        //pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
        //    pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
        //    pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
        //    pay.AddRequestData("vnp_Amount", (price* 100).ToString());
        //    pay.AddRequestData("vnp_CreateDate", dateTime.ToString("yyyyMMddHHmmss"));
        //    pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
        //    pay.AddRequestData("vnp_IpAddr", ipAddress);
        //    pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
        //    pay.AddRequestData("vnp_OrderInfo", "Thanh toan cho vi: " + transactionId);
        //    pay.AddRequestData("vnp_OrderType", "250000");
        //    pay.AddRequestData("vnp_TxnRef", transactionId.ToString());
        //    pay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:UrlReturn"]);

        //    var paymentUrl =
        //        pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

        //    return paymentUrl;
        //}


        //public async Task<bool> ReturnFromVNPay(RequestVnPayCreate vnPayResponse)
        //{
        //    IConfiguration _configuration = new ConfigurationBuilder()
        //                .SetBasePath(Directory.GetCurrentDirectory())
        //                .AddJsonFile("appsettings.json")
        //                .Build();

        //    if (vnPayResponse != null)
        //    {
        //        var vnpay = new VnPayLibrary();

        //        foreach (PropertyInfo prop in vnPayResponse.GetType().GetProperties())
        //        {
        //            string name = prop.Name;
        //            object value = prop.GetValue(vnPayResponse, null);
        //            string valueStr = value?.ToString() ?? string.Empty;
        //            vnpay.AddResponseData(name, valueStr);
        //        }

        //        var vnpayHashSecret = _configuration["Vnpay:HashSecret"];
        //        bool validateSignature = vnpay.ValidateSignature(vnPayResponse.vnp_SecureHash, vnpayHashSecret);

        //        if (validateSignature)
        //        {
        //            int id = 0;
        //            _ = int.TryParse(vnPayResponse.vnp_TxnRef, out id);

        //            var transaction = await _transactionRepository.GetByIdAsync(id);

        //            if (transaction != null)
        //            {
        //                var wallet = await _walletRepository.GetByIdAsync(transaction.WalletId.Value);

        //                if (wallet != null)
        //                {
        //                    // update transaction thành công
        //                    transaction.Status = true;

        //                    await _transactionRepository.UpdateAsync(transaction);

        //                    // update tiền vào ví của người dùng
        //                    wallet.Currency += transaction.Money;
        //                    await _walletRepository.UpdateAsync(wallet);

        //                    var result = await _unitOfWork.SaveChanges();
        //                    if (result > 0)
        //                    {
        //                        return new TransactionModel
        //                        {
        //                            Money = transaction.Money,
        //                            Description = transaction.Description,
        //                            Status = transaction.Status,
        //                            Walletid = transaction.WalletId,
        //                        };
        //                    }
        //                    else
        //                    {
        //                        throw new Exception("Có lỗi trong quá trình cập nhật dữ liệu");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception("Ví của user không tồn tại");
        //                }
        //            }
        //            else
        //            {
        //                throw new Exception("Yêu cầu không tồn tại");
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("Không đúng signature");
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Có lỗi trong quá trình return");
        //    }

        //}
    }
}