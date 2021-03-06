using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace country.back {

    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/country")]
    public class CountryController : ControllerBase {

        private readonly IConfiguration cfg;

        public CountryController(IConfiguration cfg) => this.cfg = cfg;

        [HttpGet("GetAll")]
        public async Task<IEnumerable<CountryModel>> GetAll() {
            Country country = new Country(cfg);
            return await country.GetAll();
        }

        [HttpGet("GetItem")]
        public async Task<CountryModel> GetItem(int id) {
            Country country = new Country(cfg);
            return await country.Get(id);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] CountryModel model) {
            Country country = new Country(cfg);
            bool ret = await country.Save(model);
            return ret ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [AllowAnonymous]
        [HttpGet("Source")]
        public string Source() => "https://github.com/fabioschaffer/country.back 1";
    }
}