using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaHistoricoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TarefaHistoricoController(AppDbContext context)
        {
            _context = context;
        }

         [HttpGet("tarefa/{tarefaId}")]
      public async Task<ActionResult<IEnumerable<TarefaHistorico>>> GetHistoricoPorTarefa(int tarefaId)
      {
          var historicos = await _context.TarefasHistoricos
                                         .Where(h => h.TarefaId == tarefaId)
                                         .OrderByDescending(h => h.DataModificacao)
                                         .ToListAsync();
      
          if (historicos == null || !historicos.Any())
              return NotFound("Nenhum histórico encontrado para esta tarefa.");
      
          return historicos;
      }
    }
}

