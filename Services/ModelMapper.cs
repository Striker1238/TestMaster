using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestMaster.Models.App;
using TestMaster.Models.DB;

namespace TestMaster.Services
{
    public static class ModelMapper
    {
        // ======== Question Mapping ========

        public static Question ToAppModel(QuestionDB db) => new Question
        {
            TestId = db.TestId,
            Text = db.Text,
            CorrectAnswerIndexes = new ObservableCollection<int>(db.CorrectAnswerIndexes ?? new List<int>()),
            Answers = new ObservableCollection<Answer>(
                    db.Answers?.Select(ToAppModel) ?? Enumerable.Empty<Answer>())
        };

        public static QuestionDB ToDbModel(Question app)
        {
            using var db = new DatabaseConnectionService();
            return new QuestionDB
            {
                TestId = app.TestId,
                Text = app.Text,
                Test = db.tests.Find(app.TestId) ?? new TestDB(),
                CorrectAnswerIndexes = app.CorrectAnswerIndexes?.ToList() ?? new List<int>(),
                Answers = app.Answers?.Select(ToDbModel).ToList() ?? new List<AnswerDB>()
            };
        }

        // ======== Answer Mapping ========

        public static Answer ToAppModel(AnswerDB db) => new Answer
        {
            QuestionId = db.QuestionId,
            Text = db.Text,
            IsCorrect = db.Question?.CorrectAnswerIndexes?.Contains(db.Id) == true
        };

        public static AnswerDB ToDbModel(Answer app)
        {
            using var db = new DatabaseConnectionService();
            return new AnswerDB
            {
                QuestionId = app.QuestionId,
                Text = app.Text,
                Question = db.questions.Find(app.QuestionId) ?? new QuestionDB() // Assuming QuestionId is the ID of the test
            };
        }
            


        // ======== Test Mapping ========

        public static Test ToAppModel(TestDB db) => new Test
        {
            Id = db.Id,
            Title = db.Title,
            Description = db.Description,
            Category = db.Category,
            NumberQuestions = db.NumberQuestions,
            CorrectAnswersCount = db.CorrectAnswersCount,
            IsShuffleQuestions = db.IsShuffleQuestions,
            IsShuffleAnswers = db.IsShuffleAnswers,
            Questions = new ObservableCollection<Question>(
                    db.Questions?.Select(ToAppModel) ?? Enumerable.Empty<Question>())
        };

        public static TestDB ToDbModel(Test app) => new TestDB
        {
            Id = app.Id,
            Title = app.Title,
            Description = app.Description,
            Category = app.Category,
            NumberQuestions = app.NumberQuestions,
            CorrectAnswersCount = app.CorrectAnswersCount,
            IsShuffleQuestions = app.IsShuffleQuestions,
            IsShuffleAnswers = app.IsShuffleAnswers,
            Questions = app.Questions?.Select(ToDbModel).ToList() ?? new List<QuestionDB>()
        };

        // ======== Individual Test Mapping ========

        public static IndividualTest ToAppModel(IndividualTestsDB db) => new IndividualTest
        {
            TestId = db.TestId,
            PersonnelNumber = db.PersonnelNumber,
            UserName = db.UserName,
            CountQuestions = db.CountQuestions,
            Questions = new ObservableCollection<int>(db.Questions ?? new List<int>())
        };

        public static IndividualTestsDB ToDbModel(IndividualTest app)
        {
            using var db = new DatabaseConnectionService(); 
            return new IndividualTestsDB
            {
                PersonnelNumber = app.PersonnelNumber,
                UserName = app.UserName,
                Test = db.tests.Find(app.TestId) ?? new TestDB(),
                TestId = app.TestId,
                CountQuestions = app.CountQuestions,
                Questions = app.Questions?.ToList() ?? new List<int>()
            };
        }
    }
}
