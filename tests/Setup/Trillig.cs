using System.ComponentModel.DataAnnotations;

namespace AstroPanda.Data.Test.Setup
{
    public class Trillig : IKeyedEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrotherName { get; set; }
    }
}
