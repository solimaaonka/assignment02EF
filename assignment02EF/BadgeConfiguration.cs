using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub;

public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
{
    public void Configure(EntityTypeBuilder<Badge> builder)
    {
        builder.ToTable("Badges");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BadgeNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.HasIndex(b => b.BadgeNumber)
               .IsUnique();

        builder.Property(b => b.IssuedDate)
               .HasDefaultValueSql("GETDATE()");

        builder.Property(b => b.Tier)
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.HasOne(b => b.Attendee)
               .WithOne(a => a.Badge)
               .HasForeignKey<Badge>(b => b.AttendeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
