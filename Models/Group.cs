using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_21.Models
{
    [Table("Groups")]
    public class Group
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public int FieldId { get; set; }
        public Field Field { get; set; }
        public ICollection<Student> Students { get; set; }
        public string NK
        {
            get
            {
                return Nazwa;
            }
        }
    }
}
