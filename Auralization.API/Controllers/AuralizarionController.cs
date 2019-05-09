    using Auralization.API.Models;
using Auralization.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
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
        private static readonly long MaxFileSize = 100 * 1024 * 1024; //100 mb

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

        // POST api/values
        /// <summary>
        /// Upload wav or csv configuratttioooon                 
        /// </summary>
        /// <param name="file">multiparted file data</param>
        /// <returns>nothing</returns>
        /// <response code="200">Indicates a successfull upload</response>
        /// <response code="400">If the request model is wrong</response>
        /// <response code="500">Something went wrong</response>       
        [HttpPost("UploadFile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        
        public async Task<ActionResult<ApiResponse<bool>>> Post(IFormFile file)
        {
            try
            {
                var uploadResult = await processFile(file);
                return Ok(new ApiResponse<bool>(uploadResult));
            } catch (Exception ex)
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
        /// <param name="file">multiparted file data</param>
        /// <returns>a string with location of a generated sound file or an error description, 
        /// same as the "Auralization" service response.</returns>
        /// <response code="200">Returns the filename</response>
        /// <response code="400">If the request model is wrong</response>
        /// <response code="500">Something went wrong</response>       
        [HttpPost("AuralizeFromContentAndFile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<string>>> Post([FromForm]string content, IFormFile file)
        {
            try
            {
                var fileUploaded = await processFile(file);
                if (!fileUploaded)
                {
                    return StatusCode(400);
                }

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

        private async Task<bool> processFile(IFormFile file)
        {
            _logger.LogDebug("Uploading file");
            if (file == null || file.Length == 0 || file.Length > MaxFileSize)
            {
                _logger.LogError("Uploading null file or a file with incorrect size. Ignoring");
                return false;
            }

            var supportedExtensions = new[] { ".csv", ".wav" };
            var extension = Path.GetExtension(file.FileName);
            if (!supportedExtensions.Contains(extension))
            {
                _logger.LogError("Unsupported file extension");
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "input", file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }
    }
}
