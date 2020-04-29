using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CupController : Controller
    {
        private readonly IDbService _dbService;
        private readonly ICupService _cupService;
        
        public CupController(IDbService dbService, ICupService cupService)
        {
            _dbService = dbService;
            _cupService = cupService;
        }

        [HttpGet]
        public IEnumerable<Cup> Get()
        {
            return _dbService.GetCups();
        }

        [HttpGet("{id}/temperature")]
        public async Task<IEnumerable<Temperature>> GetTemperature(string id, [FromQuery] int limit = 1)
        {
            return await _dbService.GetTemperature(id, limit);
        }
        

        [HttpGet("{id}")]
        public Cup GetCup(string id)
        {
            return _dbService.GetCup(id);
        }

        [HttpPost("update/{id}")]
        public ActionResult UpdateCup(string id, [FromForm] CupFormData cup)
        {
            _cupService.UpdateCup(id,cup);
            return Redirect("/index.html");
        }
        [HttpPost("locate/{id}")]
        public ActionResult UpdateCup(string id)
        {
            _cupService.LocateCup(id);
            return Ok();
        }

        [HttpPost]
        public ActionResult PostCup([FromBody] Cup cup)
        {
            return Ok();
        }
    }
}