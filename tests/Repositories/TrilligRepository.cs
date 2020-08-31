using AstroPanda.Data.Test.Setup;

namespace AstroPanda.Data.Test.Repositories
{
    public class TrilligRepository : IntKeyedRepository<Trillig>
    {
        public TrilligRepository(TestDbContext db) : base(db)
        {
        }
    }
}
