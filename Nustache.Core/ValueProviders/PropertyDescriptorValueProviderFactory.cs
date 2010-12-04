using System.ComponentModel;

namespace Nustache.Core.ValueProviders
{
    internal sealed class PropertyDescriptorValueProviderFactory : ValueProviderFactory
    {
        public override bool TryGetValueProvider(object target, string name, out IValueProvider provider)
        {
            provider = default(IValueProvider);

            if (target is ICustomTypeDescriptor) {
                var descriptor = (ICustomTypeDescriptor)target;

                foreach (PropertyDescriptor property in descriptor.GetProperties()) {
                    if (property.Name == name) {
                        provider = new PropertyDescriptorValueProvider(target, property);
                    }
                }
            }

            return provider != null;
        }

        private class PropertyDescriptorValueProvider : IValueProvider
        {
            private readonly object _target;
            private readonly PropertyDescriptor _descriptor;

            public PropertyDescriptorValueProvider(object target, PropertyDescriptor descriptor)
            {
                _target = target;
                _descriptor = descriptor;
            }

            public object GetValue()
            {
                return _descriptor.GetValue(_target);
            }
        }
    }
}
