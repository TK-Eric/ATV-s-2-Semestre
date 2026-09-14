using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Projeto_Bolos_do_Jacquin.Constants;
using Projeto_Bolos_do_Jacquin.DTO;
using Projeto_Bolos_do_Jacquin.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Projeto_Bolos_do_Jacquin.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuario _usuario;
        private readonly IConfiguration _configuration;

        public LoginController(
            IUsuario usuario,
            IConfiguration configuration)
        {
            _usuario = usuario;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Senha))
            {
                return BadRequest(new
                {
                    mensagem = "E-mail e senha são obrigatórios."
                });
            }

            var usuarioEncontrado =
                await _usuario.BuscarPorEmailESenha(
                    dto.Email.Trim(),
                    dto.Senha
                );

            if (usuarioEncontrado == null)
            {
                return Unauthorized(new
                {
                    mensagem = "E-mail ou senha inválidos."
                });
            }

            if (string.IsNullOrWhiteSpace(usuarioEncontrado.Perfil))
            {
                return StatusCode(500, new
                {
                    mensagem = "Usuário sem perfil configurado."
                });
            }

            var chaveJwt = _configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(chaveJwt))
            {
                return StatusCode(500, new
                {
                    mensagem = "A configuração da autenticação não está disponível."
                });
            }

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuarioEncontrado.IdUsuarios.ToString()
                ),

                new Claim(
                    ClaimTypes.Role,
                    usuarioEncontrado.Perfil
                ),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    usuarioEncontrado.Email
                ),

                new Claim(
                    "nome",
                    usuarioEncontrado.Nome
                ),

                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()
                )
            };

            var chaveSecreta = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(chaveJwt)
            );

            var credenciais = new SigningCredentials(
                chaveSecreta,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: "Projeto_Bolos_do_Jacquin",
                audience: "Projeto_Bolos_do_Jacquin",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credenciais
            );

            var tokenString =
                new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                Token = tokenString,
                Expiracao = token.ValidTo,
                Usuario = new
                {
                    usuarioEncontrado.IdUsuarios,
                    usuarioEncontrado.Nome,
                    usuarioEncontrado.Email,
                    usuarioEncontrado.Perfil
                }
            });
        }
    }
}