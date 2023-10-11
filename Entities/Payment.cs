using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tailoringapp.Entities
{
    [Table("Payment")]
    public class Payment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int AgentId { get; set; }
        public string OrderDate { get; set; }
        public string PaymentDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string PaymentMode { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? Balance { get; set; }
        public string Remarks { get; set; }
    }
}
