using BasicRepos.Test.Setup;

namespace BasicRepos.Test.Repositories
{
    public class BrilligRepository : StringKeyedRepository<Brillig>
    {
        public BrilligRepository(TestDbContext db) : base(db)
        {
        }
    }
}
