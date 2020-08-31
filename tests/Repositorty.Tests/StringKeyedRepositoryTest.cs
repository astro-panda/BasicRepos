using AstroPanda.Data.Test.Repositories;
using AstroPanda.Data.Test.Setup;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AstroPanda.Data.Test.RepositortyTests
{
    public class StringKeyedRepositoryTest
    {
        public DbContextOptions<TestDbContext> DbOptions = new DbContextOptionsBuilder<TestDbContext>().UseInMemoryDatabase(databaseName: "testDb").Options;
        public TestDbContext _db;

        public BrilligRepository sut;

        public StringKeyedRepositoryTest()
        {
            _db = new TestDbContext(DbOptions);            
        }

        [Fact]
        public void WhenConstructingTheRepository_ItWill_BeCreated()
        {
            sut = new BrilligRepository(_db);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<IKeyedRepository<Brillig, string>>(sut);
        }

        [Fact]
        public async Task DeletingEntitiesByKey_RemoveEntriesWithExisitngKeys()
        {
            // Arrange
            await _db.Database.EnsureCreatedAsync();
            var targetIds = new[] { "1", "2", "3" };

            var preBrilligs = _db.Brilligs.Where(x => targetIds.Contains(x.Id)).ToArray();
            Assert.Equal(3, preBrilligs.Length);

            sut = new BrilligRepository(_db);

            // Act
            await sut.DeleteAsync(targetIds);

            // Assert
            var postBrilligs = _db.Brilligs.Where(x => targetIds.Contains(x.Id)).ToArray();
            Assert.Empty(postBrilligs);
            await _db.Database.EnsureDeletedAsync();
        }


        [Fact]
        public async Task DeletingEntitiesByKey_WillNotAffectNonExistentKeys()
        {
            // Arrange
            await _db.Database.EnsureCreatedAsync();
            var targetIds = new[] { "1", "22", "33" };
            var preBrilligs = _db.Brilligs.Where(x => targetIds.Contains(x.Id)).ToArray();
            Assert.Single(preBrilligs);

            sut = new BrilligRepository(_db);

            // Act
            await sut.DeleteAsync(targetIds);

            // Assert
            var postBrilligs = _db.Brilligs.Where(x => targetIds.Contains(x.Id)).ToArray();
            Assert.Empty(postBrilligs);
            await _db.Database.EnsureDeletedAsync();
        }

        [Fact]
    }
}
