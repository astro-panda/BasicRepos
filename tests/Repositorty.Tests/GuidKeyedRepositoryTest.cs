using AstroPanda.Data.Test.Repositories;
using AstroPanda.Data.Test.Setup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace AstroPanda.Data.Test.RepositortyTests
{
    public class GuidKeyedRepositoryTest : IClassFixture<DataFixture>
    {        
        public DbContextOptions<TestDbContext> DbOptions;
        public TestDbContext _db;

        public MoamrathRepository sut;
        public GuidKeyedRepositoryTest(DataFixture fixture)
        {
            _db = fixture.Db;
        }

        [Fact]
        public void WhenConstructingTheRepository_ItWill_BeCreated()
        {
            sut = new MoamrathRepository(_db);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<IKeyedRepository<Moamrath, Guid>>(sut);
        }


        [Fact]
        public async Task DeletingEntitiesByKey_RemoveEntriesWithExisitngKeys()
        {
            // Arrange
            var targetIds = new[]
            {
                Guid.Parse("EFED4F70-8B1A-4BB3-B14B-B6EA2EEE2267"),
                Guid.Parse("323A3F72-3DAB-422B-A306-8E155CE1F61A"),
                Guid.Parse("8299D594-0AA4-4D33-8EB2-4557A2221AF8")
            };

            var preMoamraths = _db.Moamraths.Where(x => targetIds.Contains(x.Id)).ToArray();
            Assert.Equal(3, preMoamraths.Length);

            sut = new MoamrathRepository(_db);

            // Act
            await sut.DeleteAsync(targetIds);

            // Assert
            var postMoamraths = _db.Moamraths.Where(x => targetIds.Contains(x.Id)).ToArray();
            Assert.Empty(postMoamraths);
        }


        [Fact]
        public async Task DeletingEntitiesByKey_WillNotAffectNonExistentKeys()
        {
            // Arrange
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
            var targetIds = new[] { Guid.Parse("EFED4F70-8B1A-4BB3-B14B-B6EA2EEE2267"), Guid.NewGuid(), Guid.NewGuid() };
            var preMoamraths = await _db.Moamraths.Where(x => targetIds.Contains(x.Id)).ToArrayAsync();
            Assert.Single(preMoamraths);

            sut = new MoamrathRepository(_db);

            // Act
            await sut.DeleteAsync(targetIds);

            // Assert
            var postMoamraths = await _db.Moamraths.Where(x => targetIds.Contains(x.Id)).ToArrayAsync();
            Assert.Empty(postMoamraths);
        }

        [Fact]
        public async Task AttemptingADelete_WithEmptyIds_HasNoEffect()
        { 
            // Arrange
            sut = new MoamrathRepository(_db);
            var preMoamraths = await _db.Moamraths.CountAsync();

            // Act
            await sut.DeleteAsync();

            // Assert
            var postMoamraths = await _db.Moamraths.CountAsync();
            Assert.Equal(preMoamraths, postMoamraths);
        }

        [Fact]
        public async Task AttemptingADelete_WithEmptyIdCollection_HasNoEffect()
        {
            // Arrange
            var ids = new Guid[] { };
            sut = new MoamrathRepository(_db);
            var preMoamraths = await _db.Moamraths.CountAsync();

            // Act
            await sut.DeleteAsync(ids);

            // Assert
            var postMoamraths = await _db.Moamraths.CountAsync();
            Assert.Equal(preMoamraths, postMoamraths);
        }

        [Theory]
        [InlineData("EFED4F70-8B1A-4BB3-B14B-B6EA2EEE2267")]   
        [InlineData("323A3F72-3DAB-422B-A306-8E155CE1F61A")]   
        [InlineData("8299D594-0AA4-4D33-8EB2-4557A2221AF8")]   
        [InlineData("155C1269-5D13-4E83-8F0B-34B8B2B5FAE1")]   
        [InlineData("ABABF0EA-4128-4830-8471-B634585145D9")]   
        public async Task CheckingExistence_WillReturnTrue_WhenExists(string id)
        {
            // Arrange
            sut = new MoamrathRepository(_db);

            // Act
            bool result = await sut.Exists(Guid.Parse(id));

            // Assert
            Assert.True(result);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task CheckingExistence_WillReturnFalse_WhenNotExists(int iteration)
        {
            // Arrange
            sut = new MoamrathRepository(_db);

            // Act
            bool result = await sut.Exists(Guid.NewGuid());

            // Assert
            Assert.False(result);
        }

        [Theory]
        [ClassData(typeof(MoamrathIdsData))]
        public async Task CheckingExistenceOfMultiples_WillReturnTrue_WhenAllExist(params Guid[] ids)
        {
            // Arrange
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
            sut = new MoamrathRepository(_db);

            // Act
            bool result = await sut.Exists(ids);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [ClassData(typeof(PartialFakeMoamrathIdsData))]
        public async Task CheckingExistenceOfMultiples_WillReturnFalse_WhenAnyNotExist(params Guid[] ids)
        {
            // Arrange
            sut = new MoamrathRepository(_db);

            // Act
            bool result = await sut.Exists(ids);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("EFED4F70-8B1A-4BB3-B14B-B6EA2EEE2267")]
        [InlineData("323A3F72-3DAB-422B-A306-8E155CE1F61A")]
        [InlineData("8299D594-0AA4-4D33-8EB2-4557A2221AF8")]
        [InlineData("155C1269-5D13-4E83-8F0B-34B8B2B5FAE1")]
        [InlineData("ABABF0EA-4128-4830-8471-B634585145D9")]
        public async Task GetAsync_WillReturnTheCorrectValue_WhenExists(string id)
        {
            // Arrange
            sut = new MoamrathRepository(_db);

            // Act
            Moamrath result = await sut.GetAsync(Guid.Parse(id));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Guid.Parse(id), result.Id);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public async Task GetAsync_WillReturnNull_WhenNotExists(int iteration)
        {
            // Arrange
            sut = new MoamrathRepository(_db);

            // Act            
            Moamrath result = await sut.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("EFED4F70-8B1A-4BB3-B14B-B6EA2EEE2267", "323A3F72-3DAB-422B-A306-8E155CE1F61A")]
        [InlineData("323A3F72-3DAB-422B-A306-8E155CE1F61A", "8299D594-0AA4-4D33-8EB2-4557A2221AF8")]
        [InlineData("8299D594-0AA4-4D33-8EB2-4557A2221AF8", "155C1269-5D13-4E83-8F0B-34B8B2B5FAE1")]
        [InlineData("155C1269-5D13-4E83-8F0B-34B8B2B5FAE1", "ABABF0EA-4128-4830-8471-B634585145D9")]
        public async Task GetAsyncMultiple_WillReturnOnlyThoseWithMatchingKeys(string id1, string id2)
        {
            // Arrange
            sut = new MoamrathRepository(_db);

            // Act
            IEnumerable<Moamrath> result = await sut.GetAsync(Guid.Parse(id1), Guid.Parse(id2), Guid.NewGuid());

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }

        [Theory]
        [ClassData(typeof(MoamrathIdsData))]
        public async Task GetAsyncMultiple_WillReturnAllMatchingEntities(params Guid[] ids)
        {
            // Arrange
            sut = new MoamrathRepository(_db);

            // Act
            IEnumerable<Moamrath> result = await sut.GetAsync(ids);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(ids.Length, result.Count());           
        }
    }
}
