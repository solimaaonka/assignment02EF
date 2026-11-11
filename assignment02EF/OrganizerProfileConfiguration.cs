using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub;

public class OrganizerProfileConfiguration : IEntityTypeConfiguration<OrganizerProfile>
{
    public void Configure(EntityTypeBuilder<OrganizerProfile> builder)
    {
        builder.ToTable("OrganizerProfiles");

        builder.HasKey(p => p.OrganizerId);

        builder.Property(p => p.Biography)
               .HasMaxLength(1000);

        builder.Property(p => p.WebsiteUrl)
               .HasMaxLength(300);

        builder.Property(p => p.LogoUrl)
               .HasMaxLength(300);

        builder.HasOne(p => p.Organizer)
               .WithOne(o => o.Profile)
               .HasForeignKey<OrganizerProfile>(p => p.OrganizerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
