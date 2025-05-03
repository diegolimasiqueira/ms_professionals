using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;

namespace MSProfessionals.Infrastructure.Extensions;

public class DictionaryComparer<TKey, TValue> : ValueComparer<Dictionary<TKey, TValue>>
    where TKey : notnull
{
    public DictionaryComparer() : base(
        (d1, d2) => d1 != null && d2 != null && d1.Count == d2.Count && !d1.Except(d2).Any(),
        d => d != null ? d.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())) : 0,
        d => d != null ? new Dictionary<TKey, TValue>(d) : new Dictionary<TKey, TValue>())
    {
    }
} 