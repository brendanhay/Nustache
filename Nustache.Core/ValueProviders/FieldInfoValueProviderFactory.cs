using System.Reflection;

namespace Nustache.Core.ValueProviders
{
    internal sealed class FieldInfoValueProviderFactory : ValueProviderFactory
    {
        public override bool TryGetValueProvider(object target, string name, out IValueProvider provider)
        {
            provider = default(IValueProvider);

            var field = target.GetType().GetField(name, DefaultBindingFlags);

            if (field != null) {
                provider = new FieldInfoValueProvider(target, field);
            }

            return provider != null;
        }

        private class FieldInfoValueProvider : IValueProvider
        {
            private readonly object _target;
            private readonly FieldInfo _fieldInfo;

            public FieldInfoValueProvider(object target, FieldInfo fieldInfo)
            {
                _target = target;
                _fieldInfo = fieldInfo;
            }

            public object GetValue()
            {
                return _fieldInfo.GetValue(_target);
            }
        }
    }
}