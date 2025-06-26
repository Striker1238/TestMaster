using System.Collections.Generic;

namespace TestMaster.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<Question> Questions { get; set; }
    }
}
