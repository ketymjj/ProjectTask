namespace TaskManagerAPI.Entities
{
    public class TarefaHistorico
    {
         public int Id { get; set; }
         public int TarefaId { get; set; }
         public Tarefa Tarefa { get; set; }
     
         public string CampoAlterado { get; set; }
         public string ValorAnterior { get; set; }
         public string ValorNovo { get; set; }
     
         public DateTime DataModificacao { get; set; }
    }
}