
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BasicRepos.Test.Setup
{
    public class TestDbContext : DbContext
    {
        public TestDbContext() : this(new DbContextOptionsBuilder<TestDbContext>())
        {

        }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {

        }

        public TestDbContext(DbContextOptionsBuilder<TestDbContext> builder) : this(builder.Options)
        {

        }

        public virtual DbSet<Trillig> Trilligs { get; set; }
        public virtual DbSet<Brillig> Brilligs { get; set; }
        public virtual DbSet<Moamrath> Moamraths { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Trillig>().HasData(new List<Trillig>()
            {
                new Trillig() { Id = 1, Name = "A", BrotherName = "Larry" },
                new Trillig() { Id = 2, Name = "B", BrotherName = "Joe" },
                new Trillig() { Id = 3, Name = "C", BrotherName = "Harry" },
                new Trillig() { Id = 4, Name = "D", BrotherName = "Moe" },
                new Trillig() { Id = 5, Name = "E", BrotherName = "Jerry" }
            });

            builder.Entity<Brillig>().HasData(new List<Brillig>()
            {
                new Brillig() { Id = "1", Heads = 4, Teeth = 70 },
                new Brillig() { Id = "2", Heads = 2, Teeth = 2 },
                new Brillig() { Id = "3", Heads = 3, Teeth = 70 },
                new Brillig() { Id = "4", Heads = 3, Teeth = 762 },
                new Brillig() { Id = "5", Heads = 5, Teeth = 70 },
                new Brillig() { Id = "6", Heads = 1, Teeth = 62 },
                new Brillig() { Id = "7", Heads = 4, Teeth = 70 }
            });

            builder.Entity<Moamrath>().HasData(new List<Moamrath>()
            {
                new Moamrath() { Id = Guid.Parse("EFED4F70-8B1A-4BB3-B14B-B6EA2EEE2267"), Sound = "yee", Legs = 40 },
                new Moamrath() { Id = Guid.Parse("323A3F72-3DAB-422B-A306-8E155CE1F61A"), Sound = "squeek", Legs = 40 },
                new Moamrath() { Id = Guid.Parse("8299D594-0AA4-4D33-8EB2-4557A2221AF8"), Sound = "wheee", Legs = 40 },
                new Moamrath() { Id = Guid.Parse("155C1269-5D13-4E83-8F0B-34B8B2B5FAE1"), Sound = "whine", Legs = 40 },
                new Moamrath() { Id = Guid.Parse("ABABF0EA-4128-4830-8471-B634585145D9"), Sound = "In quantum mechanics, the uncertainty principle (also known as Heisenberg's uncertainty principle) is any of a variety of mathematical inequalities[1] asserting a fundamental limit to the precision with which the values for certain pairs of physical quantities of a particle, such as position, x, and momentum, p, can be predicted from initial conditions", Legs = 40 }
            });
        }
    }
}
