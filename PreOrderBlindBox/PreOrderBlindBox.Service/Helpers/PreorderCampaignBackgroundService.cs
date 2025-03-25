using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PreOrderBlindBox.Data.DBContext;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Helpers
{
    public class PreorderCampaignBackgroundService : BackgroundService
    {
        private readonly ILogger<PreorderCampaignBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PreorderCampaignBackgroundService(ILogger<PreorderCampaignBackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
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
                        var _preorderCampaignService = scope.ServiceProvider.GetRequiredService<IPreorderCampaignService>();
                        await _preorderCampaignService.BackGroundUpdatePreorderCampaign();
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred while updating preorder campaigns: {ex.Message}");
                }
            }
        }
    }
}
