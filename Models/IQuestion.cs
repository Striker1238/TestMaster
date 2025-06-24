using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMaster.Models
{
    public interface IQuestion
    {
        string Text { get; set; }
        bool IsAnswerCorrect { get; set; }
    }
}
