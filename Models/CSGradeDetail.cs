using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NC_21.Models
{
    [Table("CSGradeDetails")]
    public class CSGradeDetail
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }
        public decimal Ocena { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
