using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Auralization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuralizarionController : ControllerBase
    {
        readonly ILogger _logger;

        public AuralizarionController(ILogger<AuralizarionController> logger)
        {
            _logger = logger;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _logger.LogDebug("Auralization ...");
        }
    }
}
