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
            modelBuilder.Entity<EventUser>().HasKey(sc => new { sc.AppUserId, sc.EventId });
            modelBuilder.Entity<AppUser>()
        .HasOne(a => a.Details)
        .WithOne(b => b.User)
        .HasForeignKey<SearchDetails>(b => b.AppUserId);

            modelBuilder.Entity<MatchUser>().HasKey(sc => new { sc.AppUserId, sc.MatchId });

            modelBuilder.Entity<AppUser>()
       .HasOne(a => a.coordinates)
       .WithOne(b => b.User)
       .HasForeignKey<Coordinates>(b => b.AppUserId);

            modelBuilder.Entity<AppUser>()
       .HasOne(a => a.Notification)
       .WithOne(b => b.User)
       .HasForeignKey<NotificationCheck>(b => b.AppUserId);


        }





        public DbSet<SearchDetails> SearchDetails { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Coordinates> Coordinates { get; set; }
        
        public DbSet<MatchUser> MatchUsers { get; set; }

        //Added 
        public DbSet<NotificationCheck> NotificationCheck { get; set; }

        public DbSet<ReportUser> ReportUsers { get; set; }

        public DbSet<MessageUser> MessageUser { get; set; }

        public DbSet<LoginHistory> LoginHistory { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventUser> EventUser { get; set; }

    }
}
