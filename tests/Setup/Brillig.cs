using System.ComponentModel.DataAnnotations;

namespace AstroPanda.Data.Test.Setup
{
    public class Brillig : IKeyedEntity<string>
    {
        [Key]
        public string Id { get; set; }
        public int Heads { get; set; }
        public int Teeth { get; set; }
    }
}
