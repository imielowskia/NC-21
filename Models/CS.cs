using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NC_21.Models
{
    public class CS
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public string I_N { get; set; }
        public decimal Ocena { get; set; }
        public DateTime Data { get; set; }
    }
}
