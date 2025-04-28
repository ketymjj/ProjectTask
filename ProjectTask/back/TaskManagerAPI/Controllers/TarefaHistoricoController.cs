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

        // GET: api/tarefahistorico/tarefa/5
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

        // POST: api/tarefahistorico
        [HttpPost]
        public async Task<ActionResult<TarefaHistorico>> PostHistorico(TarefaHistorico historico)
        {
            var tarefaExiste = await _context.Tasks.AnyAsync(t => t.Id == historico.TarefaId);
            if (!tarefaExiste)
                return BadRequest("Tarefa não encontrada.");

            historico.DataModificacao = DateTime.UtcNow;

            _context.TarefasHistoricos.Add(historico);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHistoricoPorTarefa), new { tarefaId = historico.TarefaId }, historico);
        }
    }
}
