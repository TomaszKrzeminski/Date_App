using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DateApp.Models
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MessageUser>().HasKey(sc => new { sc.AppUserId, sc.MessageId });
            modelBuilder.Entity<AppUser>()
        .HasOne(a => a.Details)
        .WithOne(b => b.User)
        .HasForeignKey<SearchDetails>(b => b.AppUserId);
        }

        public DbSet<SearchDetails> SearchDetails { get; set; }
        public DbSet<Pair> Pairs { get; set; }
        public DbSet<Message> Messages { get; set; }




    }
}
