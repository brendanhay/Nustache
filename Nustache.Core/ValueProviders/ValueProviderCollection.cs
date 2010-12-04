using System.Collections.Generic;
using System.Collections.ObjectModel;
using Nustache.Core.Conventions;

namespace Nustache.Core.ValueProviders
{
    internal sealed class ValueProviderCollection : Collection<IValueProviderFactory>, IValueProviderCollection
    {
        private readonly INamingConvention _namingConvention;

        public ValueProviderCollection() : this(NamingConvention.None) { }

        public ValueProviderCollection(NamingConvention namingConvention)
            : this(NamingConventionFactory.Create(namingConvention)) { }

        public ValueProviderCollection(INamingConvention namingConvention)
            : this(namingConvention, new IValueProviderFactory[] { 
                new PropertyDescriptorValueProviderFactory(),
                new PropertyInfoValueProviderFactory(),
                new DictionaryValueProviderFactory(),
                new GenericDictionaryValueProviderFactory(),
                new FieldInfoValueProviderFactory(),
                new MethodInfoValueProviderFactory() 
            }) { }

        public ValueProviderCollection(INamingConvention namingConvention,
            IEnumerable<IValueProviderFactory> factories)
        {
            _namingConvention = namingConvention;

            foreach (var factory in factories) {
                Add(factory);
            }
        }

        public bool TryGetValueProvider(object target, string name, out IValueProvider provider)
        {
            provider = default(IValueProvider);

            foreach (var factory in this) {
                if (factory.TryGetValueProvider(target, _namingConvention.Format(name), out provider)) {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetValue(object target, string name, out object value)
        {
            value = null;
            IValueProvider provider;

            if (TryGetValueProvider(target, name, out provider)) {
                value = provider.GetValue();
            }

            return value != null;
        }
    }
}
