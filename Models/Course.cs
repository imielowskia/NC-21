using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_21.Models
{
    [Table("Courses")]
    public class Course
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public ICollection<Field> Fields { get; set; }
        public ICollection<CourseStudent> CourseStudents { get; set; }
        public ICollection<CSGradeDetail> CSGradeDetails { get; set; }
    }
}
