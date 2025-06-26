using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestMaster.Models;

namespace TestMaster.ViewModels
{
    public class TestRunnerViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IQuestion> Questions { get; set; }
        public IQuestion CurrentQuestion { get; set; }

        public TestRunnerViewModel()
        {
            LoadTest();
        }

        public void LoadTest()
        {
            //var loaded = _testService.LoadTest();
            //Questions = new ObservableCollection<IQuestion>(loaded);
            CurrentQuestion = Questions.FirstOrDefault();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

}
