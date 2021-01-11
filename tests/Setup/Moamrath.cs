using System;
using System.ComponentModel.DataAnnotations;

namespace BasicRepos.Test
{
    public class Moamrath : IKeyedEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public string Sound { get; set; }
        public int Legs { get; set; }        
    }
}
