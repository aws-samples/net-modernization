using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Interface
{
    public interface ISNSService
    {
        /// <summary>
        /// Utilizes AWS SNS API to send publish messages.
        /// </summary>
        /// <param name="message">message in Json format. </param>
        Task PublishMessageToSNSAsync(string message);
    }
}
