namespace TaskManagerAPI.Entities
{
    public class TarefaHistorico
    {
         public int Id { get; set; }
        public int TarefaId { get; set; }
        public Tarefa Tarefa { get; set; }
        
        public string CampoAlterado { get; set; }  = string.Empty;
        public string ValorAnterior { get; set; }  = string.Empty;
        public string ValorNovo { get; set; }  = string.Empty;
        
        public DateTime DataModificacao { get; set; }
        public string UsuarioModificador { get; set; }  = string.Empty;
    }
}