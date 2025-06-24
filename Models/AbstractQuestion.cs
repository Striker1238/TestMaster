using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestMaster.Models
{
    public abstract class AbstractQuestion : IQuestion, INotifyPropertyChanged
    {
        public string Text { get; set; }
        public bool IsAnswerCorrect { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}