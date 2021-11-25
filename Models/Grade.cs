using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NC_21.Models
{
    public class Grade
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int GroupId { get; set; }
        public String I_N { get; set; }
        public decimal Ocena { get; set; }
        public DateTime Data { get; set; }
    }
}
