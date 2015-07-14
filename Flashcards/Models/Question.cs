using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishTest.Models
{
    public class Question
    {
        public int Id { get; set; }
        public bool Pass { get; set; }
        public int WordId { get; set; }
        public int TestId { get; set; }

        public virtual Word Word { get; set; }
        public virtual Test Test { get; set; }
    }
}