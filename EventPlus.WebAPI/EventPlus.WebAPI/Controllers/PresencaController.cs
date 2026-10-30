using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresencaController : ControllerBase
    {
        private readonly IPresenca _presenca;

        public PresencaController(IPresenca presenca)
        {
            _presenca = presenca;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                return Ok(await _presenca.Listar());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var presenca = await _presenca.BuscarPorId(id);
                if (presenca == null)
                {
                    return NotFound("Presença não encontrada.");
                }

                return Ok(presenca);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ListarMinhas/{idUsuario:guid}")]
        [HttpGet("usuario/{idUsuario:guid}")]
        public async Task<IActionResult> ListarMinhasPresencas(Guid idUsuario)
        {
            try
            {
                return Ok(await _presenca.ListarMinhasPresencas(idUsuario));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Inscrever([FromBody] PresencaDTO dto)
        {
            try
            {
                var presenca = new Presenca
                {
                    Situacao = dto.Situacao,
                    IdEvento = dto.IdEvento,
                    IdUsuario = dto.IdUsuario
                };

                await _presenca.Inscrever(presenca);

                return StatusCode(201, presenca);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarSituacao(Guid id, [FromBody] bool situacao)
        {
            try
            {
                await _presenca.AtualizarSituacao(id, situacao);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _presenca.Deletar(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
