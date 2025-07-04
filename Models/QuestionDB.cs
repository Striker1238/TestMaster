using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestMaster.Models
{
    public class QuestionDB : IQuestion
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Text { get; set; }
        public List<AnswerDB> Answers { get; set; }
        public List<int> CorrectAnswerIndexes { get; set; }
        public TestDB Test { get; set; }

        
    }
    public enum QuestionType
    {
        Single,
        Multiple,
        Text
    }
}