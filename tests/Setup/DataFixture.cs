using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BasicRepos.Test.Setup
{
    public class DataFixture : IDisposable
    {
        public TestDbContext Db { get; set; }

        public DataFixture()
        {
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();


            var DbOptions = new DbContextOptionsBuilder<TestDbContext>().UseInMemoryDatabase(
                databaseName: "TestDb",
                databaseRoot: new InMemoryDatabaseRoot())
                .UseInternalServiceProvider(serviceProvider).Options;

            Db = new TestDbContext(DbOptions);

            Db.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Db.Database.EnsureDeleted();
            Db.Dispose();
        }
    }
}
