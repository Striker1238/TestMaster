using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMaster.Models.DB;

namespace TestMaster.Models.App
{
    public class IndividualTest
    {
        public int TestId { get; set; }
        public string UserName { get; set; }
        public string PersonnelNumber { get; set; }
        public int CountQuestions { get; set; }
        public ObservableCollection<int> Questions { get; set; }

    }
}
