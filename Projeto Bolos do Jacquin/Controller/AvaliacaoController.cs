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

        // =========================================================
        // CRIAR AVALIAÇÃO
        // SOMENTE CLIENTE AUTENTICADO
        // =========================================================

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

            // Obtém o usuário EXCLUSIVAMENTE do JWT
            var usuarioIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return Unauthorized(new
                {
                    mensagem = "Usuário não autenticado ou token inválido."
                });
            }

            // Impede duas avaliações do mesmo cliente para o mesmo produto
            var avaliacaoExistente =
                await _avaliacao.ExistePorUsuarioEProduto(
                    usuarioId,
                    dto.IdProdutos);

            if (avaliacaoExistente)
            {
                return Conflict(new
                {
                    mensagem =
                        "Você já possui uma avaliação para este produto. " +
                        "Edite a avaliação existente."
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

                // Nova avaliação começa publicada.
                Situacao = "PUBLICADA"
            };

            await _avaliacao.Cadastrar(avaliacao);

            return StatusCode(201, avaliacao);
        }

        // =========================================================
        // EDITAR PRÓPRIA AVALIAÇÃO
        // SOMENTE CLIENTE AUTENTICADO
        // =========================================================

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

            // Impede alteração da avaliação de outro cliente.
            if (avaliacaoExistente.IdUsuarios != usuarioId)
            {
                return Forbid();
            }

            // O cliente pode alterar somente nota e comentário.
            // Produto e usuário NÃO podem ser alterados.
            avaliacaoExistente.Nota = (int)dto.Nota;

            avaliacaoExistente.Comentario =
                string.IsNullOrWhiteSpace(dto.Comentario)
                    ? null
                    : dto.Comentario.Trim();

            avaliacaoExistente.DataAlteracao = DateTime.UtcNow;

            await _avaliacao.Atualizar(id, avaliacaoExistente);

            return NoContent();
        }

        // =========================================================
        // LISTAGEM PÚBLICA POR PRODUTO
        // SOMENTE AVALIAÇÕES PUBLICADAS
        // =========================================================

        [HttpGet("produto/{produtoId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> ListarPorProduto(int produtoId)
        {
            if (produtoId <= 0)
            {
                return BadRequest(new
                {
                    mensagem = "O identificador do produto é inválido."
                });
            }

            var avaliacoes =
                await _avaliacao.ListarPublicadasPorProduto(produtoId);

            return Ok(avaliacoes);
        }

        // =========================================================
        // LISTAR PRÓPRIAS AVALIAÇÕES
        // SOMENTE CLIENTE AUTENTICADO
        // =========================================================

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

        // =========================================================
        // EXCLUIR PRÓPRIA AVALIAÇÃO
        // SOMENTE CLIENTE AUTENTICADO
        // =========================================================

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

            // Impede exclusão da avaliação de outro cliente.
            if (avaliacao.IdUsuarios != usuarioId)
            {
                return Forbid();
            }

            await _avaliacao.Deletar(id);

            return NoContent();
        }
    }
}