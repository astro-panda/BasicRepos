using BasicRepos.Test.Setup;

namespace BasicRepos.Test.Repositories
{
    public class MoamrathRepository : GuidKeyedRepository<Moamrath>
    {
        public MoamrathRepository(TestDbContext db) : base(db)
        {
        }
    }
}
