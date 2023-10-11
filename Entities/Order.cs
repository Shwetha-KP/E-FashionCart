using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tailoringapp.Entities
{
    [Table("Order")]
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int OrderStatus { get; set; }
        public int? AgentId { get; set; }
        public int? AddressId { get; set; }
        public string AgentType { get; set; }


    }
}
