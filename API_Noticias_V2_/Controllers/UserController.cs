using API_Noticias_V2_.Models;
using API_Noticias_V2_.Token;
using Entities.Entities;
using Entities.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Noticias_V2_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CreateTokenIdentity")]
        public async Task<IActionResult> CreateTokenIdentity([FromBody] Login login)
        {
            if(string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
            {
                return Unauthorized();
            }

            var resultado = await _signInManager.PasswordSignInAsync(login.Email, login.Senha, false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                //Recupera Usuário Logado
                var userCurrent = await _userManager.FindByNameAsync(login.Email);
                var idUsuario = userCurrent.Id;

                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                .AddSubject("Empresa - Projeto DDD")
                .AddIssuer("Teste.Securiry.Bearer")
                .AddAudience("Teste.Securiry.Bearer")
                .AddClaim("idUsuario", idUsuario)
                .AddExpiry(5)
                .Builder();

                return Ok(token.value);
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/AdicionarUsuarioIdentity")]
        public async Task<IActionResult> AdicionarUsuarioIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
            {
                return Ok("Falta alguns dados");
            }

            var user = new ApplicationUser
            {
                UserName = login.Email,
                Email = login.Email,
                Cpf = login.Cpf,
                Tipo = TipoUsuario.Comum,
            };

            var resultado = await _userManager.CreateAsync(user, login.Senha);

            if (resultado.Errors.Any())
            {
                return Ok(resultado.Errors);
            }

            var userId = await _userManager.GetUserIdAsync(user);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // retorno email 
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var respose_Retorn = await _userManager.ConfirmEmailAsync(user, code);

            if (respose_Retorn.Succeeded)
                return Ok("Usuário Adicionado com sucesso");
            else
                return Ok("Erro ao confirmar cadastro do usuário");
        }
    }
}