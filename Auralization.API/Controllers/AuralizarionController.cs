using Auralization.API.Models;
using Auralization.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Auralization.API.Controllers
{
    /// <summary>
    /// Auralization controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuralizationController : ControllerBase
    {
        readonly ILogger _logger;

        public AuralizationController(ILogger<AuralizationController> logger)
        {
            _logger = logger;
        }

        // POST api/values
        /// <summary>
        /// Auralization of selected sound sources
        /// </summary>
        /// <param name="sources"></param>
        /// <returns>Json object with filename </returns>
        /// <response code="200">Returns the filename</response>
        /// <response code="400">If the request model is wrong</response>
        /// <response code="500">Something went wrong</response>       
        [HttpPost("AuralizeFromSources")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<string>>> Post([FromBody]SoundSource[] sources)
        {
            try
            {
                _logger.LogDebug("Auralization ...");
                // pass sources to NAF lib
                var fileName = NAFService.Auralization(sources);

                // delay
                await Task.Delay(500);

                // return results
                return Ok(new ApiResponse<string>(fileName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Auralization fail");
                return StatusCode(500);
            }
        }

        // POST api/values
        /// <summary>
        /// Auralization of sound sources based on text in NAFSnaP format.
        /// </summary>
        /// <param name="content">a string to accept NAFSnaP-formatted content.</param>
        /// <returns>a string with location of a generated sound file or an error description, 
        /// same as the "Auralization" service response.</returns>
        /// <response code="200">Returns the filename</response>
        /// <response code="400">If the request model is wrong</response>
        /// <response code="500">Something went wrong</response>       
        [HttpPost("AuralizeFromContent")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<string>>> Post([FromBody]string content)
        {
            try
            {
                _logger.LogDebug("Auralization ...");
                // pass sources to NAF lib
                var fileName = NAFService.AuralizeFromContent(content);

                // delay
                await Task.Delay(500);

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
