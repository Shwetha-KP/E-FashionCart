using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tailoringapp.Entities
{
    [Table("Agent")]
    public class Agent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DeliveryAgent { get; set; }
        public string AdharNumber { get; set; }
        public string Gender { get; set; }
        public string PickupAgent { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        public string Address { get; set; }
        public int? Likes { get; set; }
        public int? Dislikes { get; set; }
        }
}
