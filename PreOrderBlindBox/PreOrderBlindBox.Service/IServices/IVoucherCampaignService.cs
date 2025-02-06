using PreOrderBlindBox.Services.DTO.RequestDTO.VoucherCampaignModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.VoucherCampaignModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
	public interface IVoucherCampaignService
	{
		public Task<int> CreateVoucherCampaignAsync(RequestCreateVoucherCompaign voucherCompaign);
		public Task<int> UpdateVoucherCampaignAsync(int voucherCampaignId, RequestUpdateVoucherCampaign updateVoucher);
		public Task<int> DeleteVoucherCampaignAsync(int voucherCampaignId);
		public Task ViewVoucherCampaign();
		public Task BackGroundUpdateVoucherCampaign();
		public Task<List<ResponseVoucherCampaign>> GetAllVoucherCampaign();
		public Task<ResponseVoucherCampaign> GetVoucherCampaignById(int voucherCampaignId);
		public Task<List<ResponseVoucherCampaignBaseUser>> GetAllVoucherCampaignBaseCustomer();
	}
}
