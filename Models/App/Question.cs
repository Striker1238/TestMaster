using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestMaster.Models.DB;

namespace TestMaster.Models.App
{
    public class Question : INotifyPropertyChanged
    {
        public int TestId { get; set; }
        private string text;
        public string Text { get => text; set { text = value; OnPropertyChanged(); } }
        public ObservableCollection<Answer> Answers { get; set; }
        public ObservableCollection<int> CorrectAnswerIndexes { get; set; }
        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged();
                }
            }
        }
        public int GetId { get => Answers.FirstOrDefault().QuestionId; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
