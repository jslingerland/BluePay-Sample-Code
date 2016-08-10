using System;
using System.Collections.Generic;
using BluePayLibrary.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BluePayLibrary.Test
{
    [TestClass]
    public class DefaultBluePayResponseObjectConverterTest
    {
        [TestMethod]
        public void CreatesInstance()
        {
            var converter = new DefaultBluePayResponseObjectConverter<TestResponseObject>();
            var obj = converter.Create();
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void ThrowsOnMissingPropertyNonDynamic()
        {
            var converter = new DefaultBluePayResponseObjectConverter<TestResponseObject>();
            var obj = new TestResponseObject();

            try
            {
                converter.SetValue(obj, "Not_Exists", "Value");
                Assert.Fail("Expected ArgumentException");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual($"Property 'NotExists' (Not_Exists) does not exist on Type '{typeof(TestResponseObject).FullName}'\r\nParameter name: property", ex.Message);
            }
        }

        [TestMethod]
        public void SetsMissingPropertyDynamic()
        {
            var converter = new DefaultBluePayResponseObjectConverter<TestResponseObjectDynamic>();
            dynamic obj = new TestResponseObjectDynamic();

            converter.SetValue(obj, "Not_Exists", "Value");
            Assert.AreEqual("Value", obj.NotExists);
        }

        [TestMethod]
        public void ThrowsOnUnconvertableProperty()
        {
            var converter = new DefaultBluePayResponseObjectConverter<TestResponseObject>();
            var obj = new TestResponseObject();

            try
            {
                converter.SetValue(obj, "Unconvertable", "Value");
                Assert.Fail("Expected ArgumentException");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual($"Property 'Unconvertable' (Unconvertable) of type '{typeof(List<object>).FullName}' cannot convert from a string\r\nParameter name: property", ex.Message);
            }
        }

        [TestMethod]
        public void MapsIntValues()
        {
            var converter = new DefaultBluePayResponseObjectConverter<TestResponseObject>();
            var obj = new TestResponseObject();

            converter.SetValue(obj, "Int_Value", "1");
            Assert.AreEqual(1, obj.IntValue);
        }

        [TestMethod]
        public void MapsStringValues()
        {
            var converter = new DefaultBluePayResponseObjectConverter<TestResponseObject>();
            var obj = new TestResponseObject();

            converter.SetValue(obj, "String_Value", "ABC");
            Assert.AreEqual("ABC", obj.StringValue);
        }

        [TestMethod]
        public void MapsBoolValues()
        {
            var converter = new DefaultBluePayResponseObjectConverter<TestResponseObject>();
            var obj = new TestResponseObject();

            converter.SetValue(obj, "Bool_Value", "true");
            Assert.AreEqual(true, obj.BoolValue);
        }

        [TestMethod]
        public void MapsEnumValues()
        {
            var converter = new DefaultBluePayResponseObjectConverter<TestResponseObject>();
            var obj = new TestResponseObject();

            converter.SetValue(obj, "Enum_Value", "2");
            Assert.AreEqual(TestEnum.Two, obj.EnumValue);
        }

        [TestMethod]
        public void MapsEnumValues2()
        {
            var converter = new DefaultBluePayResponseObjectConverter<TestResponseObject>();
            var obj = new TestResponseObject();

            converter.SetValue(obj, "Enum_Value", "Two");
            Assert.AreEqual(TestEnum.Two, obj.EnumValue);
        }

        public class TestResponseObjectDynamic : BluePayMessage
        {
            public int IntValue { get; set; }
            public string StringValue { get; set; }
            public bool BoolValue { get; set; }
            public TestEnum EnumValue { get; set; }
            public List<object> Unconvertable { get; set; }

            public TestResponseObjectDynamic() : base(new Dictionary<string, object>())
            {
            }
        }

        public class TestResponseObject
        {
            public int IntValue { get; set; }
            public string StringValue { get; set; }
            public bool BoolValue { get; set; }
            public TestEnum EnumValue { get; set; }
            public List<object> Unconvertable { get; set; }
        }

        public enum TestEnum
        {
            One = 1,
            Two = 2,
            Three = 3
        }
    }
}
