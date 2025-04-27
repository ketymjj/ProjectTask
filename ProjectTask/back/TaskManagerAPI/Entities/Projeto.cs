namespace TaskManagerAPI.Entities
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        
        // Lista de tarefas do projeto
        public ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
    }
}
