using System.Collections;
using System.Collections.Generic;
using System.Data;
using Moq;
using NUnit.Framework;
using Nustache.Core.ValueProviders;

namespace Nustache.Core.Tests
{
    [TestFixture]
    public class Describe_ValueProviderCollection
    {
        private readonly IValueProviderCollection _providers = new ValueProviderCollection();

        [Test]
        public void It_returns_false_when_it_cant_get_a_value()
        {
            object value;

            Assert.IsFalse(_providers.TryGetValue(this, "x", out value));
        }

        [Test]
        public void It_gets_field_values()
        {
            ReadWriteInts target = new ReadWriteInts();
            target.IntField = 123;

            object actual;

            Assert.IsTrue(_providers.TryGetValue(target, "IntField", out actual));
            Assert.AreEqual(123, actual);
        }

        [Test]
        public void It_gets_property_values()
        {
            ReadWriteInts target = new ReadWriteInts();
            target.IntField = 123;

            object actual;

            Assert.IsTrue(_providers.TryGetValue(target, "IntProperty", out actual));
            Assert.AreEqual(123, actual);
        }

        [Test]
        public void It_gets_method_values()
        {
            ReadWriteInts target = new ReadWriteInts();
            target.IntField = 123;

            object actual;

            Assert.IsTrue(_providers.TryGetValue(target, "IntMethod", out actual));
            Assert.AreEqual(123, actual);
        }

        [Test]
        public void It_cant_get_values_from_write_only_properties()
        {
            WriteOnlyInts target = new WriteOnlyInts();

            object actual;

            Assert.IsFalse(_providers.TryGetValue(target, "IntProperty", out actual));
            Assert.IsNull(actual);
        }

        [Test]
        public void It_cant_get_values_from_write_only_methods()
        {
            WriteOnlyInts target = new WriteOnlyInts();

            object actual;

            Assert.IsFalse(_providers.TryGetValue(target, "IntMethod", out actual));
            Assert.IsNull(actual);
        }

        [Test]
        public void It_gets_Hashtable_values()
        {
            Hashtable target = new Hashtable();
            target["IntKey"] = 123;

            object actual;

            Assert.IsTrue(_providers.TryGetValue(target, "IntKey", out actual));
            Assert.AreEqual(123, actual);
        }

        [Test]
        public void It_gets_Dictionary_values()
        {
            Dictionary<string, int> target = new Dictionary<string, int>();
            target["IntKey"] = 123;

            object actual;

            Assert.IsTrue(_providers.TryGetValue(target, "IntKey", out actual));
            Assert.AreEqual(123, actual);
        }

        [Test]
        public void It_gets_GenericDictionary_values()
        {
            var mock = new Mock<IDictionary<string, int>>();
            mock.Setup(x => x.ContainsKey("Key")).Returns(true);
            mock.Setup(x => x["Key"]).Returns(123);
            IDictionary<string, int> target = mock.Object;

            object actual;

            Assert.IsTrue(_providers.TryGetValue(target, "Key", out actual));
            Assert.AreEqual(123, actual);
        }

        [Test]
        public void It_gets_DataRowView_values()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IntColumn", typeof(int));
            dt.Rows.Add(new object[] { 123 });
            DataRowView target = dt.DefaultView[0];

            object actual;

            Assert.IsTrue(_providers.TryGetValue(target, "IntColumn", out actual));
            Assert.AreEqual(123, actual);
        }

        public class ReadWriteInts
        {
            public int IntField = -1;
            public int IntProperty { get { return IntField; } set { IntField = value; } }
            public int IntMethod() { return IntField; }
            public void IntMethod(int value) { IntField = value; }
        }

        public class ReadOnlyInts
        {
            public readonly int IntField = -1;
            public int IntProperty { get { return IntField; } }
            public int IntMethod() { return IntField; }
        }

        public class WriteOnlyInts
        {
            public int IntField; // Write only?
            public int IntProperty { set { IntField = value; } }
            public void IntMethod(int value) { IntField = value; }
        }
    }
}