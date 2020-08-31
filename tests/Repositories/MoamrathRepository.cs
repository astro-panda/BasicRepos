using AstroPanda.Data.Test.Setup;

namespace AstroPanda.Data.Test.Repositories
{
    public class MoamrathRepository : GuidKeyedRepository<Moamrath>
    {
        public MoamrathRepository(TestDbContext db) : base(db)
        {
        }
    }
}
