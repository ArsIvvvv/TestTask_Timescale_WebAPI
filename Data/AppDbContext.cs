using Microsoft.EntityFrameworkCore;
using System.Xml;
using WebApi_TimeScale.Data.Entity;

namespace WebApi_TimeScale.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Value> Values {  get; set; }
        public DbSet<Result> Results { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Value>(e =>
            {
                e.Property(r => r.Date)
                .HasColumnType("timestamp with time zone");

                e.Property(r => r.Id)
                .ValueGeneratedOnAdd();

                e.HasOne(x => x.Result)
               .WithMany(x => x.ListValue)
               .HasForeignKey(x => x.ResultId)
               .OnDelete(DeleteBehavior.Cascade); 
            });
        }
    }
}
