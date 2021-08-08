using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoStudioApp.Models;
using System.Data.Entity;

namespace PhotoStudioApp.Models
{
    public class PhotoStudioDbContext : DbContext
    {
        public PhotoStudioDbContext() : base("name=PhotoStudioCon")
        {
            Database.SetInitializer<PhotoStudioDbContext>(null);
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudioSubscription> StudioSubscriptions { get; set; }
    }
}
