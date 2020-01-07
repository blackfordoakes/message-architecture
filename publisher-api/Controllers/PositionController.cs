using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using publisher_api.Services;

namespace publisher_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PositionController : ControllerBase
    {
        private readonly ILogger<PositionController> _logger;
        private readonly IPositionService _positionService;

        public PositionController(ILogger<PositionController> logger, IPositionService positionService)
        {
            _logger = logger;
            _positionService = positionService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var gps = new Position
            {
                Latitude = 40.447307,
                Longitude = -80.006841,
                Timestamp = DateTimeOffset.Now
            };

            return Ok(gps);
        }

        [HttpPost(Name = nameof(SavePosition))]
        [ProducesResponseType(200)]
        public IActionResult SavePosition(Position gps)
        {
            _positionService.Save(gps);
            return Ok();
        }
    }
}
