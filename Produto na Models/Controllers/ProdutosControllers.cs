using Microsoft.AspNetCore.Mvc;
using ProdutoCodeFirstAPI.Models;
using ProdutoCodeFirstAPI.Services;

namespace ProdutoCodeFirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoService _produtoService;

        public ProdutosController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            var produtos = await _produtoService.ObterTodosAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(Guid id)
        {
            var produto = await _produtoService.ObterPorIdAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            try
            {
                var produtoCriado = await _produtoService.CriarAsync(produto);
                return CreatedAtAction(nameof(GetProduto), new { id = produtoCriado.Id }, produtoCriado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(Guid id, Produto produto)
        {
            try
            {
                var produtoAtualizado = await _produtoService.AtualizarAsync(id, produto);

                if (produtoAtualizado == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(Guid id)
        {
            var deletado = await _produtoService.DeletarAsync(id);

            if (!deletado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
