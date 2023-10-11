using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tailoringapp.Entities;

namespace tailoringapp.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Pattern> Pattern { get; set; }
        public DbSet<Agent> Agent { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<DetailsOfOrder> DetailsOfOrder { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CustomPattern> CustomPattern { get; set; }
        public DbSet<Measurement> Measurement { get; set; }
        public DbSet<Payment> Payment { get; set; }

    } 
}

