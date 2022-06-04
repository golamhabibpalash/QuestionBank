namespace questionBank.Application.Models
{
    public class QuestionDetail : CommonProps
    {
        public string QuestionText { get; set; } = string.Empty;
        public int QuestionId { get; set; }
        public Question? Question { get; set; } 

        public int QuestionTypeId { get; set; }
        public QuestionType? QuestionType { get; set; } 
    }
}
