using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Bolos_do_Jacquin.Constants;
using Projeto_Bolos_do_Jacquin.DTO;
using Projeto_Bolos_do_Jacquin.Interfaces;
using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoria _categoria;

        public CategoriaController(ICategoria categoria)
        {
            _categoria = categoria;
        }

        /// <summary>
        /// Busca uma categoria pelo ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O identificador da categoria é inválido."
                });
            }

            var categoria = await _categoria.BuscarPorId(id);

            if (categoria == null)
            {
                return NotFound(new
                {
                    mensagem = "Categoria não encontrada."
                });
            }

            return Ok(categoria);
        }

        /// <summary>
        /// Cadastra uma nova categoria.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = Perfil.Administrador)]
        public async Task<IActionResult> Cadastrar(
            [FromBody] CategoriaDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    mensagem = "Os dados da categoria são obrigatórios."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.NomeCategoria))
            {
                return BadRequest(new
                {
                    mensagem = "O nome da categoria é obrigatório."
                });
            }

            var categoria = new Categorias
            {
                NomeCategoria = dto.NomeCategoria.Trim()
            };

            await _categoria.Cadastrar(categoria);

            return StatusCode(201, categoria);
        }

        /// <summary>
        /// Atualiza uma categoria existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = Perfil.Administrador)]
        public async Task<IActionResult> Atualizar(
            int id,
            [FromBody] CategoriaDTO dto)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O identificador da categoria é inválido."
                });
            }

            if (dto == null)
            {
                return BadRequest(new
                {
                    mensagem = "Os dados da categoria são obrigatórios."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.NomeCategoria))
            {
                return BadRequest(new
                {
                    mensagem = "O nome da categoria é obrigatório."
                });
            }

            var categoriaExistente = await _categoria.BuscarPorId(id);

            if (categoriaExistente == null)
            {
                return NotFound(new
                {
                    mensagem = "Categoria não encontrada."
                });
            }

            var categoria = new Categorias
            {
                NomeCategoria = dto.NomeCategoria.Trim()
            };

            await _categoria.Atualizar(id, categoria);

            return NoContent();
        }

        /// <summary>
        /// Lista todas as categorias.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var categorias = await _categoria.Listar();

            return Ok(categorias);
        }

        /// <summary>
        /// Exclui uma categoria somente quando ela não possui produtos vinculados.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = Perfil.Administrador)]
        public async Task<IActionResult> Deletar(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O identificador da categoria é inválido."
                });
            }

            try
            {
                var categoria = await _categoria.BuscarPorId(id);

                if (categoria == null)
                {
                    return NotFound(new
                    {
                        mensagem = "Categoria não encontrada."
                    });
                }

                await _categoria.Deletar(id);

                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return Conflict(new
                {
                    mensagem = "A categoria não pode ser excluída porque possui produtos vinculados."
                });
            }
        }
    }
}