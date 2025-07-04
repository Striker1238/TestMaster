using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMaster.Models.DB
{
    public class IndividualTestsDB
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public TestDB Test { get; set; }
        public string UserName { get; set; }
        public string PersonnelNumber { get; set; }
    }
}
