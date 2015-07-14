using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishTest.Models
{
    public class Test
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Finish { get; set; }
        public bool Finished { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}