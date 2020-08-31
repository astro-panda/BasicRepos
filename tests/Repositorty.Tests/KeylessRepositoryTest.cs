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

        [Fact]
        public void QueryWithPredicate_ReturnsTypedIQueryable()
        {
            Assert.True(false);
        }

        [Fact]
        public void QueryWithPredicate_WithNullExpress_ReturnsTypedIQueryable()
        {
            Assert.True(false);
        }

        [Fact]
        public void QueryWithNoPredicate_ReturnsTypedIQueryable()
        {
            Assert.True(false);
        }

        [Fact]
        public void Query_WithGenericParameter_ReturnsGenericallyTypedIQueryable()
        {
            Assert.True(false);
        }

        [Fact]
        public void RawQuery_ReturnsNonTyped_IQueryable()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task GetAsync_WithApplicableQueryReturns_AValue()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task GetAsync_WithNullPredicate_ReturnsFirstValue()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllExistingValues()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task GetAllAsync_WithPredicate_ReturnsMatchingValues()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task Exists_WithPredicate_ReturnsTrue_IfExists()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task Exists_WithPredicate_ReturnsFale_IfNotExists()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task AddAsync_PersistsValuesTo_DataStore()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task AddAsync_DoesNothing_When_objects_areNull()
        {
            Assert.True(false);
        }


        [Fact]
        public async Task AddAsync_DoesNothing_When_objects_areEmpty()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task DeleteAsync_Params_RemovesExistingObjects_FromStore()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task DeleteAsync_Params_DoesNothingWith_NonExistentEntities()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task DeleteAsync_Params_DoesNothing_WhenEmpty()
        {
            Assert.True(false);
        }


        [Fact]
        public async Task DeleteAsync_DoesNothing_WhenEmpty()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task DeleteAsync_DoesNothing_WhenNull()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task DeleteAsync_DoesNothing_WithNonExistentValues()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task DeleteAsync_RemovesValues_FromStore()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task UpdateAsync_SimplyCalls_DbSaveChangesAsync()
        {
            Assert.True(false);
        }

    }
}
