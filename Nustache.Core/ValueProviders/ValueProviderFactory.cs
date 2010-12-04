using System.Reflection;

namespace Nustache.Core.ValueProviders
{
    internal abstract class ValueProviderFactory : IValueProviderFactory
    {
        protected const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Instance;

        public abstract bool TryGetValueProvider(object target, string name, out IValueProvider provider);
    }
}
