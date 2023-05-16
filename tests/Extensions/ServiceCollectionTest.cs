using BasicRepos.Exceptions;
using BasicRepos.Test.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BasicRepos.Test.Extensions
{
    public class ServiceCollectionTest
    {
        private void ScaffoldDbContextConfiguration()
        {

        }
    
        [Fact]
        public void AddBasicRepos_WithDbContext_registers_an_IRepository_ForeachDbset()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<TestDbContext>(opts => opts.UseInMemoryDatabase("lolcat"));

            // Act
            services.AddBasicRepos<TestDbContext>(options => options.EnabledCachedRepositories = false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            
            // Assert
            IRepository<Trillig> trilligRepo = serviceProvider.GetService<IRepository<Trillig>>();
            Assert.NotNull(trilligRepo);

            IRepository<Brillig> brilligRepo = serviceProvider.GetService<IRepository<Brillig>>();
            Assert.NotNull(brilligRepo);

            IRepository<Moamrath> moamrathRepo = serviceProvider.GetService<IRepository<Moamrath>>();
            Assert.NotNull(moamrathRepo);
        }


        [Fact]
        public void AddBasicRepos_WithDbContext_registers_an_IReadOnlyRepository_ForeachDbset()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<TestDbContext>(opts => opts.UseInMemoryDatabase("lolcat"));

            // Act
            services.AddBasicRepos<TestDbContext>(options => options.EnabledCachedRepositories = false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Assert
            IReadOnlyRepository<Trillig> trilligRepo = serviceProvider.GetService<IReadOnlyRepository<Trillig>>();
            Assert.NotNull(trilligRepo);

            IReadOnlyRepository<Brillig> brilligRepo = serviceProvider.GetService<IReadOnlyRepository<Brillig>>();
            Assert.NotNull(brilligRepo);

            IReadOnlyRepository<Moamrath> moamrathRepo = serviceProvider.GetService<IReadOnlyRepository<Moamrath>>();
            Assert.NotNull(moamrathRepo);
        }


        [Fact]
        public void AddBasicRepos_WithDbContext_registers_an_IKeyedRepository_ForeachDbset()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<TestDbContext>(opts => opts.UseInMemoryDatabase("lolcat"));

            // Act
            services.AddBasicRepos<TestDbContext>(options => options.EnabledCachedRepositories = false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Assert
            var trilligRepo = serviceProvider.GetService<IKeyedRepository<Trillig, int>>();
            Assert.NotNull(trilligRepo);

            var brilligRepo = serviceProvider.GetService<IKeyedRepository<Brillig, string>>();
            Assert.NotNull(brilligRepo);

            var moamrathRepo = serviceProvider.GetService<IKeyedRepository<Moamrath, Guid>>();
            Assert.NotNull(moamrathRepo);
        }

        [Fact]
        public void AddBasicRepos_WithDbContext_registers_an_IKeyedReadOnlyRepository_ForeachDbset()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<TestDbContext>(opts => opts.UseInMemoryDatabase("lolcat"));

            // Act
            services.AddBasicRepos<TestDbContext>(options => options.EnabledCachedRepositories = false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Assert
            var trilligRepo = serviceProvider.GetService<IKeyedReadOnlyRepository<Trillig, int>>();
            Assert.NotNull(trilligRepo);

            var brilligRepo = serviceProvider.GetService<IKeyedReadOnlyRepository<Brillig, string>>();
            Assert.NotNull(brilligRepo);

            var moamrathRepo = serviceProvider.GetService<IKeyedReadOnlyRepository<Moamrath, Guid>>();
            Assert.NotNull(moamrathRepo);
        }

        [Fact]
        public void AddBasicRepos_WithDbContext_registers_an_ICachedRepository_ForeachDbset()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<TestDbContext>(opts => opts.UseInMemoryDatabase("lolcat"));
            services.AddPooledDbContextFactory<TestDbContext>(opts => opts.UseInMemoryDatabase("lolcat"));

            // Act
            services.AddBasicRepos<TestDbContext>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Assert
            var trilligRepo = serviceProvider.GetService<ICachedRepository<Trillig>>();
            Assert.NotNull(trilligRepo);

            var brilligRepo = serviceProvider.GetService<ICachedRepository<Brillig>>();
            Assert.NotNull(brilligRepo);

            var moamrathRepo = serviceProvider.GetService<ICachedRepository<Moamrath>>();
            Assert.NotNull(moamrathRepo);
        }

        [Fact]
        public void AddBasicRepos_With_Enabled_Cached_Repositories_FALSE_registers_NO_ICachedRepository()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<TestDbContext>(opts => opts.UseInMemoryDatabase("lolcat"));

            // Act
            services.AddBasicRepos<TestDbContext>(options => options.EnabledCachedRepositories = false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Assert
            var trilligRepo = serviceProvider.GetService<ICachedRepository<Trillig>>();
            Assert.Null(trilligRepo);

            var brilligRepo = serviceProvider.GetService<ICachedRepository<Brillig>>();
            Assert.Null(brilligRepo);

            var moamrathRepo = serviceProvider.GetService<ICachedRepository<Moamrath>>();
            Assert.Null(moamrathRepo);
        }

        [Fact]
        public void AddBasicRepos_With_EnabledCachedRepositories_TRUE_But_No_Pooled_DbContextFactory_Throws_RepositoryConfiguration()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<TestDbContext>(opts => opts.UseInMemoryDatabase("lolcat"));

            // Act
            var ex = Record.Exception(() => services.AddBasicRepos<TestDbContext>());

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<RepositoryConfigurationException>(ex);
        }

    }
}
