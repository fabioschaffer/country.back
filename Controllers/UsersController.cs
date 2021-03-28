using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace country.back {
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase {

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model) {
            UserService service = new UserService();
            User user = await service.Authenticate(model.Username, model.Password);
            if (user == null) {
                return BadRequest(new { message = "Usuário ou senha inválido." });
            }
            return Ok(user);
        }
    }
}