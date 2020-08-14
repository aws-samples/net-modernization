using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using InventoryService.Interface;
using Microsoft.Extensions.Configuration;

namespace InventoryService.Service
{
    public class SNSService : ISNSService
    {
        private readonly IConfiguration configuration;

        public SNSService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <inheritdoc />
        public async Task PublishMessageToSNSAsync(string message)
        {
            var client = new AmazonSimpleNotificationServiceClient();

            var request = new PublishRequest
            {
                TopicArn = this.configuration.GetSection("SNSTopicArn").Value,
                Message = message,
            };

            await client.PublishAsync(request);
        }
    }
}
