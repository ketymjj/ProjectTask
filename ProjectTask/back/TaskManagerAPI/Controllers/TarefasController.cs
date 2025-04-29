using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Dtos;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models.enums;


namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TarefasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/tarefas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefas()
        {
            return await _context.Tasks.Include(t => t.Projeto).ToListAsync();
        }

        // GET: api/tarefas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarefa>> GetTarefa(int id)
        {
            var tarefa = await _context.Tasks.Include(t => t.Projeto)
                                               .FirstOrDefaultAsync(t => t.Id == id);

            if (tarefa == null)
                return NotFound();

            return tarefa;
        }

        // POST: api/tarefas
        [HttpPost]
        public async Task<ActionResult<TarefaDto>> PostTarefa(CreateTarefaDto dto)
        {
            var projetoExiste = await _context.Projects
              .Include(p => p.Tarefas)
              .FirstOrDefaultAsync(p => p.Id == dto.ProjetoId);

          if (projetoExiste == null)
          return BadRequest("Projeto informado não existe.");

            if (projetoExiste.Tarefas.Count == 20)
            return BadRequest("O projeto já possui o limite máximo de 20 tarefas.");

            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                DataCriacao = DateTime.UtcNow,
                DataConclusao = dto.DataConclusao,
                Status =  StatusTarefa.Pendente.ToString(),
                Prioridade =dto.Prioridade.ToString(), 
                ProjetoId = dto.ProjetoId
            };

            _context.Tasks.Add(tarefa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTarefa), new { id = tarefa.Id }, tarefa);
        }

        // PUT: api/tarefas/5
      [HttpPut("{id}")]
      public async Task<IActionResult> PutTarefa(int id, TarefaDto dto)
      {
          if (id != dto.Id)
              return BadRequest("ID da URL diferente do corpo da requisição.");
      
          var tarefa = await _context.Tasks.FindAsync(id);
          if (tarefa == null)
              return NotFound();
      
          if (dto.Prioridade != tarefa.Prioridade)
              return BadRequest("Não é permitido alterar a prioridade da tarefa após sua criação.");
      
          // Lista para armazenar históricos de mudanças
          var historicos = new List<TarefaHistorico>();
      
          // Verifica cada campo que pode ter mudado
          if (tarefa.Titulo != dto.Titulo)
          {
              historicos.Add(new TarefaHistorico
              {
                  TarefaId = tarefa.Id,
                  CampoAlterado = "Titulo",
                  ValorAnterior = tarefa.Titulo,
                  ValorNovo = dto.Titulo,
                  DataModificacao = DateTime.UtcNow
                  
              });
              tarefa.Titulo = dto.Titulo;
          }
      
          if (tarefa.Descricao != dto.Descricao)
          {
              historicos.Add(new TarefaHistorico
              {
                  TarefaId = tarefa.Id,
                  CampoAlterado = "Descricao",
                  ValorAnterior = tarefa.Descricao,
                  ValorNovo = dto.Descricao,
                  DataModificacao = DateTime.Now
              });
              tarefa.Descricao = dto.Descricao;
          }
      
          if (tarefa.DataConclusao != dto.DataConclusao)
          {
              historicos.Add(new TarefaHistorico
              {
                  TarefaId = tarefa.Id,
                  CampoAlterado = "DataConclusao",
                  ValorAnterior = tarefa.DataConclusao?.ToString("o"), // formato ISO
                  ValorNovo = dto.DataConclusao?.ToString("o"),
                  DataModificacao = DateTime.UtcNow
              });
              tarefa.DataConclusao = dto.DataConclusao;
          }
      
          if (tarefa.Status != dto.Status)
          {
              historicos.Add(new TarefaHistorico
              {
                  TarefaId = tarefa.Id,
                  CampoAlterado = "Status",
                  ValorAnterior = tarefa.Status,
                  ValorNovo = dto.Status,
                  DataModificacao = DateTime.UtcNow
              });
              tarefa.Status = dto.Status;
          }
      
          tarefa.DataCriacao = DateTime.UtcNow; // Isso não deveria mudar numa atualização, mas se quiser manter...
      
          try
          {
              // Salva as alterações da tarefa
              await _context.SaveChangesAsync();
      
              // Se houver histórico para registrar
              if (historicos.Any())
              {
                  _context.TarefasHistoricos.AddRange(historicos);
                  await _context.SaveChangesAsync();
              }
          }
          catch (DbUpdateConcurrencyException)
          {
              return StatusCode(500, "Erro ao atualizar a tarefa.");
          }
      
          return NoContent();
       }

        // DELETE: api/tarefas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarefa(int id)
        {
            var tarefa = await _context.Tasks.FindAsync(id);
            if (tarefa == null)
                return NotFound();

            _context.Tasks.Remove(tarefa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
