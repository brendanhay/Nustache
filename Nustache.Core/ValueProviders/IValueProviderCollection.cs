using System.Collections.Generic;

namespace Nustache.Core.ValueProviders
{
    public interface IValueProviderCollection : ICollection<IValueProviderFactory>, IValueProviderFactory
    {
        bool TryGetValue(object target, string name, out object value);
    }
}
