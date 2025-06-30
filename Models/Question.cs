using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestMaster.Models
{
    public class Question : IQuestion
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Text { get; set; }
        public List<Answer> Answers { get; set; }
        public List<int> CorrectAnswerIndexes { get; set; }
        public Test Test { get; set; }
    }
}