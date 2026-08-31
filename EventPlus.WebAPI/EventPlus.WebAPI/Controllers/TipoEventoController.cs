using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")] //htttp://localhost:5170/api/TipoEvento
    [ApiController]
    public class TipoEventoController : ControllerBase
    {
        private readonly ITipoEvento _tipoEvento;

        public TipoEventoController(ITipoEvento tipoEvento)
        {
            _tipoEvento = tipoEvento;
        }

        /// <summary>
        /// Cadastra uma categoria de evento
        /// </summary>
        /// <param name="dto">objeto que será cadastrado</param>
        /// <returns>Status code 201 e o objeto cadastrado</returns>
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] TipoEventoDTO dto)
        {
            try
            {
                var tipoEvento = new TipoEvento
                {
                    TituloTipoEvento = dto.TituloTipoEvento
                };

                await _tipoEvento.Cadastrar(tipoEvento);

                return StatusCode(201, tipoEvento);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Lista todos as categorias de eventos
        /// </summary>
        /// <returns>Lista com as categorias de eventos</returns>
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

        /// <summary>
        /// Busca uma categoria de evento pelo seu id
        /// </summary>
        /// <param name="id">Id da categoria a ser buscado</param>
        /// <returns>Status code 200 e o objeto buscado</returns>
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

        /// <summary>
        /// Atualiza uma categoria de evento
        /// </summary>
        /// <param name="id">Id do evento a ser alterado</param>
        /// <param name="dto">Objeto com as novas informações</param>
        /// <returns>Status code 204 e o objeto atualizado</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] TipoEventoDTO dto)
        {
            try
            {
                var tipoEvento = new TipoEvento
                {
                    TituloTipoEvento = dto.TituloTipoEvento
                };

                await _tipoEvento.Atualizar(id, tipoEvento);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Remove uma categoria de evento
        /// </summary>
        /// <param name="id">Id do objeto a ser excluído</param>
        /// <returns>Status Code NoContent se der certo e 400 caso haja exceção</returns>
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