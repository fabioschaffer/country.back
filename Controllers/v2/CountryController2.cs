using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace country.back {

    [Authorize]
    [ApiController]
    [ApiVersion("2")]
    [Route("v{version:apiVersion}/country")]
    public class CountryController2 : ControllerBase {

        private readonly IConfiguration cfg;

        public CountryController2(IConfiguration cfg) => this.cfg = cfg;

        [HttpGet("GetAll")]
        public async Task<IEnumerable<CountryModel2>> GetAll() {
            Country2 country = new Country2(cfg);
            return await country.GetAll();
        }

        [HttpGet("GetItem")]
        public async Task<CountryModel2> GetItem(int id) {
            Country2 country = new Country2(cfg);
            return await country.Get(id);
        }

        [HttpPost("sSve")]
        public async Task<IActionResult> Save([FromBody] CountryModel2 model) {
            Country2 country = new Country2(cfg);
            bool ret = await country.Save(model);
            return ret ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [AllowAnonymous]
        [HttpGet("Source")]
        public string Source() => "https://github.com/fabioschaffer/country.back 2";
    }
}