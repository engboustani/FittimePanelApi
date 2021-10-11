using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FittimePanelApi.Data;
using System.Threading;

namespace FittimePanelApi.Data
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> options) : base(options)
        {

        }

        // Users
        public DbSet<User> Users { get; set; }
        public DbSet<UserMeta> UserMetas { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserRule> UserRules { get; set; }

        // Tickets
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }

        // Programs
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseDownload> ExerciseDownloads { get; set; }
        public DbSet<ExerciseMeta> ExerciseMetas { get; set; }
        public DbSet<ExerciseStatus> ExerciseStatus { get; set; }
        public DbSet<ExerciseType> ExerciseTypes { get; set; }

        // Payments
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentGetway> PaymentGetways { get; set; }
        public DbSet<PaymentMeta> PaymentMetas { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Ticket>().HasMany(t => t.TicketMessages).WithOne(tm => tm.Ticket);
        //    modelBuilder.Entity<Ticket>().HasMany(t => t.TicketStatuses).WithOne(ts => ts.Ticket);

        //}
        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
