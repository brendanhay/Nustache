namespace Nustache.Core.Conventions
{
    public static class NamingConventionFactory
    {
        private static readonly INamingConvention _default = Create(NamingConvention.None);

        public static INamingConvention Default { get { return _default; } }

        public static INamingConvention Create(NamingConvention convention)
        {
            var formatter = new Formatter(DefaultFormatter);

            switch (convention) {
                case NamingConvention.None:
                    break;
                case NamingConvention.PascalCased:
                    formatter = Inflector.Pascalize;
                    break;
                case NamingConvention.CamelCased:
                    formatter = Inflector.Camelize;
                    break;
                case NamingConvention.Underscored:
                    formatter = Inflector.Underscore;
                    break;
            }

            return new InflectorNamingConvention(formatter);
        }

        private static string DefaultFormatter(string name)
        {
            return name;
        }

        private delegate string Formatter(string name);

        private class InflectorNamingConvention : INamingConvention
        {
            private readonly Formatter _formatter;

            public InflectorNamingConvention(Formatter formatter)
            {
                _formatter = formatter;
            }

            public string Format(string name)
            {
                return _formatter(name);
            }
        }
    }
}
