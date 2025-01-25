using Azure.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Services
{
    public class PaymentService : IPaymentSerivce
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public PaymentService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        //MOMO payment
        public async Task<string> CreatePaymentAsync(int userId, decimal amount)
        {
            try
            {
                // Đọc cấu hình từ _configuration
                string endpoint = _configuration["MomoPayment:BaseUrl"];
                string partnerCode = _configuration["MomoPayment:PartnerCode"];
                string accessKey = _configuration["MomoPayment:AccessKey"];
                string secretKey = _configuration["MomoPayment:SecretKey"];
                string redirectUrl = _configuration["MomoPayment:ReturnUrl"];
                string ipnUrl = _configuration["MomoPayment:NotifyUrl"];
                string requestType = _configuration["MomoPayment:RequestType"];

                // Lấy thông tin user (nếu cần thiết)
                User user = await _userRepository.GetByIdAsync(userId);
                string extraData = $"email={user.Email}";
                string orderInfo = "Pay with MoMo";
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