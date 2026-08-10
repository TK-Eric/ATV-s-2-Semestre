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
    public class EventoController : ControllerBase
    {
        private readonly ITipoUsuario _tipoUsuario;


        public EventoController(IEvento tipoEvento)
        {
            _tipoUsuario = tipoEvento;
        }
        [HttpGet("{id:guid}")]

        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var tipoUsuarioBuscado = await _tipoUsuario.BuscarPorId(id);

            if (tipoUsuarioBuscado == null)
                return NotFound("Tipo Usuario nao encontrado.");

            return Ok(tipoUsuarioBuscado);
        }


        [HttpGet]
        //<summary>
        //Lista todos os perfiz e usuario
        //</summary>

        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _tipoUsuario.Listar();

                return Ok(tipos);

            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }
        /// <summary>
        /// cadastra um novo perfil de usuario
        /// </summary>
        /// <param name="tipoUsuario">Perfil do usuario a ser cadstrado</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] TipoUsuarioDTO dto)
        {

            var tipoUsuario = new TipoUsuario
            {
                TituloTipoUsuario = dto.Titulo
            };

            await _tipoUsuario.Cadastrar(tipoUsuario);

            return StatusCode(201, tipoUsuario);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id,
            [FromBody] TipoUsuarioDTO dto)
        {
            var tipoUsuario = new TipoUsuario
            {
                TituloTipoUsuario = dto.Titulo
            };

            await _tipoUsuario.Atualizar(id, tipoUsuario);

            return Ok(tipoUsuario);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _tipoUsuario.Deletar(id);
            return NoContent();
        }

    }
}