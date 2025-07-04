using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using TestMaster.Commands;
using TestMaster.Models.App;
using TestMaster.Models.DB;
using TestMaster.Services;
using TestMaster.Views;

namespace TestMaster.ViewModels
{
    public class TestCreatorViewModel : INotifyPropertyChanged
    {
        public ICommand CreateNewTestCommand  { get; }
        public ICommand EditTestCommand { get; }
        public ICommand DeleteTestCommand { get; }
        public ICommand AddIndividualTestCommand { get; }
        public ICommand EditIndividualTestCommand { get; }
        public ICommand DeleteIndividualTestCommand { get; }


        private Test _selectedTest;
        public Test SelectedTest
        {
            get => _selectedTest;
            set => SetProperty(ref _selectedTest, value);
        }
        public ObservableCollection<Test> Tests { get; set; } = new();

        private IndividualTest _selectedIndividualTest;
        public IndividualTest SelectedIndividualTest
        {
            get => _selectedIndividualTest;
            set => SetProperty(ref _selectedIndividualTest, value);
        }
        public ObservableCollection<IndividualTest> IndividualTests { get; set; } = new(); 


        public TestCreatorViewModel()
        {
            CreateNewTestCommand = new RelayCommand(_ => CreateNewTest(), _ => true);
            EditTestCommand = new RelayCommand(_ => EditTest(), _ => true);
            DeleteTestCommand = new RelayCommand(_ => DeleteTest(), _ => true);
            AddIndividualTestCommand = new RelayCommand(_ => CreateIndividualTest(), _ => true);
            EditIndividualTestCommand = new RelayCommand(_ => EditIndividualTest(), _ => true);
            DeleteIndividualTestCommand = new RelayCommand(_ => DeleteIndividualTest(), _ => true);

            using var db = new DatabaseConnectionService();
            var dbTests = db.tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .ToList();

            var tests = dbTests
                .Select(t => ModelMapper.ToAppModel(t))
                .ToList();

            Tests = new ObservableCollection<Test>(tests);
        }
        // ===== TestCommands =====
        private void DeleteTest()
        {
            if (SelectedTest == null)
            {
                MessageBox.Show("Выберите тест для удаления!", "Внимание");
                return;
            }
            using var db = new DatabaseConnectionService();
            db.tests.Remove(ModelMapper.ToDbModel(SelectedTest));
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

        // ===== TestCommands =====
        private void DeleteIndividualTest()
        {
            if (SelectedIndividualTest == null)
            {
                MessageBox.Show("Выберите индивидуальный тест для удаления!", "Внимание");
                return;
            }
            using var db = new DatabaseConnectionService();
            db.individualTests.Remove(ModelMapper.ToDbModel(SelectedIndividualTest));
            IndividualTests.Remove(SelectedIndividualTest);
        }
        private void EditIndividualTest()
        {
            if (SelectedIndividualTest == null)
            {
                MessageBox.Show("Выберите индивидуальный тест для редактирования!", "Внимание");
                return;
            }
            // Открытие страницы редактирования индивидуального теста

        }
        private void CreateIndividualTest()
        {
            // Открытие страницы создания индивидуального теста
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
