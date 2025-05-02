using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace MSProfessionals.Infrastructure.Extensions
{
    public class DictionaryComparer<TKey, TValue> : ValueComparer<Dictionary<TKey, TValue>>
    {
        public DictionaryComparer() : base(
            (d1, d2) => d1.Count == d2.Count && !d1.Except(d2).Any(),
            d => d.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            d => new Dictionary<TKey, TValue>(d))
        {
        }
    }
} 