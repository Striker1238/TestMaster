using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestMaster.Models
{
    public class TestDB
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<QuestionDB> Questions { get; set; }
        public int NumberQuestions { get; set; }
        public int CorrectAnswersCount { get; set; }
        public bool IsShuffleQuestions { get; set; }
        public bool IsShuffleAnswers { get; set; }
    }
}
