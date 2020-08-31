using AstroPanda.Data.Test.Repositories;
using AstroPanda.Data.Test.Setup;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AstroPanda.Data.Test.RepositortyTests
{
    public class IntKeyedRepositoryTest
    {
        public DbContextOptions<TestDbContext> DbOptions = new DbContextOptionsBuilder<TestDbContext>().UseInMemoryDatabase(databaseName: "testDb").Options;
        public TestDbContext _db;

        public TrilligRepository sut;

        public IntKeyedRepositoryTest()
        {
            _db = new TestDbContext(DbOptions);
            _db.Database.EnsureCreated();
        }

        [Fact]
        public void WhenConstructingTheRepository_ItWill_BeCreated()
        {
            sut = new TrilligRepository(_db);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<IKeyedRepository<Trillig, int>>(sut);
        }

    }
}
