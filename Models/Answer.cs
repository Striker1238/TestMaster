using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace TestMaster.Models
{
    public class Answer : INotifyPropertyChanged
    {
        private string text;
        private bool isSelected;
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get => text; set { text = value; OnPropertyChanged(); } }
        [NotMapped]
        public bool IsSelected { get => isSelected; set { isSelected = value; OnPropertyChanged(); } }
        public Question Question { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
