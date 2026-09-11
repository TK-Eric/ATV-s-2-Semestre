using EventPlus.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Projeto_Bolos_do_Jacquin.DTO;
using Projeto_Bolos_do_Jacquin.Interfaces;
using Projeto_Bolos_do_Jacquin.Models;
using Projeto_Bolos_do_Jacquin.Repositories;

namespace Projeto_Bolos_do_Jacquin.Controller
{
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoria _categoria;

        private readonly IModerationService _moderationService;

        public CategoriaController(ICategoria categoria, IModerationService moderationService)
        {
            _categoria = categoria;
            _moderationService = moderationService;

        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var categoria = await _categoria.BuscarPorId(id);

            if (categoria == null)
            {
                return NotFound("Categoria não encontrada."); // Retorna HTTP 404
            }

            return Ok(categoria); // Retorna HTTP 200 com os dados
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] CategoriaDTO dto)
        {
            try
            {

                var categoria = new Categorias
                {
                    NomeCategoria = dto.NomeCategoria,
                };

                await _categoria.Cadastrar(categoria);

                return StatusCode(201, categoria);
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
        public async Task<IActionResult> Atualizar(Guid id, [FromForm] CategoriaDTO dto)
        {
            try
            {
                var categoria = new Categorias
                {
                    NomeCategoria = dto.NomeCategoria,
                };

                await _categoria.Atualizar(id, categoria);

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
                return Ok(await _categoria.Listar());
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
                await _categoria.Deletar(id);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }

    }
}
}
