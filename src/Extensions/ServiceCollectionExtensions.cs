using BasicRepos.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BasicRepos;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBasicRepos<TContext>(this IServiceCollection services, Action<RepositoryOptions> configure = null) where TContext : DbContext
    {
        RepositoryOptions options = new();
        configure?.Invoke(options);
        return ScaffoldBasicRepos<TContext>(services, options);
    }

    private static IServiceCollection ScaffoldBasicRepos<TContext>(IServiceCollection services, RepositoryOptions options) where TContext : DbContext
    {
        var serviceProvider = services.BuildServiceProvider();
        TContext dbContext = serviceProvider.GetRequiredService<TContext>();
        Type dbContextType = dbContext.GetType();

        IEnumerable<Type> entityTypes = dbContext.Model.GetEntityTypes().Select(x => x.ClrType.GetTypeInfo());

        BindBasicRepos(services, dbContextType, entityTypes, options);
        BindKeyedBasicRepos(services, dbContextType, entityTypes, options);

        if (options.EnabledCachedRepositories)
        {
            BindCachedBasicRepos<TContext>(services, dbContextType, entityTypes);
        }

        return services;
    }

    private static IServiceCollection BindBasicRepos(IServiceCollection services,
                                                     Type dbContextType,
                                                     IEnumerable<Type> entityTypes,
                                                     RepositoryOptions options)
    {
        Type writeRepo = typeof(IRepository<>);
        Type readRepo = typeof(IReadOnlyRepository<>);
        Type defaultRepo = typeof(DefaultRepository<,>);            

        foreach (var entityType in entityTypes)
        {
            services.TryAddTransient(writeRepo.MakeGenericType(entityType),
                                     defaultRepo.MakeGenericType(entityType, dbContextType));

            services.TryAddTransient(readRepo.MakeGenericType(entityType),
                                     defaultRepo.MakeGenericType(entityType, dbContextType));
        }

        return services;
    }

    private static IServiceCollection BindKeyedBasicRepos(IServiceCollection services,
                                                          Type dbContextType,
                                                          IEnumerable<Type> entityTypes,
                                                          RepositoryOptions options)
    {
        Type writeKeyedRepo = typeof(IKeyedRepository<,>);
        Type readKeyedRepo = typeof(IKeyedReadOnlyRepository<,>);
        Type defaultKeyedRepo = typeof(DefaultKeyedRepository<,,>);

        Dictionary<Type, Type> typeMap = new Dictionary<Type, Type>();

        foreach(var x in entityTypes)
        {
            PropertyInfo[] props = x.GetProperties();
            PropertyInfo idProp = props.Where(x => x.Name == "Id").FirstOrDefault();
            var interfaces = x.GetInterfaces().Where(x => x.IsGenericType);

            if (idProp is null)
                continue;

            if(interfaces.Any(x => x.GetGenericArguments().Contains(idProp.PropertyType)))                
                typeMap.Add(x, idProp.PropertyType);                
        }


        foreach (var keyedEntity in typeMap)
        {
            services.TryAddTransient(writeKeyedRepo.MakeGenericType(keyedEntity.Key, keyedEntity.Value),
                                    defaultKeyedRepo.MakeGenericType(keyedEntity.Key, keyedEntity.Value, dbContextType));

            services.TryAddTransient(readKeyedRepo.MakeGenericType(keyedEntity.Key, keyedEntity.Value),
                                    defaultKeyedRepo.MakeGenericType(keyedEntity.Key, keyedEntity.Value, dbContextType));
        }

        return services;
    }

    private static IServiceCollection BindCachedBasicRepos<TContext>(IServiceCollection services,
                                                           Type dbContextType,
                                                           IEnumerable<Type> entityTypes) where TContext : DbContext
    {
        ValidateCachedRepositoryEnvironment<TContext>(services);

        Type cachedRepoType = typeof(ICachedRepository<>);        
        Type defaultCachedRepo = typeof(DefaultCachedRepository<,>);
        Type dbContextFactoryType = typeof(IDbContextFactory<>);

        foreach (var entityType in entityTypes)
        {
            services.TryAddScoped(cachedRepoType.MakeGenericType(entityType), sp =>
            {
                return Activator.CreateInstance(defaultCachedRepo.MakeGenericType(entityType, dbContextType), sp.GetService<IDbContextFactory<TContext>>());
            });
        }

        return services;
    }

    private static void ValidateCachedRepositoryEnvironment<TContext>(IServiceCollection services) where TContext : DbContext
    {
        if (services.Any(x => x.ServiceType == typeof(IDbContextFactory<TContext>)) == false)
            throw new RepositoryConfigurationException($"Cannot use Cached Repositories without DbContext Factory Pooling. If you need cached repositories, please enabled DbContext Pooling with 'services.AddPooledDbContextFactory<TContext>()'");
    }
}
