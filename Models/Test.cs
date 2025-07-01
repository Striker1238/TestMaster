using System.Collections.Generic;

namespace TestMaster.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<Question> Questions { get; set; }
        public int NumberQuestions { get; set; }
        public bool IsShuffleQuestions { get; set; }
        public bool IsShuffleAnswers { get; set; }
    }
}
