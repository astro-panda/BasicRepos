using AstroPanda.Data.Test.Setup;

namespace AstroPanda.Data.Test.Repositories
{
    public class KeylessRepository : RepositoryBase<Moamrath>
    {
        public KeylessRepository(TestDbContext db) : base(db) { }
    }
}
