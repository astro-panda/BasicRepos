using BasicRepos.Test.Setup;

namespace BasicRepos.Test.Repositories
{
    public class TrilligRepository : IntKeyedRepository<Trillig>
    {
        public TrilligRepository(TestDbContext db) : base(db)
        {
        }
    }
}
