using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PreOrderBlindBox.Service.Services;
using PreOrderBlindBox.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Helpers
{
	public class VoucherCampaignBackgroundService : BackgroundService
	{
		private readonly ILogger<VoucherCampaignBackgroundService> _logger;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public VoucherCampaignBackgroundService(ILogger<VoucherCampaignBackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
		{
			_logger = logger;
			_serviceScopeFactory = serviceScopeFactory;
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using (var scope = _serviceScopeFactory.CreateScope())
					{
						var _voucherCampaignService = scope.ServiceProvider.GetRequiredService<IVoucherCampaignService>();
						await _voucherCampaignService.BackGroundUpdateVoucherCampaign();
					}
					await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
					}
				catch (Exception ex)
				{
					_logger.LogError("Error occurred while updating voucher campaign.");
				}
			}
		}
	}
}
