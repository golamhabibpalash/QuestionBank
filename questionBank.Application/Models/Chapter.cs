using System.ComponentModel.DataAnnotations;

namespace questionBank.Application.Models
{
    public class Chapter : CommonProps
    {
        [Display(Name ="Chapter Name")]
        public string ChapterName { get; set; } = string.Empty;

        [Display(Name = "Chapter Number")]
        public int ChapterNumber { get; set; }
        public int AcademicSubjectId { get; set; }
        public AcademicSubject? AcademicSubject { get; set; }

        public ICollection<Question>? Questions { get; set; }
    }
}
