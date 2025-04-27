﻿using TaskManagerAPI.Models.enums;

namespace TaskManagerAPI.Dtos
{
    public class TarefaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string Status { get; set; } = string.Empty;

        public string Prioridade { get; set; } = string.Empty;
    }
}
