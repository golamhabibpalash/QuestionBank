namespace questionBank.Application.Models
{
    public class Question : CommonProps
    {
        public string Uddipok { get; set; } = String.Empty;
        public byte[]? Image { get; set; } 
        public string? ImagePosition { get; set; }
        public virtual ICollection<QuestionDetail>? QuestionDetails { get; set; }
        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; } = new Chapter();

    }
}
