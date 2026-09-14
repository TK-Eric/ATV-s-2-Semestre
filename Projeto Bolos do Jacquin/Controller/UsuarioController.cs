using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Bolos_do_Jacquin.Constants;
using Projeto_Bolos_do_Jacquin.DTO;
using Projeto_Bolos_do_Jacquin.Interfaces;
using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuario _usuario;

        public UsuarioController(IUsuario usuario)
        {
            _usuario = usuario;
        }

        /// <summary>
        /// Cadastra um novo cliente.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Cadastrar([FromBody] UsuarioDTO dto)
        {
            if (dto == null)
                return BadRequest(new
                {
                    mensagem = "Os dados do usuário são obrigatórios."
                });

            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest(new
                {
                    mensagem = "O nome é obrigatório."
                });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new
                {
                    mensagem = "O e-mail é obrigatório."
                });

            if (string.IsNullOrWhiteSpace(dto.Senha))
                return BadRequest(new
                {
                    mensagem = "A senha é obrigatória."
                });

            bool emailExiste = await _usuario.EmailExiste(dto.Email);

            if (emailExiste)
                return Conflict(new
                {
                    mensagem = "Já existe um usuário cadastrado com este e-mail."
                });

            var usuario = new Usuarios
            {
                Nome = dto.Nome.Trim(),
                Email = dto.Email.Trim(),
                Senha = dto.Senha,

                // Cadastro público sempre cria CLIENTE.
                Perfil = Perfil.Cliente,

                // Novo usuário começa ativo.
                Situacao = true
            };

            await _usuario.Cadastrar(usuario);

            var response = new UsuarioResponseDTO
            {
                IdUsuarios = usuario.IdUsuarios,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil,
                Situacao = (bool)usuario.Situacao
            };

            return StatusCode(201, response);
        }

        /// <summary>
        /// Lista todos os usuários.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = Perfil.Administrador)]
        public async Task<IActionResult> Listar()
        {
            var usuarios = await _usuario.Listar();

            var response = usuarios.Select(usuario => new UsuarioResponseDTO
            {
                IdUsuarios = usuario.IdUsuarios,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil,
                Situacao = (bool)usuario.Situacao
            }).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Consulta os dados do usuário autenticado.
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(claimId, out int usuarioId))
                return Unauthorized(new
                {
                    mensagem = "Token inválido."
                });

            var usuario = await _usuario.BuscarPorId(usuarioId);

            if (usuario == null)
                return NotFound(new
                {
                    mensagem = "Usuário não encontrado."
                });

            if (!(bool)usuario.Situacao)
                return Unauthorized(new
                {
                    mensagem = "Usuário desativado."
                });

            var response = new UsuarioResponseDTO
            {
                IdUsuarios = usuario.IdUsuarios,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil,
                Situacao = (bool)usuario.Situacao
            };

            return Ok(response);
        }

        /// <summary>
        /// Busca um usuário por ID.
        /// Apenas administradores podem consultar outros usuários.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = Perfil.Administrador)]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var usuario = await _usuario.BuscarPorId(id);

            if (usuario == null)
                return NotFound(new
                {
                    mensagem = "Usuário não encontrado."
                });

            var response = new UsuarioResponseDTO
            {
                IdUsuarios = usuario.IdUsuarios,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil,
                Situacao = (bool)usuario.Situacao
            };

            return Ok(response);
        }

        /// <summary>
        /// Desativa um usuário.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = Perfil.Administrador)]
        public async Task<IActionResult> Deletar(int id)
        {
            var resultado = await _usuario.Deletar(id);

            if (!resultado)
                return NotFound(new
                {
                    mensagem = "Usuário não encontrado."
                });

            return Ok(new
            {
                mensagem = "Usuário desativado com sucesso."
            });
        }

        /// <summary>
        /// Atualiza os dados de um usuário.
        /// Administradores podem atualizar qualquer usuário.
        /// Clientes somente podem atualizar a própria conta.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Atualizar(
            int id,
            [FromBody] UsuarioDTO dto)
        {
            if (dto == null)
                return BadRequest(new
                {
                    mensagem = "Os dados são obrigatórios."
                });

            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(claimId, out int usuarioLogadoId))
                return Unauthorized(new
                {
                    mensagem = "Token inválido."
                });

            bool administrador =
                User.IsInRole(Perfil.Administrador);

            if (!administrador && usuarioLogadoId != id)
                return Forbid();

            var usuarioExistente = await _usuario.BuscarPorId(id);

            if (usuarioExistente == null)
                return NotFound(new
                {
                    mensagem = "Usuário não encontrado."
                });

            bool emailExiste = await _usuario.EmailExiste(
                dto.Email,
                id
            );

            if (emailExiste)
                return Conflict(new
                {
                    mensagem = "Já existe outro usuário com este e-mail."
                });

            var usuario = new Usuarios
            {
                Nome = dto.Nome.Trim(),
                Email = dto.Email.Trim(),
                Senha = dto.Senha
            };

            await _usuario.Atualizar(id, usuario);

            return NoContent();
        }
    }
}