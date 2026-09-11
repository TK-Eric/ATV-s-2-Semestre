using Microsoft.AspNetCore.Mvc;
using Projeto_Bolos_do_Jacquin.DTO;
using Projeto_Bolos_do_Jacquin.Interfaces;
using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProduto _produto;

        public ProdutoController(IProduto produto)
        {
            _produto = produto;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromForm] ProdutoDTO dto)
        {
            try
            {

                var produto = new Produtos
                {
                    Nome = dto.Nome,
                    DescricaoCurta = dto.DescricaoCurta,
                    DescricaoLonga = dto.DescricaoLonga,
                    Situacao = dto.Situacao,
                    Disponibilidade = dto.Disponibilidade,
                };

                await _produto.Cadastrar(produto);

                return StatusCode(201, produto);
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message, inner = e.InnerException?.Message });
            }
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromForm] ProdutoDTO dto)
        {
            try
            {
                var produto = new Produtos
                {
                    Nome = dto.Nome,
                    DescricaoCurta = dto.DescricaoCurta,
                    DescricaoLonga = dto.DescricaoLonga,
                    Situacao = dto.Situacao,
                    Disponibilidade = dto.Disponibilidade,
                };

                await _produto.Atualizar(id, produto);

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
                return Ok(await _produto.Listar());
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
                await _produto.Deletar(id);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}