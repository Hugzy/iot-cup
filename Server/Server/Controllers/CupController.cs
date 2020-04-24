using System.Collections.Generic;
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
        
        public CupController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IEnumerable<Cup> Get()
        {
            return _dbService.GetCups();
        }

        [HttpGet("{id}")]
        public Cup GetCup(string id)
        {
            return new Cup();
        }

        [HttpPost]
        public ActionResult PostCup([FromBody] Cup cup)
        {
            return Ok();
        }
    }
}