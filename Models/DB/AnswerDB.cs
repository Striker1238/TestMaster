using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace TestMaster.Models.DB
{
    public class AnswerDB 
    {
        public int Id { get; set; }
        public QuestionDB Question { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
    }
}
