using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase
    {
        private readonly ITipoUsuario _tipoUsuario;

        public TipoUsuarioController(ITipoUsuario tipoUsuario)
        {
            _tipoUsuario = tipoUsuario;
        }

        /// <summary>
        /// Busca um tipo de usuário pelo seu id
        /// </summary>
        /// <param name="id">Id do usuário a ser buscado</param>
        /// <returns>Status Code 200 com objeto ou 404</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var tipoUsuarioBuscado = await _tipoUsuario.BuscarPorId(id);

                if (tipoUsuarioBuscado == null)
                {
                    return NotFound("Tipo de usuário não encontrado.");
                }

                return Ok(tipoUsuarioBuscado);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

        /// <summary>
        /// Lista todos os perfis de usuário
        /// </summary>
        /// <returns>Status Code 200 com a lista dos perfis de usuário ou 400</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _tipoUsuario.Listar();

                return Ok(tipos);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

        /// <summary>
        /// Cadastra um novo perfil de usuário
        /// </summary>
        /// <param name="tipoUsuario">Perfil do usuário a ser cadastrado</param>
        /// <returns>Status Code 201 com objeto cadastrado ou 404</returns>
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] TipoUsuarioDTO dto)
        {
            try
            {
                var tipoUsuario = new TipoUsuario
                {
                    TituloTipoUsuario = dto.Titulo
                };

                await _tipoUsuario.Cadastrar(tipoUsuario);

                return StatusCode(201, tipoUsuario);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

        /// <summary>
        /// Atualiza um tipo de usuário
        /// </summary>
        /// <param name="id">Id do usuário a ser atualizado</param>
        /// <param name="dto">Objeto com novas informações</param>
        /// <returns>Status Code 200 com objeto atualizado ou 404</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] TipoUsuarioDTO dto)
        {
            try
            {
                var tipoUsuario = new TipoUsuario
                {
                    TituloTipoUsuario = dto.Titulo
                };

                await _tipoUsuario.Atualizar(id, tipoUsuario);

                return Ok(tipoUsuario);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

        /// <summary>
        /// Remove um perfil de usuário pelo ID.
        /// </summary>
        /// <param name="id">Id do perfil a ser removido</param>
        /// <returns>Status code 203 ou 404 </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _tipoUsuario.Deletar(id);

                return NoContent();
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }
    }
}