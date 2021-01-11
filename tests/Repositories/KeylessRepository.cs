using BasicRepos.Test.Setup;

namespace BasicRepos.Test.Repositories
{
    public class KeylessRepository : RepositoryBase<Trillig>
    {
        public KeylessRepository(TestDbContext db) : base(db) { }
    }
}
