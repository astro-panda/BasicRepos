using Microsoft.EntityFrameworkCore;

namespace AstroPanda.Data
{
    public class StringKeyedRepository<T> : KeyedRepositoryBase<T, string> where T : class, IKeyedEntity<string>
    {
        public StringKeyedRepository(DbContext db) : base(db) { }        
    }
}
