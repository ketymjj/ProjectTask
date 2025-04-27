using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Dtos;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models.enums;

[ApiController]
[Route("api/[controller]")]
public class ProjetosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjetosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjetoDto>>> GetProjetos()
    {
        var projetos = await _context.Projects.ToListAsync();
        var projetosDto = projetos.Select(p => new ProjetoDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            DataCriacao = p.DataCriacao
        }).ToList();

        return Ok(projetosDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjetoDto>> GetProjeto(int id)
    {
        var projeto = await _context.Projects.FindAsync(id);

        if (projeto == null)
            return NotFound();

        var projetoDto = new ProjetoDto
        {
            Id = projeto.Id,
            Nome = projeto.Nome,
            Descricao = projeto.Descricao,
            DataCriacao = projeto.DataCriacao
        };

        return Ok(projetoDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProjetoDto>> PostProjeto(CreateProjetoDto dto)
    {
        var projeto = new Projeto
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            DataCriacao = DateTime.UtcNow
        };

        _context.Projects.Add(projeto);
        await _context.SaveChangesAsync();

        var projetoDto = new ProjetoDto
        {
            Id = projeto.Id,
            Nome = projeto.Nome,
            Descricao = projeto.Descricao,
            DataCriacao = projeto.DataCriacao
        };

        return CreatedAtAction(nameof(GetProjeto), new { id = projetoDto.Id }, projetoDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProjeto(int id, ProjetoDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID da URL não confere com o do corpo.");

        var projeto = await _context.Projects.FindAsync(id);
        if (projeto == null)
            return NotFound();

        projeto.Nome = dto.Nome;
        projeto.Descricao = dto.Descricao;
        projeto.DataCriacao = dto.DataCriacao;


        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProjeto(int id)
         {
         var projeto = await _context.Projects
             .Include(p => p.Tarefas) // Inclui as tarefas relacionadas
             .FirstOrDefaultAsync(p => p.Id == id);
     
         if (projeto == null)
             return NotFound(new { message = "Projeto não encontrado." });
     
         bool temTarefasPendentes = projeto.Tarefas.Any(t => 
            t.Status == StatusTarefa.Pendente.ToString() || 
            t.Status == StatusTarefa.EmProgresso.ToString());
     
         if (temTarefasPendentes)
             return BadRequest(new { message = "Projeto não pode ser excluído pois possui tarefas pendentes ou em progresso." });
     
         _context.Projects.Remove(projeto);
         await _context.SaveChangesAsync();
     
         return NoContent();
    }

    [HttpGet("projeto/{projetoId}")]
    public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefasPorProjeto(int projetoId)
    {
        return await _context.Tasks
                             .Where(t => t.ProjetoId == projetoId)
                             .ToListAsync();
    }
}
