using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_21.Models
{
    [Table("CourseStudents")]
    public class CourseStudent
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        [Range(0, 5.5)]
        [Required]
        [RegularExpression("2|3|4|5")]
        public decimal Ocena { get; set; }
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
