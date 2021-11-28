using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NC_21.Models
{
    [Table("Students")]
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        [StringLength(6)]
        public string Album { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data rozpoczęcia")]
        public DateTime Data_start { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public ICollection<CourseStudent> CourseStudents { get; set; }
        public ICollection<CSGradeDetail> CSGradeDetails { get; set; }
        public string I_N
        {
            get
            {
                return Imie + " " + Nazwisko;
            }
        }
    }
}

