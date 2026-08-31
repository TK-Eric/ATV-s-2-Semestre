using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Repositories;
using EventPlus.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
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
                bool reprovado = await _moderationService.ModerarTexto(dto.Descricao);

                var comentario = new Comentario
                {
                    DataComentario = DateTime.Now,
                    Descricao = dto.Descricao,
                    IdEvento = dto.IdEvento,
                    IdUsuario = dto.IdUsuario,
                    Exibe = !reprovado
                };

                await _comentario.Cadastrar(comentario);

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
        public async Task<IActionResult> Atualizar(Guid id, [FromForm] ComentarioDTO dto)
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
                return Ok(await _comentario.Listar());
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
                await _comentario.Deletar(id);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}

//CRUDS ?

// Tenho todos os controlers para todos os métodos de cada repositório

// Travas as rotas com Authorize



//Para quem já testou todas as rotas, iniciar o processo abaixo:

// Iniciar o projeto 2 - Backend
// 1) Diagrama do banco
// 2) Diagrama de classe 
// 3) Validar c/ professores os diagramas
// 4) Criar o banco
// 5) Desenvolver a API