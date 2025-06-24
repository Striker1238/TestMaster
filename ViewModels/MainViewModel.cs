using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TestMaster.Commands;
using TestMaster.Models;

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

        public ObservableCollection<IQuestion> Questions { get; }

        private int _currentQuestionIndex;

        public MainViewModel()
        {
            Questions = new ObservableCollection<IQuestion>
            {
                new SingleChoiceQuestion
                {
                    Text = "В течение какого срока должны храниться результаты индивидуального дозиметрического контроля?",
                    Answers = {
                        new Answer { Text = "Не менее 30 лет" },
                        new Answer { Text = "Не менее 20 лет" },
                        new Answer { Text = "Не менее 50 лет" }
                    },
                    CorrectAnswerIndex = 2
                },
                new SingleChoiceQuestion
                {
                    Text = "Кто, согласно Федеральному закону «Об использовании атомной энергии», несет ответственность за убытки и вред, причиненные радиационным воздействием?",
                    Answers = {
                        new Answer { Text = "Правительство Российской Федерации." },
                        new Answer { Text = "Госкорпорация «Росатом»." },
                        new Answer { Text = "Эксплуатирующая организация." }
                    },
                    CorrectAnswerIndex = 2
                },
                new SingleChoiceQuestion
                {
                    Text = "Какие значения основных пределов доз установлены для персонала (группа А)?",
                    Answers = {
                        new Answer { Text = "1 мЗв в год за любые последние 5 лет." },
                        new Answer { Text = "5 мЗв в год в среднем за любые 5 лет." },
                        new Answer { Text = "20 мЗв в год за любые последовательные 5 лет, но не более 50 мЗв в год." }
                    },
                    CorrectAnswerIndex = 2
                }
            };

            StartTestCommand = new RelayCommand(_ => StartTest(), _ => true);
            AnswerCommand = new RelayCommand(_ => Answer(), _ => IsTestRunning);
            ResetAnswerCommand = new RelayCommand(_ => ResetAnswer(), _ => IsTestRunning);

            IsTestRunning = false;
        }

        private void StartTest()
        {
            _currentQuestionIndex = 0;
            CurrentQuestion = Questions[_currentQuestionIndex];
            IsTestRunning = true;

            // Сброс всех ответов
            foreach (var question in Questions.OfType<SingleChoiceQuestion>())
            {
                foreach (var answer in question.Answers)
                {
                    answer.IsSelected = false;
                }
            }
        }

        private void Answer()
        {
            if (CurrentQuestion is SingleChoiceQuestion scq)
            {
                bool isCorrect = scq.IsAnswerCorrect();

                // Здесь можно логировать или считать правильные ответы
                Console.WriteLine(isCorrect ? "Верно" : "Неверно");
            }

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
            if (CurrentQuestion is SingleChoiceQuestion scq)
            {
                foreach (var answer in scq.Answers)
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
