using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class PrimesController : ControllerBase
    {
        private readonly ILogger<PrimesController> _logger;

        public PrimesController(ILogger<PrimesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns prime factors for a given number
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        [HttpGet("{n}")]
        [ProducesResponseType(typeof(int[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Factors(int n)
        {
            if (n < 0)
            {
                return BadRequest("Parameter must be a non-negative integer");
            }

            var factors = new List<int>();
            for (int p = 2; p <= Math.Sqrt(n); p++)
            {
                while (n % p == 0)
                {
                    factors.Add(p);
                    n /= p;
                }
            }

            if (n > 1)
            {
                factors.Add(n);
            }

            return Ok(factors);
        }
    }
}
