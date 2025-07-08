using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private string fullName;
        public string FullName { get => fullName; set => SetProperty(ref fullName, value); }

        private string personnelNumber;
        public string PersonnelNumber { get => personnelNumber; set => SetProperty(ref personnelNumber, value); }

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
                .Select(ModelMapper.ToAppModel)
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
            // Проверяем, указаны ли данные пользователя, если нет то уведомляем и предлагаем
            // запустить общий тест, если указаны ищем инд. тест под указанные данные, и запускаем его
            using var db = new DatabaseConnectionService();
            if (!IsValidAndNormalize(ref fullName) || !IsValidAndNormalize(ref personnelNumber))
            {
                MessageBox.Show("Вы запустили общий тест, поскольку не указали " +
                    "индивидуальные данные или допустили в них ошибку", "Внимание!");
                Questions = SelectedTest.Questions.ToList();
            }
            else
            {

                var individualTests = db.individualtests.AsEnumerable()
                    .Where(it =>
                        it.UserName.ToLower() == FullName.ToLower() &&
                        it.PersonnelNumber.ToLower() == PersonnelNumber.ToLower() &&
                        it.TestId == SelectedTest.Id).ToList();

                var questionIds = individualTests
                    .SelectMany(it => it.Questions)
                    .ToHashSet();

                Questions = SelectedTest.Questions
                    .Where(q => questionIds.Contains(q.GetId))
                    .ToList();

            }

            if (Questions.Count == 0)
            {
                MessageBox.Show("Нет доступных вопросов для запуска теста.", "Внимание");
                return;
            }

            // Перемешивание вопросов
            if (SelectedTest.IsShuffleQuestions)
            {
                Questions = Questions.OrderBy(_ => Guid.NewGuid()).ToList();
            }
            // Ограничение количества вопросов
            if (SelectedTest.NumberQuestions > 0 && Questions.Count > SelectedTest.NumberQuestions)
            {
                Questions = Questions.Take(SelectedTest.NumberQuestions).ToList();
            }


            foreach (var question in Questions)
            {
                if (SelectedTest.IsShuffleAnswers)
                {
                    var shuffledAnswers = question.Answers
                        .Select((a, i) => new { Answer = a, OriginalIndex = i })
                        .OrderBy(_ => Guid.NewGuid())
                        .ToList();

                    question.Answers = new ObservableCollection<Answer>(shuffledAnswers.Select(x => x.Answer).ToList());
                }

                question.CorrectAnswerIndexes = new ObservableCollection<int>(question.Answers
                    .Select((a, idx) => new { a, idx })
                    .Where(x => x.a.IsCorrect)
                    .Select(x => x.idx)
                    .ToList());
            }


            _currentQuestionIndex = 0;
            CurrentQuestion = Questions[_currentQuestionIndex];
            IsTestRunning = true;

            ResetAnswer();
        }

        private void Answer()
        {
            var selectedAnswers = CurrentQuestion.Answers.Where(a => a.IsSelected).ToList();

            if (selectedAnswers.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите хотя бы один ответ перед продолжением.", "Ответ не выбран");
                return;
            }

            if (_currentQuestionIndex < Questions.Count - 1)
            {
                _currentQuestionIndex++;
                CurrentQuestion = Questions[_currentQuestionIndex];
            } 
            else
            {
                IsTestRunning = false;

                int correctAnswersCount = 0;
                var questionResult = new ObservableCollection<QuestionResult>();
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

                    questionResult.Add(new QuestionResult { Text = question.Text, IsCorrect = isCorrect });
                }

                int totalQuestions = Questions.Count;

                var result = new Result
                {
                    ComplatedTest = SelectedTest,
                    FullName = FullName,
                    PersonnelNumber = PersonnelNumber,
                    CountQuestions = totalQuestions,
                    CountCorrectAnswer = correctAnswersCount,
                    QuestionResult = questionResult,
                    IsSuccessfully = SelectedTest.CorrectAnswersCount <= 0 || SelectedTest.CorrectAnswersCount >= totalQuestions
                    ? correctAnswersCount == totalQuestions 
                    : correctAnswersCount >= SelectedTest.CorrectAnswersCount
                };



                Page editPage = new ResultPage(result);

                var mainWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
                if (mainWindow != null)
                {

                    var frame = FindFrameByName(mainWindow, "MainFrame");
                    if (frame != null)
                    {
                        frame.Navigate(editPage);
                        return;
                    }
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
            }
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

        public bool IsValidAndNormalize(ref string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = string.Join(" ", input
                .Trim()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .ToLowerInvariant();

            return true;
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
