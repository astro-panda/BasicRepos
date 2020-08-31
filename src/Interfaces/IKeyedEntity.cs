
using System;

namespace AstroPanda.Data {

    public interface IKeyedEntity<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }        
    }
}