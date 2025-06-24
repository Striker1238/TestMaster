using System.Collections.Generic;

namespace TestMaster.Models
{
    public class Test
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<IQuestion> Questions { get; set; } = new();
    }
}
