using Microsoft.EntityFrameworkCore;
using EventHub;

using var context = new EventHubContext();

if (!context.Database.GetMigrations().Any())
{
    Console.WriteLine("No migrations found! Run: Add-Migration InitialCreate  then run the app again.");
    return;
}

context.Database.Migrate();
Console.WriteLine($"Connected to database: {context.Database.GetDbConnection().Database}");

if (!context.Organizers.Any())
{
    var organizer = new Organizer
    {
        Name = "Ahmed Ali",
        CompanyName = "Tech Events Co.",
        IsVerified = true,
        Profile = new OrganizerProfile
        {
            Biography = "Organizing tech conferences since 2015.",
            WebsiteUrl = "https://techevents.example.com",
            LogoUrl = "https://techevents.example.com/logo.png"
        },
        Events = new List<Event>
        {
            new Event
            {
                Title = "EF Core Workshop",
                Description = "A hands-on workshop about Entity Framework Core.",
                StartDate = new DateTime(2026, 12, 1, 10, 0, 0),
                EndDate = new DateTime(2026, 12, 1, 16, 0, 0),
                MaxAttendees = 100
            },
            new Event
            {
                Title = "ASP.NET Meetup",
                Description = "Monthly community meetup.",
                StartDate = new DateTime(2026, 12, 15, 18, 0, 0),
                MaxAttendees = 50   
            }
        }
    };

    var attendee = new Attendee
    {
        FullName = "Sara Mohamed",
        Email = "sara@example.com",
        Street = "12 Nile St",
        City = "Cairo",
        Country = "Egypt",
        PostalCode = "11511",
        Badge = new Badge
        {
            BadgeNumber = "BDG-0001",
            Tier = BadgeTier.VIP
        }
    };

    context.Organizers.Add(organizer);
    context.Attendees.Add(attendee);
    context.SaveChanges();
}

Console.WriteLine("\n--- Organizers ---");
foreach (var o in context.Organizers.Include(o => o.Profile).Include(o => o.Events))
{
    Console.WriteLine($"{o.Name} ({o.CompanyName}) Verified={o.IsVerified}");
    Console.WriteLine($"  Profile: {o.Profile?.Biography} | {o.Profile?.WebsiteUrl}");
    foreach (var e in o.Events)
        Console.WriteLine($"  Event: {e.Title} | Start={e.StartDate:d} | End={(e.EndDate?.ToString("d") ?? "N/A")} | Max={e.MaxAttendees}");
}

Console.WriteLine("\n--- Attendees ---");
foreach (var a in context.Attendees.Include(a => a.Badge))
{
    Console.WriteLine($"{a.FullName} | {a.Email}");
    Console.WriteLine($"  FullAddress: {a.FullAddress}");
    Console.WriteLine($"  Badge: {a.Badge?.BadgeNumber} | {a.Badge?.Tier} | Issued={a.Badge?.IssuedDate:g}");
}
