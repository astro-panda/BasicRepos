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
    public static IServiceCollection AddBasicRepos<TContext>(this IServiceCollection services) where TContext : DbContext, new()
    {
        return ScaffoldBasicRepos<TContext>(services);
    }

    public static IServiceCollection ScaffoldBasicRepos<TContext>(IServiceCollection services) where TContext : DbContext, new()
    {
        var serviceProvider = services.BuildServiceProvider();
        TContext dbContext = serviceProvider.GetRequiredService<TContext>();
        Type dbContextType = dbContext.GetType();

        IEnumerable<Type> entityTypes = dbContext.Model.GetEntityTypes().Select(x => x.ClrType.GetTypeInfo());

        BindBasicRepos(services, dbContextType, entityTypes);
        BindKeyedBasicRepos(services, dbContextType, entityTypes);
        BindCachedBasicRepos(services, dbContextType, entityTypes);

        return services;
    }

    private static IServiceCollection BindBasicRepos(IServiceCollection services, Type dbContextType, IEnumerable<Type> entityTypes)
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

    private static IServiceCollection BindKeyedBasicRepos(IServiceCollection services, Type dbContextType, IEnumerable<Type> entityTypes)
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

    private static IServiceCollection BindCachedBasicRepos(IServiceCollection services, Type dbContextType, IEnumerable<Type> entityTypes)
    {
        Type cachedRepoType = typeof(ICachedRepository<>);        
        Type defaultCachedRepo = typeof(DefaultCachedRepository<,>);

        foreach (var entityType in entityTypes)
        {
            services.TryAddScoped(cachedRepoType.MakeGenericType(entityType),
                                     defaultCachedRepo.MakeGenericType(entityType, dbContextType));
        }

        return services;
    }
}
