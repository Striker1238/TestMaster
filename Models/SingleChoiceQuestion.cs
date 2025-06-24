using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestMaster.Models
{
    public class SingleChoiceQuestion : AbstractQuestion
    {
        public ObservableCollection<Answer> Answers { get; set; } = new ObservableCollection<Answer>();
        public int CorrectAnswerIndex { get; set; }
        public bool IsAnswerCorrect()
        {
            return Answers.Any(a => a.IsSelected && a.IsCorrect);
        }
    }
}