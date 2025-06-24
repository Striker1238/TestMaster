using System.Collections.Generic;

namespace TestMaster.Models
{
    public class TestResult
    {
        public Test Test { get; set; }
        public Dictionary<IQuestion, object> GivenAnswers { get; set; } = new();
    }
}