using EventPlus.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Projeto_Bolos_do_Jacquin.DTO;
using Projeto_Bolos_do_Jacquin.Interfaces;
using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacao _avaliacao;

        private readonly IModerationService _moderationService;

        public AvaliacaoController(IAvaliacao avaliacao, IModerationService moderationService)
        {
            _avaliacao = avaliacao;
            _moderationService = moderationService;

        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] AvaliacaoDTO dto)
        {
            try
            {
                bool reprovado = await _moderationService.ModerarTexto(dto.Descricao);

                var comentario = new Comentario
                {
                    DataComentario = DateTime.Now,
                    Descricao = dto.Descricao,
                    IdEvento = dto.IdEvento,
                    IdUsuario = dto.IdUsuario,
                    Exibe = !reprovado
                };

                await _avaliacao.Cadastrar(comentario);

                return StatusCode(201, comentario);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    e.Message,
                    inner = e.InnerException?.Message


                });

            }

        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromForm] AvaliacaoDTO dto)
        {
            try
            {
                var evento = new Comentario
                {
                    Descricao = dto.Descricao,
                    IdEvento = dto.IdEvento,
                    IdUsuario = dto.IdUsuario,
                };

                await _comentario.Atualizar(id, evento);

                return NoContent();
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
                return Ok(await _avaliacao.Listar());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _avaliacao.Deletar(id);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }

    }
}
