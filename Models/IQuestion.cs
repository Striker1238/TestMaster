using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMaster.Models
{
    public interface IQuestion
    {
        string Text { get; set; }
        public List<AnswerDB> Answers { get; set; }
        public List<int> CorrectAnswerIndexes { get; set; }
    }
}
