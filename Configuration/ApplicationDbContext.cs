using Kruggers_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Kruggers_Backend.Configuration;

public class ApplicationDbContext :  DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)

    {
    }


    public DbSet<Role> Roles { get; set; }

    public DbSet<Status> Statuses { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<ImageTask> ImageTasks { get; set; }

    public DbSet<Gallery> Galleries { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    

    modelBuilder.Entity<Role>()
        .Property(r => r.RoleType)
        .HasConversion<string>();

    modelBuilder.Entity<Status>()
        .Property(s => s.StatusType)
        .HasConversion<string>();


    modelBuilder.Entity<Role>().HasData(
        new Role { Id = 1, RoleType = RoleType.CREATOR.ToString() },
        new Role { Id = 2, RoleType = RoleType.CONSUMER.ToString() }
    );

    modelBuilder.Entity<Status>().HasData(
        new Status { Id = 1, StatusType = StatusType.OPEN_ASSIGNED.ToString() },
        new Status { Id = 2, StatusType = StatusType.OPEN_REQUESTED.ToString() },
        new Status { Id = 3, StatusType = StatusType.OPEN_COOKING.ToString() },
        new Status { Id = 4, StatusType = StatusType.CLOSED_REJECTED.ToString() },
        new Status { Id = 5, StatusType = StatusType.CLOSED_RESOLVED.ToString() },
        new Status { Id = 6, StatusType = StatusType.CLOSED_CANCELLED.ToString() }
    );


    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();
    
    modelBuilder.Entity<User>()
        .HasIndex(u=>u.Username)
        .IsUnique();

    modelBuilder.Entity<User>()
        .HasOne(u => u.Role)
        .WithMany(r => r.Users)
        .HasForeignKey(u => u.RoleId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<ImageTask>()
        .HasOne(t => t.Status)
        .WithMany(s => s.Tasks)
        .HasForeignKey(t => t.StatusId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<ImageTask>()
        .HasOne(t => t.Consumer)
        .WithMany(u => u.CreatedTasks)
        .HasForeignKey(t => t.ConsumerId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<ImageTask>()
        .HasOne(t => t.Creator)
        .WithMany(u => u.AssignedTasks) 
        .HasForeignKey(t => t.CreatorId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Gallery>()
        .HasOne(g => g.CreatedBy)
        .WithMany(u => u.CreatedGalleries) 
        .HasForeignKey(g => g.CreatorId)
        .OnDelete(DeleteBehavior.Restrict);


    modelBuilder.Entity<Gallery>()
        .HasOne(g => g.RequestedBy)
        .WithMany(u => u.RequestedGalleries) 
        .HasForeignKey(g => g.RequesterId)
        .OnDelete(DeleteBehavior.Restrict);
}
}