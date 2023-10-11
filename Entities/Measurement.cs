using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tailoringapp.Entities
{
    [Table("Measurement")]
    public class Measurement
        {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? DetailId { get; set; }
        public string Partname { get; set; }
        public string UnitName { get; set; }
        public decimal? UnitValue { get; set; }
        }
}
