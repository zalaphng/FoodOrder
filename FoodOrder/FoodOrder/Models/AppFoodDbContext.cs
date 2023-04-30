using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FoodOrder.Models
{
    public class AppFoodDbContext:DbContext
    {
        public AppFoodDbContext():base("FoodDB")
        {

        }
        public DbSet<Products> Products { get; set; }
        public DbSet<Users> SignupLogin { get; set; }
        public DbSet<AdminLogins> adminLogin { get; set; }
        public DbSet<InvoiceModels> invoiceModel { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<ContactModels> contactModels { get; set; }
        //public DbSet<BlogModel> blogModels { get; set; }

    }
}