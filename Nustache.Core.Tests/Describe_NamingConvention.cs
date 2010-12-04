using System;
using NUnit.Framework;
using Nustache.Core.Conventions;
using Nustache.Core.ValueProviders;

namespace Nustache.Core.Tests
{
    [TestFixture]
    public class Describe_NamingConvention
    {
        [Test]
        public void Any_name_gets_pascalcased_field_values()
        {
            AssertNamesGetValues(PascalCased.Instance);
        }

        [Test]
        public void Any_name_gets_camelcased_field_values()
        {
            AssertNamesGetValues(CamelCased.Instance);
        }

        [Test]
        public void Any_name_gets_underscored_field_values()
        {
            AssertNamesGetValues(Underscored.Instance);
        }

        #region Static Assertion Helpers

        public static void AssertNamesGetValues<T>(T target) where T : Target
        {
            foreach (var name in _names) {
                AssertGetValue(target, name);
            }
        }

        private static void AssertGetValue<T>(T target, string name) where T : Target
        {
            var providers = new ValueProviderCollection(target.NameConvention);
            var message = string.Format("Couldn't retrieve name: '{0}', from type: '{1}'",
                name, target.GetType().Name);

            object result;

            Assert.IsTrue(providers.TryGetValue(target, name, out result), message);
            Assert.AreEqual(target.Expected, result);
        }

        #endregion

        #region Data

        private static readonly Random _random = new Random();

        private static readonly string[] _names = {
            "FieldName", "PropertyName", "MethodName",
            "fieldName", "propertyName", "methodName",
            "field_name", "property_name", "method_name",
            "Field_name", "Property_name", "Method_name",
            "Field_Name", "Property_Name", "Method_Name"
        };

        public sealed class PascalCased : Target
        {
            public static readonly PascalCased Instance = new PascalCased();

            public int FieldName = _random.Next();
            public int PropertyName { get { return FieldName; } set { FieldName = value; } }
            public int MethodName() { return FieldName; }

            public override NamingConvention NameConvention
            {
                get { return NamingConvention.PascalCased; }
            }

            public override int Expected { get { return FieldName; } }
        }

        public sealed class CamelCased : Target
        {
            public static readonly CamelCased Instance = new CamelCased();

            public int fieldName = _random.Next();
            public int propertyName { get { return fieldName; } set { fieldName = value; } }
            public int methodName() { return fieldName; }

            public override NamingConvention NameConvention
            {
                get { return NamingConvention.CamelCased; }
            }

            public override int Expected { get { return fieldName; } }
        }

        public sealed class Underscored : Target
        {
            public static readonly Underscored Instance = new Underscored();

            public int field_name = _random.Next();
            public int property_name { get { return field_name; } set { field_name = value; } }
            public int method_name() { return field_name; }

            public override NamingConvention NameConvention
            {
                get { return NamingConvention.Underscored; }
            }

            public override int Expected { get { return field_name; } }
        }

        public abstract class Target
        {
            public abstract NamingConvention NameConvention { get; }

            public abstract int Expected { get; }
        }

        #endregion
    }
}