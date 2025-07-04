using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using TestMaster.Models.App;

namespace TestMaster.Models.DB
{
    public class QuestionDB 
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Text { get; set; }
        public List<AnswerDB> Answers { get; set; }
        public List<int> CorrectAnswerIndexes { get; set; }
        public TestDB Test { get; set; }
        public QuestionType Type { get; set; }
    }
}