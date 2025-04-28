using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models;


namespace TaskManagerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Projeto> Projects { get; set; }
        public DbSet<Tarefa> Tasks { get; set; }
        public DbSet<TarefaHistorico> TarefasHistoricos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Descricao).IsRequired();
                entity.Property(t => t.Status).IsRequired();
                entity.HasOne(t => t.Projeto)
                      .WithMany(p => p.Tarefas)
                      .HasForeignKey(t => t.ProjetoId);
            });

        }
    }
}
