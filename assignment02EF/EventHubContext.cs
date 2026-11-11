using Microsoft.EntityFrameworkCore;

namespace EventHub;

public class EventHubContext : DbContext
{
    public DbSet<Organizer> Organizers { get; set; }
    public DbSet<OrganizerProfile> OrganizerProfiles { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Attendee> Attendees { get; set; }
    public DbSet<Badge> Badges { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=EventHubDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Organizer and Attendee => Data Annotations (nothing here for them)

        // OrganizerProfile and Badge => Fluent API in separate configuration classes
        modelBuilder.ApplyConfiguration(new OrganizerProfileConfiguration());
        modelBuilder.ApplyConfiguration(new BadgeConfiguration());

        // Event => Fluent API directly inside OnModelCreating
        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Events");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(e => e.Description)
                  .IsRequired()
                  .HasMaxLength(2000);

            entity.Property(e => e.StartDate)
                  .IsRequired();

            // EndDate is optional (DateTime?)
            entity.Property(e => e.EndDate)
                  .IsRequired(false);

            entity.Property(e => e.MaxAttendees)
                  .IsRequired();

            // One-to-Many: Organizer (1) -> Events (many)
            entity.HasOne(e => e.Organizer)
                  .WithMany(o => o.Events)
                  .HasForeignKey(e => e.OrganizerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
