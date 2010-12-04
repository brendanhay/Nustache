using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nustache.Core.ValueProviders
{
    internal sealed class GenericDictionaryValueProviderFactory : ValueProviderFactory
    {
        public override bool TryGetValueProvider(object target, string name, out IValueProvider provider)
        {
            provider = default(IValueProvider);
            var dictionaryType = default(Type);

            foreach (var interfaceType in target.GetType().GetInterfaces()) {
                if (interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>) &&
                    interfaceType.GetGenericArguments()[0] == typeof(string)) {
                    dictionaryType = interfaceType;

                    break;
                }
            }

            if (dictionaryType != null) {
                var containsKeyMethod = dictionaryType.GetMethod("ContainsKey");

                if ((bool)containsKeyMethod.Invoke(target, new object[] { name })) {
                    provider = new GenericDictionaryValueProvider(target, name, dictionaryType);
                }
            }

            return provider != null;
        }

        private class GenericDictionaryValueProvider : IValueProvider
        {
            private readonly object _target;
            private readonly string _key;
            private readonly MethodInfo _getMethod;

            public GenericDictionaryValueProvider(object target, string key, Type dictionaryType)
            {
                _target = target;
                _key = key;
                _getMethod = dictionaryType.GetMethod("get_Item");
            }

            public object GetValue()
            {
                return _getMethod.Invoke(_target, new object[] { _key });
            }
        }
    }
}
