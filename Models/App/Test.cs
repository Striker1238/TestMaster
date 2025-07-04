using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TestMaster.Models.DB;

namespace TestMaster.Models.App
{
    public class Test
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public ObservableCollection<Question> Questions { get; set; }
        public int NumberQuestions { get; set; }
        public int CorrectAnswersCount { get; set; }
        public bool IsShuffleQuestions { get; set; }
        public bool IsShuffleAnswers { get; set; }
    }
}

