using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestMaster.Models.DB;

namespace TestMaster.Models.App
{
    public class Answer : INotifyPropertyChanged
    {
        public int QuestionId { get; set; }

        private string text;
        public string Text { get => text; set { text = value; OnPropertyChanged(); } }

        private bool isSelected;
        public bool IsSelected { get => isSelected; set { isSelected = value; OnPropertyChanged(); } }

        public bool IsCorrect;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
