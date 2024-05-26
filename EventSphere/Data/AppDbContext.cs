using EventSphere.Models;
using Microsoft.EntityFrameworkCore;

namespace EventSphere.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Attendees)
                .WithMany(a => a.Events)
                .UsingEntity(j => j.ToTable("EventAttendees"));



            Console.WriteLine("This should be a conflict");
            modelBuilder.Entity<Ticket>();
            Console.WriteLine("This should be a confldsaict");

        }
    }
}
