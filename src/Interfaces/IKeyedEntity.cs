using System;

namespace BasicRepos
{
    public interface IKeyedEntity<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }        
    }
}