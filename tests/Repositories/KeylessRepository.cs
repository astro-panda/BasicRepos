using AstroPanda.Data.Test.Setup;

namespace AstroPanda.Data.Test.Repositories
{
    public class KeylessRepository : RepositoryBase<Trillig>
    {
        public KeylessRepository(TestDbContext db) : base(db) { }
    }
}
