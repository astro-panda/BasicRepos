using System.ComponentModel.DataAnnotations;

namespace BasicRepos.Test.Setup
{
    public class Brillig : IKeyedEntity<string>
    {
        [Key]
        public string Id { get; set; }
        public int Heads { get; set; }
        public int Teeth { get; set; }
    }
}
