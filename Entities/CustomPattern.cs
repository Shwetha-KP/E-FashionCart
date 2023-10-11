using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tailoringapp.Entities
{
    [Table("CustomPattern")]
    public class CustomPattern
        {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public int Quantity { get; set; }
        public string StitchingType { get; set; }
        public decimal? Price { get; set; }
        }
}
