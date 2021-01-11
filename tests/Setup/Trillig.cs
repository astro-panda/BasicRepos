using System.ComponentModel.DataAnnotations;

namespace BasicRepos.Test.Setup
{
    public class Trillig : IKeyedEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrotherName { get; set; }
    }
}
