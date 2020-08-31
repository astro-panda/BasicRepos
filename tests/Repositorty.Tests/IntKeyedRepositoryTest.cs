using AstroPanda.Data.Test.Repositories;
using AstroPanda.Data.Test.Setup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AstroPanda.Data.Test.RepositortyTests
{
    public class IntKeyedRepositoryTest
    {
        public DbContextOptions<TestDbContext> DbOptions;
        public TestDbContext _db;

        public TrilligRepository sut;

        public IntKeyedRepositoryTest()
        {
            DbOptions = new DbContextOptionsBuilder<TestDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _db = new TestDbContext(DbOptions);            
        }

        [Fact]
        public void WhenConstructingTheRepository_ItWill_BeCreated()
        {
            sut = new TrilligRepository(_db);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<IKeyedRepository<Trillig, int>>(sut);
        }

        [Fact]
        public async Task DeletingEntitiesByKey_RemoveEntriesWithExisitngKeys()
        {
            // Arrange
            await _db.Database.EnsureCreatedAsync();
            var targetIds = new[] { 1, 2, 3 };

            var preTrilligs = _db.Trilligs.Where(x => targetIds.Contains(x.Id)).ToArray();
            Assert.Equal(3, preTrilligs.Length);

            sut = new TrilligRepository(_db);

            // Act
            await sut.DeleteAsync(targetIds);

            // Assert
            var postTrilligs = _db.Trilligs.Where(x => targetIds.Contains(x.Id)).ToArray();
            Assert.Empty(postTrilligs);
            await _db.Database.EnsureDeletedAsync();
        }


        [Fact]
        public async Task DeletingEntitiesByKey_WillNotAffectNonExistentKeys()
        {
            // Arrange
            await _db.Database.EnsureCreatedAsync();
            var targetIds = new[] { 1, 22, 33 };
            var preTrilligs = _db.Trilligs.Where(x => targetIds.Contains(x.Id)).ToArray();
            Assert.Single(preTrilligs);

            sut = new TrilligRepository(_db);

            // Act
            await sut.DeleteAsync(targetIds);

            // Assert
            var postTrilligs = _db.Trilligs.Where(x => targetIds.Contains(x.Id)).ToArray();
            Assert.Empty(postTrilligs);
            await _db.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task AttemptingADelete_WithEmptyIds_HasNoEffect()
        {
            await _db.Database.EnsureCreatedAsync();
            // Arrange
            sut = new TrilligRepository(_db);
            var preTrilligs = await _db.Trilligs.CountAsync();

            // Act
            await sut.DeleteAsync();

            // Assert
            var postTrilligs = await _db.Trilligs.CountAsync();
            Assert.Equal(preTrilligs, postTrilligs);

            await _db.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task AttemptingADelete_WithEmptyIdCollection_HasNoEffect()
        {
            await _db.Database.EnsureCreatedAsync();
            // Arrange
            var ids = new int[] { };
            sut = new TrilligRepository(_db);
            var preTrilligs = await _db.Trilligs.CountAsync();

            // Act
            await sut.DeleteAsync(ids);

            // Assert
            var postTrilligs = await _db.Trilligs.CountAsync();
            Assert.Equal(preTrilligs, postTrilligs);

            await _db.Database.EnsureDeletedAsync();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task CheckingExistence_WillReturnTrue_WhenExists(int id)
        {
            await _db.Database.EnsureCreatedAsync();
            // Arrange
            sut = new TrilligRepository(_db);

            // Act
            bool result = await sut.Exists(id);

            // Assert
            Assert.True(result);
            await _db.Database.EnsureDeletedAsync();
        }


        [Theory]
        [InlineData(201)]
        [InlineData(202)]
        [InlineData(203)]
        [InlineData(204)]
        [InlineData(205)]
        [InlineData(206)]
        [InlineData(207)]
        public async Task CheckingExistence_WillReturnFalse_WhenNotExists(int id)
        {
            await _db.Database.EnsureCreatedAsync();
            // Arrange
            sut = new TrilligRepository(_db);

            // Act
            bool result = await sut.Exists(id);

            // Assert
            Assert.False(result);
            await _db.Database.EnsureDeletedAsync();
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(2, 3, 4)]
        [InlineData(3, 4, 5)]
        public async Task CheckingExistenceOfMultiples_WillReturnTrue_WhenAllExist(int id1, int id2, int id3)
        {
            await _db.Database.EnsureCreatedAsync();
            // Arrange
            sut = new TrilligRepository(_db);

            // Act
            bool result = await sut.Exists(id1, id2, id3);

            // Assert
            Assert.True(result);
            await _db.Database.EnsureDeletedAsync();
        }

        [Theory]
        [InlineData(1, 2, 10)]
        [InlineData(2, 3, 20)]
        [InlineData(3, 4, 30)]
        [InlineData(4, 5, 40)]
        [InlineData(5, 6, 50)]
        public async Task CheckingExistenceOfMultiples_WillReturnFalse_WhenAnyNotExist(int id1, int id2, int id3)
        {
            await _db.Database.EnsureCreatedAsync();
            // Arrange
            sut = new TrilligRepository(_db);

            // Act
            bool result = await sut.Exists(id1, id2, id3);

            // Assert
            Assert.False(result);
            await _db.Database.EnsureDeletedAsync();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task GetAsync_WillReturnTheCorrectValue_WhenExists(int id)
        {
            await _db.Database.EnsureCreatedAsync();
            // Arrange
            sut = new TrilligRepository(_db);

            // Act
            Trillig result = await sut.GetAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);

            await _db.Database.EnsureDeletedAsync();
        }

        [Theory]
        [InlineData(201)]
        [InlineData(202)]
        [InlineData(203)]
        [InlineData(204)]
        [InlineData(205)]
        [InlineData(206)]
        [InlineData(207)]
        public async Task GetAsync_WillReturnNull_WhenNotExists(int id)
        {
            await _db.Database.EnsureCreatedAsync();
            // Arrange
            sut = new TrilligRepository(_db);

            // Act            
            Trillig result = await sut.GetAsync(id);

            // Assert
            Assert.Null(result);

            await _db.Database.EnsureDeletedAsync();
        }

        [Theory]
        [InlineData(1, 2, 10)]
        [InlineData(2, 3, 20)]
        [InlineData(3, 4, 30)]
        [InlineData(4, 5, 40)]
        public async Task GetAsyncMultiple_WillReturnOnlyThoseWithMatchingKeys(int id1, int id2, int id3)
        {
            await _db.Database.EnsureCreatedAsync();
            // Arrange
            sut = new TrilligRepository(_db);

            // Act
            IEnumerable<Trillig> result = await sut.GetAsync(id1, id2, id3);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());

            await _db.Database.EnsureDeletedAsync();
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(2, 3, 4)]
        [InlineData(3, 4, 5)]
        public async Task GetAsyncMultiple_WillReturnAllMatchingEntities(int id1, int id2, int id3)
        {
            await _db.Database.EnsureCreatedAsync();
            // Arrange
            sut = new TrilligRepository(_db);

            // Act
            IEnumerable<Trillig> result = await sut.GetAsync(id1, id2, id3);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count());

            await _db.Database.EnsureDeletedAsync();
        }
    }
}
