namespace questionBank.Application.Models
{
    public class AcademicClass: CommonProps
    {
        public string ClassName { get; set; } = string.Empty;
        public ICollection<AcademicSubject>? AcademicSubjects { get; set; }
    }
}
