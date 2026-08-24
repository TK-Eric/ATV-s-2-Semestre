using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuario _usuario;
        private readonly IConfiguration _configuration;

        public LoginController(IUsuario usuario)
        {
            _usuario = usuario;
        }

        [HttpPost]

        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {

            var usuarioEncontrado = await _usuario.BuscarPorEmailESenha(dto.Email, dto.Senha);

            if (usuarioEncontrado == null)
            {
                return Unauthorized("Email ou senha erradas");
            }
            var claim = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, usuarioEncontrado.IdUsuario.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuarioEncontrado.Email),
            new Claim("nome", usuarioEncontrado.Nome),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var chaveSecreta = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
                );
            var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(

                issuer: "EventPlus.WebAPI",
                audience: "EventPlus.WebAPI",
                claims: claim,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credenciais
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                Token = tokenString,
                Expiracao = token.ValidTo,
                Usuario = new
                {
                    usuarioEncontrado.IdUsuario,
                    usuarioEncontrado.Nome,
                    usuarioEncontrado.Email
                }
            }
            );

        }
    }
}
