using System.Text.Json.Serialization;
using TaskManagerAPI.Models.enums;

namespace TaskManagerAPI.Entities
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow.Date;
        public DateTime? DataConclusao { get; set; }

        // Novo campo
        public string Status { get; set; } = string.Empty;

        public string Prioridade { get; set; } = string.Empty;

        // Relacionamento com Projeto
        public int ProjetoId { get; set; }

        [JsonIgnore] // Evita o ciclo
        public Projeto Projeto { get; set; } 

         public ICollection<TarefaHistorico> Historicos { get; set; }

    }
}
