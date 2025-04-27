using TaskManagerAPI.Models.enums;

namespace TaskManagerAPI.Dtos
{
    public class CreateTarefaDto
    {


        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataConclusao { get; set; }

        public string Prioridade { get; set; } = string.Empty;

        public int ProjetoId { get; set; }
        public string Status { get; set; } = string.Empty;
        
    }
}
