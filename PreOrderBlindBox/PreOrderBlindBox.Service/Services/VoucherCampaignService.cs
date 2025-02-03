using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.VoucherCampaignModel;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.Service.Services
{
	public class VoucherCampaignService : IVoucherCampaignService
	{
		private readonly IVoucherCampaignRepository _voucherCampaignRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public VoucherCampaignService(IVoucherCampaignRepository voucherCampaignRepository, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_voucherCampaignRepository = voucherCampaignRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<int> CreateVoucherCampaignAsync(RequestCreateVoucherCompaign voucherCompaign)
		{
			if (voucherCompaign == null)
			{
				throw new ArgumentNullException("Invalid voucher campaign");
			}
			if (voucherCompaign.StartDate < DateTime.Now || voucherCompaign.EndDate < DateTime.Now)
			{
				throw new ArgumentException("Start date and end date must be in the future");
			}
			if (voucherCompaign.StartDate >= voucherCompaign.EndDate)
			{
				throw new ArgumentException("End date must be later than start date.");
			}
			if (voucherCompaign.StartDate.AddDays(1) > voucherCompaign.EndDate)
			{
				throw new ArgumentException("End date must be at least 1 day after start date");
			}
			if (voucherCompaign.MaximumUserCanGet >= voucherCompaign.Quantity)
			{
				throw new ArgumentException("Maximum user can get must be smaller than quantity");
			}

			VoucherCampaign newVoucherCampaign = _mapper.Map<VoucherCampaign>(voucherCompaign);
			newVoucherCampaign.CreatedDate = DateTime.Now;
			newVoucherCampaign.IsDeleted = false;

			if (newVoucherCampaign.StartDate > DateTime.Now)
			{
				newVoucherCampaign.Status = VoucherCampaignEnum.Pending.ToString();
			}
			else if (newVoucherCampaign.StartDate < DateTime.Now)
			{
				newVoucherCampaign.Status = VoucherCampaignEnum.Active.ToString();
			}
			else if (newVoucherCampaign.EndDate < DateTime.Now)
			{
				newVoucherCampaign.Status = VoucherCampaignEnum.Expired.ToString();
			}

			await _voucherCampaignRepository.InsertAsync(newVoucherCampaign);

			return await _unitOfWork.SaveChanges();
		}

		public Task DeleteVoucherCampaignAsync()
		{
			throw new NotImplementedException();
		}

		public Task UpdateVoucherCampaignAsync()
		{
			throw new NotImplementedException();
		}

		public Task ViewVoucherCampaign()
		{
			throw new NotImplementedException();
		}

		public async Task BackGroundUpdateVoucherCampaign()
		{
			try
			{
				var listVoucherCampaign = await _voucherCampaignRepository.GetAllVoucherCampaign();
				var updateListVoucherCampaign = new List<VoucherCampaign>();

				foreach (var voucherCampaign in listVoucherCampaign)
				{
					if (voucherCampaign.StartDate <= DateTime.Now && DateTime.Now <= voucherCampaign.EndDate)
					{
						voucherCampaign.Status = VoucherCampaignEnum.Active.ToString();
						updateListVoucherCampaign.Add(voucherCampaign);
					}
					else if (voucherCampaign.StartDate <= DateTime.Now && voucherCampaign.EndDate <= DateTime.Now)
					{
						voucherCampaign.Status = VoucherCampaignEnum.Expired.ToString();
						updateListVoucherCampaign.Add(voucherCampaign);
					}
				}

				if (updateListVoucherCampaign.Any())
				{
					_voucherCampaignRepository.UpdateRangeAsync(listVoucherCampaign);
					await _unitOfWork.SaveChanges();
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error updating voucher campaigns: {ex.Message}");
			}
		}

	}
}
