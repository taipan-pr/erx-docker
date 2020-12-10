using Erx.Api.Attributes;
using Erx.Api.DataTransferObjects;
using Erx.Queue;
using Microsoft.AspNetCore.Mvc;

namespace Erx.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly IQueueService _queueService;

        public QueueController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult EnqueueMessageAsync(PublishMessageRequest request)
        {
            if (request == null) return BadRequest();
            _queueService.PublishMessage(request.Message);
            return Ok();
        }
    }
}
