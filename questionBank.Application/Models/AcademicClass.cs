namespace questionBank.Application.Models
{
    public class AcademicClass
    {
        public string ClassName { get; set; } = string.Empty;
        public ICollection<AcademicSubject> AcademicSubjects { get; set; } = new List<AcademicSubject>();
    }
}
