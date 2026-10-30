using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoEventoController : ControllerBase
    {
        private readonly ITipoEvento _tipoEvento;

        public TipoEventoController(ITipoEvento tipoEvento)
        {
            _tipoEvento = tipoEvento;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] TipoEventoDTO dto)
        {
            try
            {
                var titulo = !string.IsNullOrWhiteSpace(dto.TituloTipoEvento)
                    ? dto.TituloTipoEvento.Trim()
                    : dto.Titulo?.Trim();

                if (string.IsNullOrWhiteSpace(titulo))
                {
                    return BadRequest("O título do tipo de evento é obrigatório!");
                }

                var tipoEvento = new TipoEvento
                {
                    TituloTipoEvento = titulo
                };

                await _tipoEvento.Cadastrar(tipoEvento);

                return StatusCode(201, tipoEvento);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _tipoEvento.Listar();
                return Ok(tipos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var tipo = await _tipoEvento.BuscarPorId(id);

                if (tipo == null)
                {
                    return NotFound();
                }

                return Ok(tipo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] TipoEventoDTO dto)
        {
            try
            {
                var titulo = !string.IsNullOrWhiteSpace(dto.TituloTipoEvento)
                    ? dto.TituloTipoEvento.Trim()
                    : dto.Titulo?.Trim();

                if (string.IsNullOrWhiteSpace(titulo))
                {
                    return BadRequest("O título do tipo de evento é obrigatório!");
                }

                var tipoEvento = new TipoEvento
                {
                    TituloTipoEvento = titulo
                };

                await _tipoEvento.Atualizar(id, tipoEvento);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _tipoEvento.Deletar(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
