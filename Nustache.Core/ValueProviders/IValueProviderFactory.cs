namespace Nustache.Core.ValueProviders
{
    public interface IValueProviderFactory
    {
        bool TryGetValueProvider(object target, string name, out IValueProvider provider);
    }
}
