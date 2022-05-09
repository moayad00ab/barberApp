using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using barber;
using System;
using System.Collections.Generic;
using System.Text;
using barber.Models;

namespace barber.Data;

public class ApplicationDbContext : IdentityDbContext<users>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    public DbSet<appointment> appointment { get; set; }
        public DbSet<offers> offers { get; set; }
        public DbSet<services> services { get; set; }
        public DbSet<files> files { get; set; }
       public DbSet<feedback> feedback { get; set; }
              public DbSet<timeList> timeList { get; set; }

       public DbSet<slot> slot { get; set; }
}
