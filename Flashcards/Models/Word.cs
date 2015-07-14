using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishTest.Models
{
    public class Word
    {
        public int Id { get; set; }
        [Required]
        [Index("IX_Polish", 1, IsUnique=true)]
        [MaxLength(100)]
        public string Polish { get; set; }
        [Required]
        [MaxLength(100)]
        public string English { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}