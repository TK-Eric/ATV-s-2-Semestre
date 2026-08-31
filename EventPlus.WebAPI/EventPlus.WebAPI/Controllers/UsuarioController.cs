using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EventPlus.WebAPI.Controllers
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
        /// Cadastra um usuário
        /// </summary>
        /// <param name="dto">Objeto a ser cadastrado</param>
        /// <returns>Status code e o objeto cadastrado</returns>
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] UsuarioDTO dto)
        {
            try
            {
                var usuario = new Usuario
                {
                    Nome = dto.Nome,
                    Email = dto.Email,
                    Senha = dto.Senha,//obs: a criptografia ocorre dentro do repositório
                    IdTipoUsuario = dto.IdTipoUsuario
                };

                await _usuario.Cadastrar(usuario);

                return StatusCode(201, usuario);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Lista todos os usuários
        /// </summary>
        /// <returns>Status code e a lista de usuários</returns>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var usuarios = await _usuario.Listar();
                return Ok(usuarios);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Busca um usuário pelo seu id
        /// </summary>
        /// <param name="id">Id do usuário a ser buscado</param>
        /// <returns>Status Code 200 com objeto ou 400</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var usuarioBuscado = await _usuario.BuscarPorId(id);

                if (usuarioBuscado == null)
                {
                    return NotFound("Tipo de usuário não encontrado.");
                }

                return Ok(usuarioBuscado);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

        /// <summary>
        /// Exclui um usuário pelo seu id
        /// </summary>
        /// <param name="id">Id do usuário a ser excluido</param>
        /// <returns>Status Code 204 ou 400</returns>        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _usuario.Deletar(id);

                return NoContent();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Atualiza um usuário
        /// </summary>
        /// <param name="id">Id do usuário a ser atualizado</param>
        /// <param name="dto">Objeto com as novas informações</param>
        /// <returns>Status Code NoContent</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] UsuarioDTO dto)
        {
            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha,
                IdTipoUsuario = dto.IdTipoUsuario
            };

            await _usuario.Atualizar(id, usuario);

            return NoContent();
        }
    }
}