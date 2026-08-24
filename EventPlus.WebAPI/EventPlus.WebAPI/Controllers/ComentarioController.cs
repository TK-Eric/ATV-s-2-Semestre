using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    // Isso aqui avisa que essa classe é uma API (ativa umas verificações
    // automáticas, tipo checar se os dados enviados estão certos)
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentario _comentario;
        private readonly IModerationService _moderationService;

        public ComentarioController(IComentario comentario, IModerationService moderationService)
        {
            _comentario = comentario;
            _moderationService = moderationService;
        }

        [HttpPost]

        public async Task<IActionResult> Cadastrar([FromBody] ComentarioDTO dto)
        {
            try
            {
                bool reprovado = await _moderationService.ModeratorTexto(dto.Descricao);

                var comentario = new Comentario
                {
                    Descricao = dto.Descricao,
                    IdEvento = dto.IdEvento,
                    IdUsuario = dto.IdUsuario,
                    Exibe = !reprovado
                };

                await _comentario.Cadastrar(comentario);

                return StatusCode(201, comentario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            {

            }
            return Ok(dto);
        }
    }
}
