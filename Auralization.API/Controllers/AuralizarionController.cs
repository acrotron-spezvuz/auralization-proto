using Auralization.API.Models;
using Auralization.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Auralization.API.Controllers
{
    /// <summary>
    /// Auralization controller
    /// </summary>
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
        /// <summary>
        /// Auralization of selected sound sources
        /// </summary>
        /// <param name="sources"></param>
        /// <returns>Json object with filename </returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ApiResponse<string>))]
        public ActionResult<ApiResponse<string>> Post([FromBody]SoundSource[] sources)
        {
            try
            {
                _logger.LogDebug("Auralization ...");
                // pass sources to NAF lib
                var fileName = NAFService.Auralization(sources);

                // return results
                return Ok(new ApiResponse<string>(fileName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Auralization fail");
                return StatusCode(500);
            }
        }
    }
}
