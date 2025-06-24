namespace TestMaster.Models
{
    public class TextAnswerQuestion : AbstractQuestion
    {
        public string CorrectAnswer { get; set; }

        public bool ValidateAnswer(object userAnswer)
        {
            if (userAnswer is string answer)
                return answer.Trim().ToLower() == CorrectAnswer.Trim().ToLower();
            return false;
        }
    }
}