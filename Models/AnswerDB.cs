using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace TestMaster.Models
{
    public class AnswerDB : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        private string text;
        public string Text { get => text; set { text = value; OnPropertyChanged(); } }
        private bool isSelected;
        [NotMapped]
        public bool IsSelected { get => isSelected; set { isSelected = value; OnPropertyChanged(); } }
        public QuestionDB Question { get; set; }

        //private bool isCorrect;
        [NotMapped]
        public bool IsCorrect => Question?.CorrectAnswerIndexes?.Contains(Id) == true;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
