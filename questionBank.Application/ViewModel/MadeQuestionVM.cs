using questionBank.Application.Models;

namespace questionBank.Application.ViewModel
{
    public class MadeQuestionVM
    {
        public string InstituteName { get; set; } = string.Empty;
        public string ExamTypeName { get; set; } = string.Empty;
        public DateOnly dateOnly { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int TotalMark { get; set; }
        public double Time { get; set; }
        public List<Question> Questions { get; set; }
    }
}
