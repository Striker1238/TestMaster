using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using TestMaster.Commands;
using TestMaster.Models;
using TestMaster.Services;
using TestMaster.Views;

namespace TestMaster.ViewModels
{
    public class TestCreatorViewModel : INotifyPropertyChanged
    {
        public ICommand CreateNewTestCommand  { get; }
        public ICommand EditTestCommand { get; }
        public ICommand DeleteTestCommand { get; }

        private TestDB _selectedTest;
        public TestDB SelectedTest
        {
            get => _selectedTest;
            set => SetProperty(ref _selectedTest, value);
        }
        public ObservableCollection<TestDB> Tests { get; set; } = new();


        public TestCreatorViewModel()
        {
            CreateNewTestCommand = new RelayCommand(_ => CreateNewTest(), _ => true);
            EditTestCommand = new RelayCommand(_ => EditTest(), _ => true);
            DeleteTestCommand = new RelayCommand(_ => DeleteTest(), _ => true);

            using var db = new DatabaseConnectionService();
            var tests = db.tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .ToList();

            Tests = new ObservableCollection<TestDB>(tests);
        }

        private void DeleteTest()
        {
            if (SelectedTest == null)
            {
                MessageBox.Show("Выберите тест для удаления!", "Внимание");
                return;
            }
            using var db = new DatabaseConnectionService();
            db.Remove(SelectedTest);
            Tests.Remove(SelectedTest);

            //Добавить подтверждение удаления
            db.SaveChanges();
        }
        private void EditTest()
        {
            if (SelectedTest == null)
            {
                MessageBox.Show("Выберите тест для редактирования!", "Внимание");
                return;
            }

            OpenEditPage(false);
        }
        public void CreateNewTest()
        {
            OpenEditPage(true);
        }

        private void OpenEditPage(bool isForNewTest)
        {
            var editPage = new TestCreatorEditPage(isForNewTest?null: SelectedTest);

            // Поиск активного окна, в котором есть Frame с именем TestCreator
            var mainWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            if (mainWindow != null)
            {
                // Поиск Frame по имени TestCreator
                var frame = FindFrameByName(mainWindow, "TestCreator");
                if (frame != null)
                {
                    frame.Navigate(editPage);
                    return;
                }
                // Если не найден Frame, пробуем стандартную навигацию
                if (mainWindow is NavigationWindow navWindow)
                {
                    navWindow.Navigate(editPage);
                    return;
                }
                else if (mainWindow.Content is Frame contentFrame)
                {
                    contentFrame.Navigate(editPage);
                    return;
                }
            }
            MessageBox.Show("Возникла непредвиденная ошибка, обратитесь к разработчикам.", "Внимание!");
        }
        private Frame? FindFrameByName(DependencyObject parent, string frameName)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Frame frame && frame.Name == frameName)
                    return frame;
                var result = FindFrameByName(child, frameName);
                if (result != null)
                    return result;
            }
            return null;
        }


        


        public event PropertyChangedEventHandler? PropertyChanged;
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
