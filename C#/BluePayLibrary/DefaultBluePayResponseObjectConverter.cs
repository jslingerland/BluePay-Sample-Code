using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Text;

namespace BluePayLibrary.Interfaces
{
    public class DefaultBluePayResponseObjectConverter<T> : IBluePayResponseObjectConverter<T> where T:new()
    {
        private static readonly Type Type = typeof(T);
        private readonly ConcurrentDictionary<string, string> _propertyNameMap = new ConcurrentDictionary<string, string>();

        public void SetValue(T obj, string property, string value)
        {
            var name = GetNormalizedPropertyName(property);

            var prop = Type.GetProperty(name);
            if (prop == null)
            {
                var rs = obj as BluePayMessage;
                if (rs != null)
                {
                    rs[name] = value;
                    return;
                }
                else
                    throw new ArgumentException(
                        $"Property '{name}' ({property}) does not exist on Type '{Type.FullName}'", nameof(property));
            }

            
            if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(obj, value, null);
                return;
            }
            
            var converter = TypeDescriptor.GetConverter(prop.PropertyType);
            if (converter == null || !converter.CanConvertFrom(typeof(string)))
            {
                throw new ArgumentException($"Property '{name}' ({property}) of type '{prop.PropertyType.FullName}' cannot convert from a string", nameof(property));
            }

            if(!string.IsNullOrEmpty(value))
                prop.SetValue(obj, converter.ConvertFromString(value), null);
        }

        public T Create()
        {
            return new T();
        }

        protected virtual string NormalizePropertyName(string pn)
        {
            var ret = new StringBuilder(pn.Length);

            var nextIsUpper = true;

            foreach (var c in pn)
            {
                if (!char.IsLetter(c))
                {
                    nextIsUpper = true;
                    if (!char.IsDigit(c))
                        continue; //only letters or digits
                    else
                        ret.Append(c);
                }
                else
                {
                    ret.Append(nextIsUpper ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c));
                    nextIsUpper = false;
                }
            }

            return ret.ToString();
        }

        public string GetNormalizedPropertyName(string pn)
        {
            return _propertyNameMap.GetOrAdd(pn, NormalizePropertyName);
        }
    }
}