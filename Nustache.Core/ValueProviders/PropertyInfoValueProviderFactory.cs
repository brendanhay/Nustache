using System.Reflection;

namespace Nustache.Core.ValueProviders
{
    internal sealed class PropertyInfoValueProviderFactory : ValueProviderFactory
    {
        public override bool TryGetValueProvider(object target, string name, out IValueProvider provider)
        {
            provider = default(IValueProvider);

            var property = target.GetType().GetProperty(name, DefaultBindingFlags);

            if (property != null && property.CanRead) {
                provider = new PropertyInfoValueProvider(target, property);
            }

            return provider != null;
        }

        private class PropertyInfoValueProvider : IValueProvider
        {
            private readonly object _target;
            private readonly PropertyInfo _property;

            public PropertyInfoValueProvider(object target, PropertyInfo property)
            {
                _target = target;
                _property = property;
            }

            public object GetValue()
            {
                return _property.GetValue(_target, null);
            }
        }
    }
}
