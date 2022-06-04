namespace questionBank.Application.Models
{
    public class Chapter : CommonProps
    {
        public string ChapterName { get; set; } = string.Empty;
        public int ChapterNumber { get; set; }
        public int AcademicSubjectId { get; set; }
        public AcademicSubject AcademicSubject { get; set; } = new AcademicSubject();

        public ICollection<Question> Questions { get; set; }
    }
}
