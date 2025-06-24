using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestMaster.Models
{
    public class Answer : INotifyPropertyChanged
    {
        private string text;
        private bool isSelected;
        private bool isCorrect;
        

        public string Text
        {
            get => text;
            set { text = value; OnPropertyChanged(); }
        }
        public bool IsSelected
        {
            get => isSelected;
            set { isSelected = value; OnPropertyChanged(); }
        }
        public bool IsCorrect { 
            get => isCorrect; 
            set { isCorrect = value; OnPropertyChanged(); } 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
