using AstroPanda.Data.Test.Repositories;
using AstroPanda.Data.Test.Setup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AstroPanda.Data.Test.RepositortyTests
{
    public class KeylessRepositoryTest
    {
        public DbContextOptions<TestDbContext> DbOptions = new DbContextOptionsBuilder<TestDbContext>().UseInMemoryDatabase(databaseName: "testDb").Options;
        public TestDbContext _db;

        public KeylessRepository sut;

        public KeylessRepositoryTest()
        {
            _db = new TestDbContext(DbOptions);
            _db.Database.EnsureCreated();
        }

        [Fact]
        public void WhenConstructingTheRepository_ItWill_BeCreated()
        {
            sut = new KeylessRepository(_db);

            Assert.NotNull(sut);
        }

    }
}
