using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FittimePanelApi.Data;
using System.Threading;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FittimePanelApi.Configuration.Entities;

namespace FittimePanelApi.Data
{
    public class AppDb : IdentityDbContext<User>
    {
        public AppDb(DbContextOptions<AppDb> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());

            builder.Entity<UserBlob>().Property(p => p.Value)
                .HasColumnType("MediumBlob");

            builder.Entity<ExerciseBlob>().Property(p => p.Value)
                .HasColumnType("MediumBlob");

            builder.Entity<ExerciseDownload>().Property(p => p.Value)
                .HasColumnType("MediumBlob");

            builder.Entity<PaymentGetaway>().HasData(new PaymentGetaway { Id = 1, Name = "payir", Enabled = true });
            builder.Entity<PaymentGetaway>().HasData(new PaymentGetaway { Id = 2, Name = "idpay", Enabled = true });
            builder.Entity<PaymentGetaway>().HasData(new PaymentGetaway { Id = 3, Name = "zarinpal", Enabled = true });

            builder.Entity<Exercise>()
                .HasOne(p => p.UserStudent)
                .WithMany(b => b.Exercises);

            builder.Entity<Ticket>()
                .HasOne(p => p.UserCreated)
                .WithMany(b => b.Tickets);
        }

        // Users
        public DbSet<UserMeta> UserMetas { get; set; }
        public DbSet<UserBlob> UserBlobs { get; set; }

        // Tickets
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }

        // Programs
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseDownload> ExerciseDownloads { get; set; }
        public DbSet<ExerciseMeta> ExerciseMetas { get; set; }
        public DbSet<ExerciseBlob> ExerciseBlobs { get; set; }
        public DbSet<ExerciseType> ExerciseTypes { get; set; }

        // Payments
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentGetaway> PaymentGetways { get; set; }

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
