using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace thing_list
{
    public class Application_Context:DbContext
    {
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Thing> Things { get; set; } = null!;
        public Application_Context() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=db.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Employee>()
                .HasMany(c => c.Things)
                .WithMany(s => s.Employees)
                .UsingEntity<Taken_things>(
                   j => j
                    .HasOne(pt => pt.thing)
                    .WithMany(t => t.Taken_Things)
                    .HasForeignKey(pt => pt.id_thing),
                j => j
                    .HasOne(pt => pt.employee)
                    .WithMany(p => p.Taken_Things)
                    .HasForeignKey(pt => pt.id_employee),
                j =>
                {
                    j.Property(pt => pt.date).HasDefaultValueSql("CURRENT_TIMESTAMP");
                    j.Property(pt => pt.comm).HasDefaultValue(null);
                    j.HasKey(t => new { t.id_thing, t.id_employee });
                    j.ToTable("Enrollments");
                });
        }
    }
}
