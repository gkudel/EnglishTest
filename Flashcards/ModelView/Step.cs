using EnglishTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishTest.ModelView
{
    public class Step
    {
        public int TestId { get; set; }
        [Display(Name = "Polish word")]
        public string PolishWord { get; set; }
        [Display(Name = "English word")]
        [Required]
        public string EnglishWord { get; set; }

    }
}