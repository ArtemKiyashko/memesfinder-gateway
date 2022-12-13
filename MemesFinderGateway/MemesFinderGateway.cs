using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace MemesFinderGateway
{
    public class MemesFinderGateway
    {
        public MemesFinderGateway()
        {
        }

        [FunctionName("MemesFinderGateway")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Update tgUpdate,
            ILogger log)
        {
            return new OkObjectResult(tgUpdate.Message is null ? tgUpdate.EditedMessage.Text : tgUpdate.Message.Text);
        }
    }
}

