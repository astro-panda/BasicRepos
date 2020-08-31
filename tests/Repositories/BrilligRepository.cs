using AstroPanda.Data.Test.Setup;

namespace AstroPanda.Data.Test.Repositories
{
    public class BrilligRepository : StringKeyedRepository<Brillig>
    {
        public BrilligRepository(TestDbContext db) : base(db)
        {
        }
    }
}
