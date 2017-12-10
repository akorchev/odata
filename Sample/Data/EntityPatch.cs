using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Data
{
    public static class EntityPatch
    {
        public static void Apply(object obj, JObject patch)
        {
            var typeInfo = obj.GetType().GetTypeInfo();

            foreach (var property in patch)
            {
                var propertyInfo = typeInfo.GetProperty(property.Key);

                if (propertyInfo != null)
                {
                    var computedAttribute = propertyInfo.GetCustomAttributes(typeof(DatabaseGeneratedAttribute), false)
                             .Cast<DatabaseGeneratedAttribute>().FirstOrDefault();
                    if(computedAttribute == null) {
                        var value = property.Value.ToObject(propertyInfo.PropertyType);
                        propertyInfo.SetValue(obj, value);
                    }
                }
            }

        }
    }
}
