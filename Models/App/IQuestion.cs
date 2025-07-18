﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMaster.Models.DB;

namespace TestMaster.Models.App
{
    public interface IQuestion
    {
        string Text { get; set; }
        public List<AnswerDB> Answers { get; set; }
        public List<int> CorrectAnswerIndexes { get; set; }
    }
}
