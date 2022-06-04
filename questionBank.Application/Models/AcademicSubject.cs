﻿namespace questionBank.Application.Models
{
    public class AcademicSubject : CommonProps
    {
        public string SubjectName { get; set; } = string.Empty;
        public int AcademicClassId { get; set; }
        public AcademicClass AcademicClass { get; set; } = new AcademicClass();
    }
}
