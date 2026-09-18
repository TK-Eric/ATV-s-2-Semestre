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
    public class ProdutoController : ControllerBase
    {
        private readonly IProduto _produto;

        public ProdutoController(IProduto produto)
        {
            _produto = produto;
        }

        [HttpPost]
        [Authorize(Roles = Perfil.Administrador)]
        public async Task<IActionResult> Cadastrar([FromBody] ProdutoDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    mensagem = "Os dados do produto são obrigatórios."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                return BadRequest(new
                {
                    mensagem = "O nome do produto é obrigatório."
                });
            }

            if (dto.Preco <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O preço do produto deve ser maior que zero."
                });
            }

            if (dto.IdCategoria <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "A categoria do produto é obrigatória."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.EnderecoImagem))
            {
                return BadRequest(new
                {
                    mensagem = "O endereço da imagem é obrigatório."
                });
            }

            var produto = new Produtos
            {
                Nome = dto.Nome.Trim(),
                Preco = dto.Preco,
                EnderecoImagem = dto.EnderecoImagem.Trim(),
                IdCategoria = dto.IdCategoria,
                DescricaoCurta = dto.DescricaoCurta?.Trim(),
                DescricaoLonga = dto.DescricaoLonga?.Trim(),
                Disponibilidade = dto.Disponibilidade,
                Situacao = dto.Situacao
            };

            await _produto.Cadastrar(produto);

            return StatusCode(201, produto);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Perfil.Administrador)]
        public async Task<IActionResult> Atualizar(
            int id,
            [FromBody] ProdutoDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    mensagem = "Os dados do produto são obrigatórios."
                });
            }

            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O identificador do produto é inválido."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                return BadRequest(new
                {
                    mensagem = "O nome do produto é obrigatório."
                });
            }

            if (dto.Preco <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O preço do produto deve ser maior que zero."
                });
            }

            if (dto.IdCategoria <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "A categoria do produto é obrigatória."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.EnderecoImagem))
            {
                return BadRequest(new
                {
                    mensagem = "O endereço da imagem é obrigatório."
                });
            }

            var produto = new Produtos
            {
                Nome = dto.Nome.Trim(),
                Preco = dto.Preco,
                EnderecoImagem = dto.EnderecoImagem.Trim(),
                IdCategoria = dto.IdCategoria,
                DescricaoCurta = dto.DescricaoCurta?.Trim(),
                DescricaoLonga = dto.DescricaoLonga?.Trim(),
                Disponibilidade = dto.Disponibilidade,
                Situacao = dto.Situacao
            };

            await _produto.Atualizar(id, produto);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] string? nome,
            [FromQuery] int? categoria,
            [FromQuery] decimal? precoMin,
            [FromQuery] decimal? precoMax)
        {
            if (precoMin.HasValue && precoMin < 0)
            {
                return BadRequest(new
                {
                    mensagem = "O preço mínimo não pode ser negativo."
                });
            }

            if (precoMax.HasValue && precoMax < 0)
            {
                return BadRequest(new
                {
                    mensagem = "O preço máximo não pode ser negativo."
                });
            }

            if (precoMin.HasValue &&
                precoMax.HasValue &&
                precoMin > precoMax)
            {
                return BadRequest(new
                {
                    mensagem = "O preço mínimo não pode ser maior que o preço máximo."
                });
            }

            if (categoria.HasValue && categoria <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "A categoria informada é inválida."
                });
            }

            var produtos = await _produto.Listar(
                nome,
                categoria,
                precoMin,
                precoMax
            );

            return Ok(produtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O identificador do produto é inválido."
                });
            }

            var produto = await _produto.BuscarPorId(id);

            if (produto == null)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado."
                });
            }

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Perfil.Administrador)]
        public async Task<IActionResult> Deletar(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O identificador do produto é inválido."
                });
            }

            var resultado = await _produto.Deletar(id);

            if (!resultado)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado."
                });
            }

            return NoContent();
        }
    }
}