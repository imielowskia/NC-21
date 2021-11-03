using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_21.Models
{
    [Table("Fields")]
    public class Field
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public int InstitutId { get; set; }
        public Institut Institut { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
