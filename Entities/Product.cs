using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tailoringapp.Entities
{
    [Table("Product")]
    public class Product
    {        
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

            public int Id { get; set; }
            public string ProductName { get; set; }
            public string Picture { get; set; }
        }
}
