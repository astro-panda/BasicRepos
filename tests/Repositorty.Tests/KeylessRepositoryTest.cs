using BasicRepos.Test.Repositories;
using BasicRepos.Test.Setup;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BasicRepos.Test.RepositortyTests
{
    public class KeylessRepositoryTest : IClassFixture<DataFixture>
    {
        private TestDbContext _db;
        private KeylessRepository sut;

        public KeylessRepositoryTest(DataFixture fixture)
        {
            _db = fixture.Db;
        }

        [Fact]
        public void WhenConstructingTheRepository_ItWill_BeCreated()
        {
            sut = new KeylessRepository(_db);
            Assert.NotNull(sut);
        }

        [Fact]
        public void QueryWithPredicate_ReturnsTypedIQueryable()
        {
            // Arrange
            sut = new KeylessRepository(_db);

            // Act
            var result = sut.Query(x => x.Name == "Joe");

            // Assert
            Assert.IsAssignableFrom<IQueryable<Trillig>>(result);
        }

        [Fact]
        public void QueryWithPredicate_WithNullExpress_ReturnsTypedIQueryable()
        {
            // Arrange
            sut = new KeylessRepository(_db);

            // Act
            var result = sut.Query();

            // Assert
            Assert.IsAssignableFrom<IQueryable<Trillig>>(result);
        }

        [Fact]
        public void Query_WithGenericParameter_ReturnsGenericallyTypedIQueryable()
        {
            // Arrange
            sut = new KeylessRepository(_db);

            // Act
            var result = sut.Query<SuperTrillig>();

            // Assert
            Assert.IsAssignableFrom<IQueryable<SuperTrillig>>(result);
        }

        [Fact]
        public void RawQuery_ReturnsNonTyped_IQueryable()
        {
            // Arrange
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
            sut = new KeylessRepository(_db);

            var result = sut.RawQuery();
            
            Assert.IsAssignableFrom<IQueryable>(result);
        }

        [Theory]
        [InlineData("Larry")]
        [InlineData("Joe")]
        [InlineData("Harry")]
        [InlineData("Moe")]
        [InlineData("Jerry")]
        public async Task GetAsync_WithApplicableQueryReturns_AValue(string broName)
        {
            // Arrange
      
      
            sut = new KeylessRepository(_db);

            // Act
            var result = await sut.GetAsync(x => x.BrotherName == broName);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllExistingValues()
        {
            // Arrange
      
      
            sut = new KeylessRepository(_db);

            // Act
            var result = await sut.GetAllAsync();
            
            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(5, result.Count());
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(2, 3, 4)]
        [InlineData(3, 4, 5)]
        public async Task GetAllAsync_WithPredicate_ReturnsMatchingValues(int id1, int id2, int id3)
        {
            // Arrange
      
      
            sut = new KeylessRepository(_db);

            // Act
            var result = await sut.GetAllAsync(x => new[] { id1, id2, id3 }.Contains(x.Id));

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count());
        }

        [Theory]
        [InlineData("A")]
        [InlineData("B")]
        [InlineData("C")]
        [InlineData("D")]
        [InlineData("E")]
        public async Task Exists_WithPredicate_ReturnsTrue_IfExists(string name)
        {
            // Arrange
      
      
            sut = new KeylessRepository(_db);

            // Act
            bool result = await sut.Exists(x => x.Name == name);

            // Assert
            Assert.True(result);
        }


        [Theory]
        [InlineData("non")]
        [InlineData("exists")]
        [InlineData("lol")]
        [InlineData("jupiter")]
        [InlineData("purple")]
        public async Task Exists_WithPredicate_ReturnsFalse_IfNotExists(string name)
        {
            // Arrange
      
      
            sut = new KeylessRepository(_db);

            // Act
            bool result = await sut.Exists(x => x.Name == name);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddAsync_PersistsValuesTo_DataStore()
        {
            // Arrange
            Trillig toAdd = new Trillig() { Name = "F", BrotherName = "No Brother" };
      
      
            sut = new KeylessRepository(_db);

            // Act
            await sut.AddAsync(toAdd);

            // Assert
            bool result = _db.Trilligs.Any(x => x.Name == "F");

            Assert.True(result);
        }

        [Fact]
        public async Task AddAsync_DoesNothing_When_objects_areNull()
        {
            // Arrange   
      
      
            sut = new KeylessRepository(_db);

            // Act
            await sut.AddAsync(null);

            // Assert
            bool result = _db.Trilligs.Any(x => x.Name == "Z");

            Assert.False(result);
        }


        [Fact]
        public async Task AddAsync_DoesNothing_When_objects_areEmpty()
        {
            // Arrange   
      
      
            sut = new KeylessRepository(_db);

            // Act
            await sut.AddAsync(new Trillig[] { });

            // Assert
            bool result = _db.Trilligs.Any(x => x.Name == "Z");

            Assert.False(result);
        }

        //[Fact]
        //public async Task DeleteAsync_Params_RemovesExistingObjects_FromStore()
        //{
        //    // Arrange
        //    await _db.Database.EnsureDeletedAsync();
        //    await _db.Database.EnsureCreatedAsync();
        //    sut = new KeylessRepository(_db);

        //    var remove1 = await _db.Trilligs.FindAsync(1);
        //    var remove2 = await _db.Trilligs.FindAsync(2);
        //    ICollection<int> removedIds = new List<int>() { 1, 2 };

        //    bool existenceVAlidation = await _db.Trilligs.AnyAsync(x => removedIds.Contains(x.Id));

        //    Assert.True(existenceVAlidation);

        //    // Act
        //    await sut.DeleteAsync(remove1, remove2);

        //    // Assert
        //    bool result = await _db.Trilligs.AnyAsync(x => removedIds.Contains(x.Id));

        //    Assert.False(result);
        ////}

        //[Fact]
        //public async Task DeleteAsync_Params_DoesNothingWith_NonExistentEntities()
        //{
        //    // Arrange      
        //    await _db.Database.EnsureDeletedAsync();
        //    await _db.Database.EnsureCreatedAsync();
        //    sut = new KeylessRepository(_db);

        //    var count = await _db.Trilligs.ToListAsync();
        //    Assert.Equal(5, count.Count);

        //    // Act
        //    await sut.DeleteAsync();

        //    // Assert
        //    var postCount = await _db.Trilligs.ToListAsync();
        //    Assert.Equal(count.Count, postCount.Count);
        //}

//        [Fact]
        public async Task DeleteAsync_Params_DoesNothing_WhenEmpty()
        {
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
            Assert.True(false);
        }


        //[Fact]
        public async Task DeleteAsync_DoesNothing_WhenEmpty()
        {
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
            Assert.True(false);
        }

       // [Fact]
        public async Task DeleteAsync_DoesNothing_WhenNull()
        {
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
            Assert.True(false);
        }

        //[Fact]
        public async Task DeleteAsync_DoesNothing_WithNonExistentValues()
        {
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
            Assert.True(false);
        }

        //[Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task DeleteAsync_RemovesValues_FromStore(int id)
        {
            var mockDb = new Mock<TestDbContext>();
            var mockSet = new Mock<DbSet<Trillig>>();
            var trilligList = _db.Trilligs.ToList();


            mockDb.Setup(x => x.Set<Trillig>()).Returns(mockSet.Object);            
            mockSet.Setup(y => y.Remove(It.IsAny<Trillig>())).Callback<Trillig>((t) => trilligList.Remove(t)); 
            mockSet.Setup(y => y.RemoveRange(It.IsAny<Trillig[]>())).Callback<Trillig>((t) => trilligList.RemoveAll(x => x.Id == t.Id)); 

            _db = mockDb.Object;
            sut = new KeylessRepository(_db);            

            await sut.DeleteAsync(new Trillig() { Id = id });

            bool stillExists = trilligList.Any(x => x.Id == id);

            Assert.False(stillExists);
        }

        [Fact]
        public async Task UpdateAsync_SimplyCalls_DbSaveChangesAsync()
        {
            // Arrange
            var mockDb = new Mock<TestDbContext>();
            var mockSet = new Mock<DbSet<Trillig>>();
            mockDb.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            mockDb.Setup(x => x.Set<Trillig>()).Returns(mockSet.Object);
            _db = mockDb.Object;
            sut = new KeylessRepository(_db);

            // Act
            await sut.UpdateAsync();

            mockDb.Verify(x => x.SaveChangesAsync(default), Times.Once());
            mockSet.Verify(x => x.UpdateRange(It.IsAny<Trillig[]>()), Times.Once);
        }
    }
}
