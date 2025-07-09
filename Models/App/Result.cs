using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TestMaster.Models.App
{
    public class Result : INotifyPropertyChanged
    {
        public string FullName { get; set; }
        public string PersonnelNumber { get; set; }
        public Test ComplatedTest { get; set; }
        public ObservableCollection<QuestionResult> QuestionResult { get; set; }
        public int CountQuestions { get; set; }
        public int CountCorrectAnswer { get; set; }
        public bool IsSuccessfully { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T field, T value, string propertyName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
