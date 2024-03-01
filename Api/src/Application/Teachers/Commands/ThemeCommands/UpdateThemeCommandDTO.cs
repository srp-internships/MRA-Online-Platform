﻿namespace Application.Teachers.Commands.ThemeCommands
{
    public record UpdateThemeCommandDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
