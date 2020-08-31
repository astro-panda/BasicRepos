using AstroPanda.Data.Test.Repositories;
using AstroPanda.Data.Test.Setup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AstroPanda.Data.Test.RepositortyTests
{
    public class GuidKeyedRepositoryTest
    {
        public DbContextOptions<TestDbContext> DbOptions = new DbContextOptionsBuilder<TestDbContext>().UseInMemoryDatabase(databaseName: "testDb").Options;
        public TestDbContext _db;

        public MoamrathRepository sut;

        public GuidKeyedRepositoryTest()
        {
            _db = new TestDbContext(DbOptions);
            _db.Database.EnsureCreated();
        }

        [Fact]
        public void WhenConstructingTheRepository_ItWill_BeCreated()
        {
            sut = new MoamrathRepository(_db);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<IKeyedRepository<Moamrath, Guid>>(sut);
        }



        
    }
}
