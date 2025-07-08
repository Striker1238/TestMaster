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
using System.Windows.Input;
using TestMaster.Commands;
using TestMaster.Models.App;
using TestMaster.Models.DB;
using TestMaster.Services;

namespace TestMaster.ViewModels
{
    class EditIndividualPageViewModel : INotifyPropertyChanged
    {
        public ICommand SaveIndividualTestCommand { get; }
        public ICommand CancelEditIndividualTestCommand { get; }
        private string fullName;
        public string FullName { get => fullName; set => SetProperty(ref fullName, value); }

        private string personnelNumber;
        public string PersonnelNumber { get => personnelNumber; set => SetProperty(ref personnelNumber, value); }

        private int countQuestions;
        public int CountQuestions { get => countQuestions; set => SetProperty(ref countQuestions, value); }


        public IndividualTest SelectIndividualTest { get; set; }
        public ObservableCollection<Question> QuestionsFromSelectTest { get; set; }

        public EditIndividualPageViewModel(Test SelectTest, IndividualTest? SelectIndividualTest)
        {
            SaveIndividualTestCommand = new RelayCommand(_ => Save(), _ => true);
            CancelEditIndividualTestCommand = new RelayCommand(_ => Cancel(), _ => true);

            FullName = SelectIndividualTest?.UserName ?? string.Empty;
            PersonnelNumber = SelectIndividualTest?.PersonnelNumber ?? string.Empty;
            CountQuestions = SelectIndividualTest?.CountQuestions ?? 0;

            this.SelectIndividualTest = SelectIndividualTest ?? new IndividualTest()
            {
                Questions = new ObservableCollection<int>(),
                TestId = SelectTest.Id
            };

            QuestionsFromSelectTest = SelectTest.Questions;
            foreach (var question in QuestionsFromSelectTest)
            {
                question.IsSelected = SelectIndividualTest?.Questions.Contains(question.GetId) ?? false;
                question.PropertyChanged += Question_PropertyChanged;
            }

            UpdateCountQuestions();
        }
        private void Question_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Question.IsSelected))
            {
                UpdateCountQuestions();
            }
        }

        private void UpdateCountQuestions()
        {
            CountQuestions = QuestionsFromSelectTest.Count(q => q.IsSelected);
        }

        public void Save()
        {
            // Проверки данных
            if (string.IsNullOrEmpty(FullName)) throw new ArgumentException("Укажите имя");
            if (string.IsNullOrEmpty(PersonnelNumber)) throw new ArgumentException("Укажите табельный номер");
            if (SelectIndividualTest.TestId <= 0) throw new ArgumentException("Не выбран тест");

            using var db = new DatabaseConnectionService();

            var individualTest = new IndividualTestsDB
            {
                TestId = SelectIndividualTest.TestId,
                UserName = FullName,
                PersonnelNumber = PersonnelNumber,
                CountQuestions = CountQuestions,
                Questions = QuestionsFromSelectTest.Where(q => q.IsSelected).Select(q => q.GetId).ToList()

            };

            var testExists = db.tests.Any(t => t.Id == SelectIndividualTest.TestId);
            if (!testExists) throw new ArgumentException("Указанный тест не существует");

            var existing = db.individualtests
                .FirstOrDefault(it => it.TestId == individualTest.TestId
                                  && it.PersonnelNumber == individualTest.PersonnelNumber);

            if (existing != null)
            {
                existing.UserName = individualTest.UserName;
                existing.CountQuestions = individualTest.CountQuestions;
                existing.Questions = individualTest.Questions;
            }
            else
            {
                db.individualtests.Add(individualTest);
            }

            db.SaveChanges();
            MessageBox.Show("Сохранено успешно!");
        }
        public void Cancel()
        {
            // Выход обратно к списку
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
