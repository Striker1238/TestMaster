using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TestMaster.Commands;
using TestMaster.Models;
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

        private IQuestion _currentQuestion;
        public IQuestion CurrentQuestion
        {
            get => _currentQuestion;
            set => SetProperty(ref _currentQuestion, value);
        }

        public List<IQuestion> Questions { get; set; }

        private int _currentQuestionIndex;

        public MainViewModel()
        {
            StartTestCommand = new RelayCommand(_ => StartTest(), _ => true);
            AnswerCommand = new RelayCommand(_ => Answer(), _ => IsTestRunning);
            ResetAnswerCommand = new RelayCommand(_ => ResetAnswer(), _ => IsTestRunning);

            IsTestRunning = false;
        }

        private void StartTest()
        {
            using var db = new DatabaseConnectionService();

            Questions = db.questions.Include(q => q.Answers)
                                    .ToList()
                                    .Cast<IQuestion>()
                                    .ToList();

            _currentQuestionIndex = 0;
            CurrentQuestion = Questions[_currentQuestionIndex];
            IsTestRunning = true;

            ResetAnswer();
        }

        private void Answer()
        {
            var selectedIndexes = CurrentQuestion.Answers
                .Select((a, idx) => new { a.IsSelected, idx })
                .Where(x => x.IsSelected)
                .Select(x => x.idx)
                .ToList();

            bool isCorrect = selectedIndexes.Count == CurrentQuestion.CorrectAnswerIndexes.Count &&
                             !selectedIndexes.Except(CurrentQuestion.CorrectAnswerIndexes).Any() &&
                             !CurrentQuestion.CorrectAnswerIndexes.Except(selectedIndexes).Any();

            Console.WriteLine(isCorrect ? "Верно" : "Неверно");

            if (_currentQuestionIndex < Questions.Count - 1)
            {
                _currentQuestionIndex++;
                CurrentQuestion = Questions[_currentQuestionIndex];
            } 
            else
            {
                IsTestRunning = false;
                MessageBox.Show("Тест завершён!");
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
