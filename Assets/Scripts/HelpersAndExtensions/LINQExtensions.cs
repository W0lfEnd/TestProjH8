using System.Collections.Generic;


public static class LINQExtensions
{
  public static TValue safeGet<TKey, TValue>( this IReadOnlyDictionary<TKey, TValue> dict, TKey key )
  {
    TValue obj;
    return dict != null && (object)key != null && dict.TryGetValue( key, out obj ) ? obj : default( TValue );
  }
}