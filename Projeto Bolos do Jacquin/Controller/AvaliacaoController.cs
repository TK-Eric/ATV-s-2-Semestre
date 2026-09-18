using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Bolos_do_Jacquin.Constants;
using Projeto_Bolos_do_Jacquin.DTO;
using Projeto_Bolos_do_Jacquin.Interfaces;
using Projeto_Bolos_do_Jacquin.Models;
using System.Security.Claims;

namespace Projeto_Bolos_do_Jacquin.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacao _avaliacao;

        public AvaliacaoController(IAvaliacao avaliacao)
        {
            _avaliacao = avaliacao;
        }

        // POST: api/Avaliacao
        [HttpPost]
        [Authorize(Roles = Perfil.Cliente)]
        public async Task<IActionResult> Cadastrar(
            [FromBody] AvaliacaoDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    mensagem = "Os dados da avaliação são obrigatórios."
                });
            }

            if (dto.IdProdutos <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O produto informado é inválido."
                });
            }

            if ((int)dto.Nota < 1 || (int)dto.Nota > 5)
            {
                return BadRequest(new
                {
                    mensagem = "A nota deve estar entre 1 e 5."
                });
            }

            var usuarioIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return Unauthorized(new
                {
                    mensagem = "Usuário não autenticado ou token inválido."
                });
            }

            var avaliacao = new Avaliacoes
            {
                Nota = (int)dto.Nota,

                Comentario = string.IsNullOrWhiteSpace(dto.Comentario)
                    ? null
                    : dto.Comentario.Trim(),

                DataCriacao = DateTime.UtcNow,
                IdProdutos = dto.IdProdutos,
                IdUsuarios = usuarioId,

                Situacao = "Publicada",
                Exibe = true
            };

            await _avaliacao.Cadastrar(avaliacao);

            return StatusCode(201, avaliacao);
        }

        // PUT: api/Avaliacao/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = Perfil.Cliente)]
        public async Task<IActionResult> Atualizar(
            int id,
            [FromBody] AvaliacaoDTO dto)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O identificador da avaliação é inválido."
                });
            }

            if (dto == null)
            {
                return BadRequest(new
                {
                    mensagem = "Os dados da avaliação são obrigatórios."
                });
            }

            if (dto.IdProdutos <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O produto informado é inválido."
                });
            }

            if ((int)dto.Nota < 1 || (int)dto.Nota > 5)
            {
                return BadRequest(new
                {
                    mensagem = "A nota deve estar entre 1 e 5."
                });
            }

            var usuarioIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return Unauthorized(new
                {
                    mensagem = "Usuário não autenticado ou token inválido."
                });
            }

            var avaliacaoExistente =
                await _avaliacao.BuscarPorId(id);

            if (avaliacaoExistente == null)
            {
                return NotFound(new
                {
                    mensagem = "Avaliação não encontrada."
                });
            }

            if (avaliacaoExistente.IdUsuarios != usuarioId)
            {
                return Forbid();
            }

            var avaliacaoAtualizada = new Avaliacoes
            {
                Nota = (int)dto.Nota,

                Comentario = string.IsNullOrWhiteSpace(dto.Comentario)
                    ? null
                    : dto.Comentario.Trim(),

                IdProdutos = avaliacaoExistente.IdProdutos
            };

            await _avaliacao.Atualizar(
                id,
                avaliacaoAtualizada);

            return NoContent();
        }

        // GET: api/Avaliacao/produto/{produtoId}
        [HttpGet("produto/{produtoId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> ListarPorProduto(
            int produtoId)
        {
            if (produtoId <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O identificador do produto é inválido."
                });
            }

            var avaliacoes =
                await _avaliacao.ListarPorProduto(produtoId);

            return Ok(avaliacoes);
        }

        // GET: api/Avaliacao/minhas
        [HttpGet("minhas")]
        [Authorize(Roles = Perfil.Cliente)]
        public async Task<IActionResult> MinhasAvaliacoes()
        {
            var usuarioIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return Unauthorized(new
                {
                    mensagem = "Usuário não autenticado ou token inválido."
                });
            }

            var avaliacoes =
                await _avaliacao.ListarPorUsuario(usuarioId);

            return Ok(avaliacoes);
        }

        // DELETE: api/Avaliacao/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = Perfil.Cliente)]
        public async Task<IActionResult> Deletar(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O identificador da avaliação é inválido."
                });
            }

            var usuarioIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return Unauthorized(new
                {
                    mensagem = "Usuário não autenticado ou token inválido."
                });
            }

            var avaliacao =
                await _avaliacao.BuscarPorId(id);

            if (avaliacao == null)
            {
                return NotFound(new
                {
                    mensagem = "Avaliação não encontrada."
                });
            }

            if (avaliacao.IdUsuarios != usuarioId)
            {
                return Forbid();
            }

            await _avaliacao.Deletar(id);

            return NoContent();
        }
    }
}