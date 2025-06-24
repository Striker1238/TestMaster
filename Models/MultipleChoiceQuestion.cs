using System.Collections.Generic;
using System.Linq;

namespace TestMaster.Models
{
    public class MultipleChoiceQuestion : AbstractQuestion
    {
        public List<string> Options { get; set; } = new();
        public List<int> CorrectIndices { get; set; } = new();

        public bool ValidateAnswer(object userAnswer)
        {
            if (userAnswer is List<int> indices)
                return !CorrectIndices.Except(indices).Any() && !indices.Except(CorrectIndices).Any();
            return false;
        }
    }
}