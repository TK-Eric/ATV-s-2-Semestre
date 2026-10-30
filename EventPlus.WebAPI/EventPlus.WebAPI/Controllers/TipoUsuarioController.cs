using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
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

        [HttpGet]
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

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] TipoUsuarioDTO dto)
        {
            try
            {
                var titulo = !string.IsNullOrWhiteSpace(dto.Titulo)
                    ? dto.Titulo.Trim()
                    : dto.TituloTipoUsuario?.Trim();

                if (string.IsNullOrWhiteSpace(titulo))
                {
                    return BadRequest("O título é obrigatório.");
                }

                var tipoUsuario = new TipoUsuario
                {
                    TituloTipoUsuario = titulo
                };

                await _tipoUsuario.Cadastrar(tipoUsuario);

                return StatusCode(201, tipoUsuario);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] TipoUsuarioDTO dto)
        {
            try
            {
                var titulo = !string.IsNullOrWhiteSpace(dto.Titulo)
                    ? dto.Titulo.Trim()
                    : dto.TituloTipoUsuario?.Trim();

                if (string.IsNullOrWhiteSpace(titulo))
                {
                    return BadRequest("O título é obrigatório.");
                }

                var tipoUsuario = new TipoUsuario
                {
                    TituloTipoUsuario = titulo
                };

                await _tipoUsuario.Atualizar(id, tipoUsuario);

                return Ok(tipoUsuario);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

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
