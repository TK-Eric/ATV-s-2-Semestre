using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstituicaoController : ControllerBase
    {
        private readonly IInstituicao _instituicao;

        public InstituicaoController(IInstituicao instituicao)
        {
            _instituicao = instituicao;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var instituicaoBuscada = await _instituicao.BuscarPorId(id);

            if (instituicaoBuscada == null)
                return NotFound("Instituição não encontrada.");

            return Ok(instituicaoBuscada);
        }

        /// <summary>
        /// Lista todas as instituições
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var instituicoes = await _instituicao.Listar();
                return Ok(instituicoes);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] InstituicaoDTO dto)
        {
            var instituicao = new Instituicao
            {
                NomeFantasia = dto.NomeFantasia
            };

            await _instituicao.Cadastrar(instituicao);

            return StatusCode(201, instituicao);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] InstituicaoDTO dto)
        {
            var instituicao = new Instituicao
            {
                NomeFantasia = dto.NomeFantasia
            };

            // Corrigido: passa o objeto 'instituicao' e retorna ele no Ok()
            await _instituicao.Atualizar(id, instituicao);

            return Ok(instituicao);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _instituicao.Deletar(id);
            return NoContent();
        }
    }
}