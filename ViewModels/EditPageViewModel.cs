using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TestMaster.Commands;
using TestMaster.Models;
using TestMaster.Models.App;
using TestMaster.Models.DB;
using TestMaster.Services;

namespace TestMaster.ViewModels
{
    class EditPageViewModel : INotifyPropertyChanged
    {
        public ICommand AddNewQuestionCommand { get; set; }
        public ICommand EditQuestionCommand { get; set; }
        public ICommand DeleteQuestionCommand { get; set; }


        public ICommand AddAnswerCommand { get; set; }
        public ICommand DeleteAnswerCommand { get; set; }
        public ICommand SaveTestCommand { get; set; }

        public Test CreatedTest { get; set; }

        private Question selectedQuestion;
        public Question SelectedQuestion
        {
            get => selectedQuestion;
            set => SetProperty(ref selectedQuestion, value);
        }

        private bool isEditQuestionVisible;
        public bool IsEditQuestionVisible
        {
            get => isEditQuestionVisible;
            set => SetProperty(ref isEditQuestionVisible, value);
        }


        public EditPageViewModel(Test? selectTest)
        {
            AddNewQuestionCommand = new RelayCommand(_ => CreatedNewQuestion(), _ => true);
            EditQuestionCommand = new RelayCommand(_ => OpenChangesQuestion(), _ => true);
            DeleteQuestionCommand = new RelayCommand(q => DeleteQuestion(q as Question), q => q is Question);

            AddAnswerCommand = new RelayCommand(_ => CreateNewAnswer(), _ => true);
            DeleteAnswerCommand = new RelayCommand(a => DeleteAnswer(a as Answer), a => a is Answer);

            SaveTestCommand = new RelayCommand(_ => SaveTest(), _ => true);

            
            CreatedTest = selectTest ?? new Test
            {
                Title = "Новый тест",
                Category = "Без категории",
                Description = "",
                NumberQuestions = 0,
                CorrectAnswersCount = 0,
                IsShuffleQuestions = false,
                IsShuffleAnswers = false,
                Questions = new ObservableCollection<Question>()
            };

        }


        public void CreatedNewQuestion()
        {
            var newQuestion = new Question
            {
                Text = "Новый вопрос",
                Answers = new(),
                CorrectAnswerIndexes = new()
            };

            CreatedTest.Questions.Add(newQuestion);
            SelectedQuestion = newQuestion;
            IsEditQuestionVisible = true;
            OnPropertyChanged(nameof(CreatedTest));
        }

        public void OpenChangesQuestion() => IsEditQuestionVisible = true;

        public void CreateNewAnswer()
        {
            if (SelectedQuestion == null)
            {
                MessageBox.Show("Ошибка добавления ответа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newAnswer = new Answer { Text = "Новый ответ", IsCorrect = false };
            SelectedQuestion.Answers.Add(newAnswer);
            OnPropertyChanged(nameof(SelectedQuestion));
        }

        public void DeleteAnswer(Answer? answer)
        {
            if (SelectedQuestion != null && answer != null)
            {
                SelectedQuestion.Answers.Remove(answer);
                OnPropertyChanged(nameof(SelectedQuestion));
            }
        }

        public void DeleteQuestion(Question? question)
        {
            if (question != null)
            {
                CreatedTest.Questions.Remove(question);
                if (SelectedQuestion == question)
                {
                    SelectedQuestion = null;
                    IsEditQuestionVisible = false;
                }
                OnPropertyChanged(nameof(CreatedTest));
            }
        }

        public void SaveTest()
        {
            if (CreatedTest == null)
            {
                MessageBox.Show("Не удается сохранить тест", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            using var db = new DatabaseConnectionService();

            TestDB testDb;

            var existingTest = db.tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefault(t => t.Id == CreatedTest.Id);

            if (existingTest != null)
            {
                db.questions.RemoveRange(existingTest.Questions);

                testDb = existingTest;
                testDb.Title = CreatedTest.Title;
                testDb.Description = CreatedTest.Description;
                testDb.Category = CreatedTest.Category;
                testDb.NumberQuestions = CreatedTest.NumberQuestions;
                testDb.CorrectAnswersCount = CreatedTest.CorrectAnswersCount;
                testDb.IsShuffleQuestions = CreatedTest.IsShuffleQuestions;
                testDb.IsShuffleAnswers = CreatedTest.IsShuffleAnswers;
                testDb.Questions = CreatedTest.Questions.Select(ModelMapper.ToDbModel).ToList();
                
            }
            else
            {
                testDb = new TestDB
                {
                    Title = CreatedTest.Title,
                    Description = CreatedTest.Description,
                    Category = CreatedTest.Category,
                    NumberQuestions = CreatedTest.NumberQuestions,
                    CorrectAnswersCount = CreatedTest.CorrectAnswersCount,
                    IsShuffleQuestions = CreatedTest.IsShuffleQuestions,
                    IsShuffleAnswers = CreatedTest.IsShuffleAnswers,
                    Questions = CreatedTest.Questions.Select(ModelMapper.ToDbModel).ToList()
                };

                db.tests.Add(testDb);
            }

            db.SaveChanges();

            var updatedTest = db.tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers)
                .First(t => t.Id == testDb.Id);

            for (int i = 0; i < CreatedTest.Questions.Count; i++)
            {
                var uiQuestion = CreatedTest.Questions[i];
                var dbQuestion = updatedTest.Questions[i];

                dbQuestion.CorrectAnswerIndexes = dbQuestion.Answers
                    .Where((a, index) => uiQuestion.Answers[index].IsCorrect)
                    .Select(a => a.Id)
                    .ToList();
            }

            db.SaveChanges();

            MessageBox.Show("Тест успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
