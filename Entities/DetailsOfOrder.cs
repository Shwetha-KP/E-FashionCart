using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tailoringapp.Entities
{
    [Table("DetailsOfOrder")]
    public class DetailsOfOrder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int PatternId { get; set; }
        public string PatternType { get; set; }
        public int Quantity { get; set; }
        public string StitchingType { get; set; }
        public bool? Status { get; set; }
        public decimal? StitchingAmount { get; set; }
    }
}
