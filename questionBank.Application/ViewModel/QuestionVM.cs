using questionBank.Application.Models;

namespace questionBank.Application.ViewModel
{
    public class QuestionVM
    {
        public string Uddipok { get; set; } = String.Empty;
        public IFormFile? Image { get; set; }
        public string? ImagePosition { get; set; }
        public List<QuestionDetail>? QuestionDetails { get; set; } 
        public int ChapterId { get; set; }

        public int AcademicSubjectId { get; set; }
        public int AcademicClassId { get; set; }

    }
}
