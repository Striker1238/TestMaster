using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TestMaster.Commands;
using TestMaster.Models.App;
using TestMaster.Models.DB;
using TestMaster.Services;

namespace TestMaster.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand StartTestCommand { get; }
        public ICommand AnswerCommand { get; }
        public ICommand ResetAnswerCommand { get; }

        private bool _isTestRunning;
        public bool IsTestRunning
        {
            get => _isTestRunning;
            set => SetProperty(ref _isTestRunning, value);
        }

        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get => _currentQuestion;
            set => SetProperty(ref _currentQuestion, value);
        }
        public ObservableCollection<Test> Tests { get; set; } = new();

        private Test _selectedTest;
        public Test SelectedTest
        {
            get => _selectedTest;
            set => SetProperty(ref _selectedTest, value);
        }
        public List<Question> Questions { get; set; }
        private int _currentQuestionIndex;

        public MainViewModel()
        {
            StartTestCommand = new RelayCommand(_ => StartTest(), _ => true);
            AnswerCommand = new RelayCommand(_ => Answer(), _ => IsTestRunning);
            ResetAnswerCommand = new RelayCommand(_ => ResetAnswer(), _ => IsTestRunning);

            IsTestRunning = false;

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

        private void StartTest()    
        {
            if (SelectedTest == null)
            {
                MessageBox.Show("Выберите доступный тест из списка!","Внимание");
                return;
            }
            using var db = new DatabaseConnectionService();

            Questions = SelectedTest.Questions.ToList();

            _currentQuestionIndex = 0;
            CurrentQuestion = Questions[_currentQuestionIndex];
            IsTestRunning = true;

            ResetAnswer();
        }

        private void Answer()
        {

            if (_currentQuestionIndex < Questions.Count - 1)
            {
                _currentQuestionIndex++;
                CurrentQuestion = Questions[_currentQuestionIndex];
            } 
            else
            {
                IsTestRunning = false;

                int correctAnswersCount = 0;
                for (int i = 0; i < Questions.Count; i++)
                {
                    var question = Questions[i];
                    var selected = question.Answers
                        .Select((a, idx) => new { a.IsSelected, idx })
                        .Where(x => x.IsSelected)
                        .Select(x => x.idx)
                        .ToList();

                    bool isCorrect = selected.Count == question.CorrectAnswerIndexes.Count &&
                                     !selected.Except(question.CorrectAnswerIndexes).Any() &&
                                     !question.CorrectAnswerIndexes.Except(selected).Any();

                    if (isCorrect)
                        correctAnswersCount++;
                }

                int totalQuestions = SelectedTest.Questions.Count;
                double percent = totalQuestions > 0 ? (double)correctAnswersCount / totalQuestions * 100 : 0;

                MessageBox.Show($"Всего вопросов: {totalQuestions}\n" +
                    $"Правильных ответов: {correctAnswersCount}\n" +
                    $"Процент правильных ответов: {percent:F2}%",
                    "Результат тестирования");
            }
        }

        private void ResetAnswer()
        {
            if (CurrentQuestion is not null)
            {
                foreach (var answer in CurrentQuestion.Answers)
                {
                    answer.IsSelected = false;
                }
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
