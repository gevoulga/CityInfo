using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace CityInfo.API.Utils;

public static class ClassExtensions
{
    public static dynamic ToAnonymous(this IDictionary<string, object> dictionary)
    {
        // https://stackoverflow.com/questions/7595416/convert-dictionarystring-object-to-anonymous-object
        dynamic eo = dictionary.Aggregate(
            new ExpandoObject() as IDictionary<string, object>,
            (dict, kv) =>
            {
                dict.Add(kv.Key, kv.Value);
                return dict;
            });
        return eo;
    }

    public static IDictionary<string, object> AsDictionary(
        this object source,
        BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
    {
        // https://stackoverflow.com/questions/4943817/mapping-object-to-dictionary-and-vice-versa
        return source.GetType().GetProperties(bindingAttr).ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(source, null)
        );
    }

    public static T ToObject<T>(this IDictionary<string, object> dictionary)
        where T : class, new()
    {
        return dictionary.Aggregate(new T(), (obj, kv) =>
            {
                obj.GetType()
                    .GetProperty(kv.Key)
                    ?.SetValue(obj, kv.Value, null);
                return obj;
            });
    }


}