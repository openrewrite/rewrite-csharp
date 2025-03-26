namespace Rewrite.RewriteCSharp.Format;

public static class DictionaryExtensions
{
     /// <summary>
     /// Adds a value to a list associated with the specified key in a dictionary. If the key doesn't exist,
     /// creates a new list and adds the value.
     /// </summary>
     /// <typeparam name="TKey">The type of the dictionary key.</typeparam>
     /// <typeparam name="TValue">The type of values in the lists.</typeparam>
     /// <param name="dict">The dictionary to modify.</param>
     /// <param name="key">The key to find or create.</param>
     /// <param name="value">The value to add to the list.</param>
     /// <returns>True if a new list was created for the key; false if the value was added to an existing list.</returns>
     public static bool Upsert<TKey, TValue>(this IDictionary<TKey, List<TValue>> dict, TKey key, TValue value)
     {
         var isNew = false;
         if (!dict.TryGetValue(key, out var list))
         {
             list = new List<TValue>();
             dict[key] = list;
             isNew = true;
         }
         list.Add(value);
         return isNew;
     }
}
