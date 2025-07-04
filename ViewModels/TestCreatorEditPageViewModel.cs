using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace TestMaster.ViewModels
{
    class TestCreatorEditPageViewModel : INotifyPropertyChanged
    {
        public ICommand AddNewQuestionCommand { get; set; }
        public ICommand EditQuestionCommand { get; set; }
        public ICommand SaveEditQuestionCommand { get; set; }
        public ICommand CancelEditQuestionCommand { get; set; }
        public Test CreatedTest { get; set; }

        private Question selectedQuestion;
        public Question SelectedQuestion
        {
            get => selectedQuestion;
            set => SetProperty(ref selectedQuestion, value);
        }
        private bool isEditQuestionVisible;
        public bool IsEditQuestionVisible {
            get => isEditQuestionVisible; 
            set => SetProperty(ref isEditQuestionVisible, value); 
        }

        public TestCreatorEditPageViewModel(Test? selectTest)
        {
            AddNewQuestionCommand = new RelayCommand(_ => CreatedNewQuestion(), _ => true);
            EditQuestionCommand = new RelayCommand(_ => OpenChangesQuestion(), _ => true);
            SaveEditQuestionCommand = new RelayCommand(_ => SaveChangesQuestion(), _ => true);
            CancelEditQuestionCommand = new RelayCommand(_ => CancelChangesQuestion(), _ => true);

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
                Answers = new (),
                CorrectAnswerIndexes = new ()
            };

            CreatedTest.Questions.Add(newQuestion);

            // Делаем его выбранным для редактирования
            SelectedQuestion = newQuestion;
            IsEditQuestionVisible = true;
            OnPropertyChanged(nameof(CreatedTest));
        }
        public void OpenChangesQuestion()
        {
            IsEditQuestionVisible = true;
        }
        public void SaveChangesQuestion()
        {
            // Предполагается, что SelectedQuestion уже изменён через биндинг
            // Можно добавить валидацию или обновление списка вопросов
            IsEditQuestionVisible = false;
            OnPropertyChanged(nameof(CreatedTest));
        }
        public void CancelChangesQuestion()
        {
            // Если вопрос новый и не сохранён — удалить его
            if (SelectedQuestion != null && string.IsNullOrWhiteSpace(SelectedQuestion.Text))
            {
                CreatedTest.Questions?.Remove(SelectedQuestion);
            }
            IsEditQuestionVisible = false;
            SelectedQuestion = null;
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
