﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Auralization.Spa.Controllers
{
    /// <summary>
    /// Configuration controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        /// <summary>
        /// Api settings
        /// </summary>
        readonly IOptionsSnapshot<ApiSettings> _apiSettings;

        public ConfigController(IOptionsSnapshot<ApiSettings> apiSettings)
        {
            // inject
            _apiSettings = apiSettings;
        }

        /// <summary>
        /// get config
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Config()
        {
            return Ok(_apiSettings.Value);
        }
    }
}