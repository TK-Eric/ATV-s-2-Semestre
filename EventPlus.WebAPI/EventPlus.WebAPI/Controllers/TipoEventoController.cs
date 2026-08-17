using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

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
        [HttpGet("{id:guid}")]

        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var tipoEventoBuscado = await _tipoEvento.BuscarPorId(id);

            if (tipoEventoBuscado == null)
                return NotFound("Tipo Usuario nao encontrado.");

            return Ok(tipoEventoBuscado);
        }


        [HttpGet]
        //<summary>
        //Lista todos os perfiz e usuario
        //</summary>

        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _tipoEvento.Listar();

                return Ok(tipos);

            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult> Cadastrar([FromBody] TipoEventoDTO dto)
        {

            var tipoEvento = new TipoEvento
            {
                TituloTipoEvento = dto.TituloTipoEvento
            };

            await _tipoEvento.Cadastrar(tipoEvento);

            return StatusCode(201, tipoEvento);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id,
            [FromBody] TipoEventoDTO dto)
        {
            var tipoEvento = new TipoEvento
            {
                TituloTipoEvento = dto.TituloTipoEvento
            };

            await _tipoEvento.Atualizar(id, tipoEvento);

            return Ok(tipoEvento);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _tipoEvento.Deletar(id);
            return NoContent();
        }

    }
}