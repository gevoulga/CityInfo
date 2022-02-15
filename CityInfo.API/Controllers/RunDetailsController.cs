using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CityInfo.API.Contexts;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunDetailsController : ControllerBase
    {
        [HttpGet]
        //Using Action injection
        public async Task<IActionResult> GetGuid(
            [FromServices] IOpenGenericService<GuidAttribute> guidService)
        {
            var guid = await guidService.GetAsync();
            return guid is null ? Ok(Guid.NewGuid()) : Ok(guid.Value);
        }

        private readonly CityInfoContext _context;

        public RunDetailsController(CityInfoContext context)
        {
            _context = context;
        }

        [HttpGet("db")]
        //Using Action injection
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}