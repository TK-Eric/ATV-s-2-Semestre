using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    // DTO = "Data Transfer Object". É a classe que representa os dados
    // que chegam de fora (do formulário, no caso) pra dentro da API.
    // Ela é parecida com o Model "Evento", mas não é a mesma coisa:
    // o DTO existe só pra transportar/validar dados da requisição,
    // sem misturar com a estrutura real do banco
    public class EventDTO
    {
        // [Required] obriga o campo a ser preenchido — se vier vazio,
        // a API já barra automaticamente antes de chegar no controller
        // (lembra do [ApiController]? é ele que faz essa validação sozinho)
        [Required(ErrorMessage = "O nome do evento é obrigatório.")]
        // Limita o tamanho máximo do texto e define a mensagem de erro
        // caso passe do limite
        [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
        // "= string.Empty" evita que a propriedade comece como null
        public string NomeEvento { get; set; } = string.Empty;

        // Também obrigatório, sem limite de tamanho definido aqui
        [Required(ErrorMessage = "A descrição do evento é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        // Data obrigatória do evento
        [Required(ErrorMessage = "A data do evento é obrigatória.")]
        public DateTime DataEvento { get; set; }

        // Campo opcional (repare que não tem [Required]).
        // Provavelmente serve pra quando o evento JÁ tem uma imagem
        // (ex: numa edição, você manda a URL que já existe em vez de
        // subir o arquivo de novo)
        public string? ImagemUrl { get; set; }

        // O arquivo de imagem em si, vindo do formulário
        // (é esse aqui que o controller verifica com "dto.ArquivoImagem is not null"
        // pra decidir se sobe pro Cloudinary ou não). Opcional também
        public IFormFile? ArquivoImagem { get; set; }

        // Id opcional do tipo do evento (FK)
        public Guid? IdTipoEvento { get; set; }

        // Id opcional da instituição (FK)
        public Guid? IdInstituicao { get; set; }
    }
}