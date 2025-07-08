using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestMaster.Commands;
using TestMaster.Models.App;
using TestMaster.Services;

namespace TestMaster.ViewModels
{
    public class ResultViewModel : INotifyPropertyChanged
    {
        public ICommand ExportCommand { get; set; }
        public Result result;

        public ResultViewModel(Result result)
        {
            ExportCommand = new RelayCommand(_ => ExportTest(), _ => true);
            this.result = result;
        }

        public void ExportTest()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                FileName = result.FullName.Replace(' ','_')
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                ExcelFileService.ExportResultToExcel(result, saveFileDialog.FileName);
            }
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
