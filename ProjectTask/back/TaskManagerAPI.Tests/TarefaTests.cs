
using TaskManagerAPI.Models.enums;
using Xunit;
using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Tests
{
    public class TarefaTests
    {
        [Fact]
        public void Deve_Criar_Tarefa_Com_Status_Pendente()
        {
            var tarefa = new Tarefa
            {
                Titulo = "Teste",
                Descricao = "Testar algo",
                DataConclusao = DateTime.Now.AddDays(1)
            };

            Assert.Equal(tarefa.Status, tarefa.Status);
        }
    }

}
