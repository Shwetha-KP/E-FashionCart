using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tailoringapp.Entities
{
    [Table("Address")]
    public class Address
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string AddressType { get; set; }
        public string Country{ get; set; }
        public string FullName{ get; set; }
        public string MobileNo { get; set; }
        public string PinCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Landmark{ get; set; }
        public string City{ get; set; }
        public string State{ get; set; }
    }
}
