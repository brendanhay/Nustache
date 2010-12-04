using System.Reflection;

namespace Nustache.Core.ValueProviders
{
    internal sealed class MethodInfoValueProviderFactory : ValueProviderFactory
    {
        public override bool TryGetValueProvider(object target, string name, out IValueProvider provider)
        {
            provider = default(IValueProvider);
            var methods = target.GetType().GetMember(name, MemberTypes.Method, DefaultBindingFlags);

            foreach (MethodInfo method in methods) {
                if (method.ReturnType != typeof(void) && method.GetParameters().Length == 0) {
                    provider = new MethodInfoValueProvider(target, method);
                }
            }

            return provider != null;
        }

        private class MethodInfoValueProvider : IValueProvider
        {
            private readonly object _target;
            private readonly MethodInfo _methodInfo;

            public MethodInfoValueProvider(object target, MethodInfo methodInfo)
            {
                _target = target;
                _methodInfo = methodInfo;
            }

            public object GetValue()
            {
                return _methodInfo.Invoke(_target, null);
            }
        }
    }
}

