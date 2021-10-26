using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BasicRepos
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBasicRepos(this IServiceCollection services, params DbContext[] dbContexts)
        {
            return BindBasicRepos(services, dbContexts);
        }

        internal static IServiceCollection BindBasicRepos(IServiceCollection services, DbContext[] dbContexts)
        {
            Dictionary<DbContext, IEnumerable<Type>> entityTypeDictionary = new Dictionary<DbContext, IEnumerable<Type>>();

            Type writeRepo = typeof(IRepository<>);
            Type readRepo = typeof(IReadOnlyRepository<>);
            Type writeKeyedRepo = typeof(IKeyedRepository<,>);
            Type readKeyedRepo = typeof(IKeyedReadOnlyRepository<,>);
            Type defaultRepo = typeof(DefaultRepository<,>);
            Type defaultKeyedRepo = typeof(DefaultKeyedRepository<,,>);

            foreach (var dbContext in dbContexts)
            {
                var types = dbContext.Model.GetEntityTypes();
                entityTypeDictionary.Add(dbContext, types.Select(x => x.ClrType));
            }

            foreach(var entitymap in entityTypeDictionary)
            {
                Type dbContextType = entitymap.Key.GetType();

                foreach (var entityType in entitymap.Value)
                {
                    Type concreteEntityType = entityType.GetType();
                    services.TryAddTransient(writeRepo.MakeGenericType(concreteEntityType), defaultRepo.MakeGenericType(concreteEntityType, dbContextType));
                    services.TryAddTransient(readRepo.MakeGenericType(concreteEntityType), defaultRepo.MakeGenericType(concreteEntityType, dbContextType));

                    if(concreteEntityType.IsAssignableFrom(typeof(IKeyedEntity<>)))
                    {
                        PropertyInfo[] props = concreteEntityType.GetTypeInfo().GetProperties(BindingFlags.Public);
                        PropertyInfo idProp = props.Where(x => x.Name == "Id").FirstOrDefault();

                        if(idProp != null)
                        {
                            services.TryAddTransient(writeKeyedRepo.MakeGenericType(concreteEntityType), defaultRepo.MakeGenericType(concreteEntityType, idProp.PropertyType, dbContextType));
                            services.TryAddTransient(writeKeyedRepo.MakeGenericType(concreteEntityType), defaultRepo.MakeGenericType(concreteEntityType, idProp.PropertyType, dbContextType));
                        }
                    }
                }
            }

            return services;
        }
    }
}
