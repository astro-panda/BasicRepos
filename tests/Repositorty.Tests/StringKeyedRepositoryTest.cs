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
    public class StringKeyedRepositoryTest : IClassFixture<DataFixture>
    {
        public TestDbContext _db;

        public BrilligRepository sut;

        public StringKeyedRepositoryTest(DataFixture fixture)
        {
            _db = fixture.Db;
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
            await _db.Database.EnsureDeletedAsync();
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
        }


        [Fact]
        public async Task DeletingEntitiesByKey_WillNotAffectNonExistentKeys()
        {
            // Arrange
            var targetIds = new[] { "2", "22", "33" };
            var preBrilligs = _db.Brilligs.Where(x => targetIds.Contains(x.Id)).ToArray();
            Assert.Single(preBrilligs);

            sut = new BrilligRepository(_db);

            // Act
            await sut.DeleteAsync(targetIds);

            // Assert
            var postBrilligs = _db.Brilligs.Where(x => targetIds.Contains(x.Id)).ToArray();
            Assert.Empty(postBrilligs);
        }

        [Fact]
        public async Task AttemptingADelete_WithEmptyIds_HasNoEffect()
        { 
            // Arrange
            sut = new BrilligRepository(_db);
            var preBrilligs = await _db.Brilligs.CountAsync();

            // Act
            await sut.DeleteAsync();

            // Assert
            var postBrilligs = await _db.Brilligs.CountAsync();
            Assert.Equal(preBrilligs, postBrilligs);

        }

        [Fact]
        public async Task AttemptingADelete_WithEmptyIdCollection_HasNoEffect()
        {
            // Arrange
            var ids = new string[] { };
            sut = new BrilligRepository(_db);
            var preBrilligs = await _db.Brilligs.CountAsync();

            // Act
            await sut.DeleteAsync(ids);

            // Assert
            var postBrilligs = await _db.Brilligs.CountAsync();
            Assert.Equal(preBrilligs, postBrilligs);

        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("4")]
        [InlineData("5")]
        [InlineData("6")]
        [InlineData("7")]
        public async Task CheckingExistence_WillReturnTrue_WhenExists(string id)
        {
            // Arrange
            sut = new BrilligRepository(_db);

            // Act
            bool result = await sut.Exists(id);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("c")]
        [InlineData("d")]
        [InlineData("e")]
        [InlineData("f")]
        [InlineData("g")]
        public async Task CheckingExistence_WillReturnFalse_WhenNotExists(string id)
        {
            // Arrange
            sut = new BrilligRepository(_db);

            // Act
            bool result = await sut.Exists(id);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("1", "2", "3")]
        [InlineData("2", "3", "4")]
        [InlineData("3", "4", "5")]
        [InlineData("4", "5", "6")]
        [InlineData("5", "6", "7")]
        public async Task CheckingExistenceOfMultiples_WillReturnTrue_WhenAllExist(string id1, string id2, string id3)
        {
            // Arrange
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
            sut = new BrilligRepository(_db);

            // Act
            bool result = await sut.Exists(id1, id2, id3);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("1", "2", "a")]
        [InlineData("2", "3", "b")]
        [InlineData("3", "4", "c")]
        [InlineData("4", "5", "d")]
        [InlineData("5", "6", "e")]
        public async Task CheckingExistenceOfMultiples_WillReturnFalse_WhenAnyNotExist(string id1, string id2, string id3)
        {
            // Arrange
            
     
            sut = new BrilligRepository(_db);

            // Act
            bool result = await sut.Exists(id1, id2, id3);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("4")]
        [InlineData("5")]
        [InlineData("6")]
        [InlineData("7")]
        public async Task GetAsync_WillReturnTheCorrectValue_WhenExists(string id)
        {
     
            
            // Arrange
            sut = new BrilligRepository(_db);

            // Act
            Brillig result = await sut.GetAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);

        }

        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("c")]
        [InlineData("d")]
        [InlineData("e")]
        [InlineData("f")]
        [InlineData("g")]
        public async Task GetAsync_WillReturnNull_WhenNotExists(string id)
        {
            // Arrange
            sut = new BrilligRepository(_db);

            // Act            
            Brillig result = await sut.GetAsync(id);

            // Assert
            Assert.Null(result);            
        }

        [Theory]
        [InlineData("1", "2", "a")]
        [InlineData("2", "3", "b")]
        [InlineData("3", "4", "c")]
        [InlineData("4", "5", "d")]
        [InlineData("5", "6", "e")]
        [InlineData("6", "7", "f")]
        public async Task GetAsyncMultiple_WillReturnOnlyThoseWithMatchingKeys(string id1, string id2, string id3)
        {
            // Arrange
            sut = new BrilligRepository(_db);

            // Act
            IEnumerable<Brillig> result = await sut.GetAsync(id1, id2, id3);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());

        }

        [Theory]
        [InlineData("1", "2", "3")]
        [InlineData("2", "3", "4")]
        [InlineData("3", "4", "5")]
        [InlineData("4", "5", "6")]
        [InlineData("5", "6", "7")]
        public async Task GetAsyncMultiple_WillReturnAllMatchingEntities(string id1, string id2, string id3)
        {
            // Arrange
            sut = new BrilligRepository(_db);

            // Act
            IEnumerable<Brillig> result = await sut.GetAsync(id1, id2, id3);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count());

        }
    }
}
