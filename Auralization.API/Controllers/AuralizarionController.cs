﻿using Auralization.API.Models;
using Auralization.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
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
