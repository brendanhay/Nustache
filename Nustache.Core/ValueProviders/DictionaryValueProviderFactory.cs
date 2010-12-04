using System.Collections;

namespace Nustache.Core.ValueProviders
{
    internal sealed class DictionaryValueProviderFactory : ValueProviderFactory
    {
        public override bool TryGetValueProvider(object target, string name, out IValueProvider provider)
        {
            provider = default(IValueProvider);

            if (target is IDictionary) {
                var dictionary = (IDictionary)target;

                if (dictionary.Contains(name)) {
                    provider = new DictionaryValueProvider(dictionary, name);
                }
            }

            return provider != null;
        }

        private class DictionaryValueProvider : IValueProvider
        {
            private readonly IDictionary _target;
            private readonly string _key;

            public DictionaryValueProvider(IDictionary target, string key)
            {
                _target = target;
                _key = key;
            }

            public object GetValue()
            {
                return _target[_key];
            }
        }
    }
}