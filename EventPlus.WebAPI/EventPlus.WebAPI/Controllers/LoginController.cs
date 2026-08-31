using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventPlus.WebAPI.Controllers
{
    /// <summary>
    /// Controller responsável pela autenticação de usuários via JWT (JSON Web Token).
    /// 
    /// COMO FUNCIONA O JWT?
    /// 1. O usuário envia e-mail e senha via POST /api/Login.
    /// 2. A API valida as credenciais no banco (e-mail e hash BCrypt).
    /// 3. Se válido, a API gera um Token JWT assinado com uma chave secreta.
    /// 4. O cliente usa esse token no cabeçalho "Authorization: Bearer {token}" 
    ///    em todas as requisições seguintes que exigem autenticação ([Authorize]).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuario _usuario;
        private readonly IConfiguration _configuration;
        public LoginController(IUsuario usuario, IConfiguration configuration)
        {
            _usuario = usuario;
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            //1. Busca o usuário pelo e-mail e valida a senha com BCrypt
            var usuarioEncontrado = await _usuario.BuscarPorEmailESenha(dto.Email, dto.Senha);

            //2. Se as credenciais forem inválidas, retorna 401 Unauthorized
            if (usuarioEncontrado == null)
            {
                return Unauthorized("Email ou senha inválidos!");
            }

            //INICIO DA CONFIGURAÇÃO DO TOKEN JWT

            //3.Criar a lista de Claims(informações que ficam dentro do token)
            //Claims: são como "afirmações" sobre o usuário que ficam codificadas no token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioEncontrado.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuarioEncontrado.Email),
                new Claim("nome", usuarioEncontrado.Nome),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //4.Criar a chave de segurança com base na chave secreta definida
            var chaveSecreta = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            //Encoding.UTF8 : Definir o padrão de codificação de caracteres
            //GetBytes: "Pegar" a string e devolver um array de bytes
            );

            //5.Definir o algoritmo de assinatura (HMACSHA256 é o padrão)
            var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

            //6. "Montar" o token JWT com as informações
            var token = new JwtSecurityToken(
                //emissor: quem está emitindo o token
                issuer: "EventPlus.WebAPI",
                //público-alvo: quem tem permissão para consumir o token
                audience: "EventPlus.WebAPI",
                //informações de identidade
                claims: claims,
                //expiração do token
                expires: DateTime.UtcNow.AddHours(8),
                //credenciais de assinatura
                signingCredentials: credenciais
            );

            //7. Converter o token para string e retornar para o clinte
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