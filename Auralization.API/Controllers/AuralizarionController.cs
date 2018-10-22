using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Auralization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuralizarionController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
