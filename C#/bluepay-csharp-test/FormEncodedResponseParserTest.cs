using System.IO;
using System.Linq;
using BluePayLibrary.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

namespace BluePayLibrary.Test
{
    [TestClass]
    public class FormEncodedResponseParserTest
    {
        [TestMethod]
        public void ParserDisposesReader()
        {
            var reader = new Mock<TextReader>();
            
            using (var parser = new FormEncodedResponseParser(reader.Object))
            {
                //do nothing
            }

            reader.Protected().Verify("Dispose", Times.AtLeastOnce(), true);
        }

        [TestMethod]
        public void HexEncodedValueIsParsed()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("My%20Key=My%20Value")))
            {
                var val = parser.Read();
                Assert.AreEqual("My Key", val.Item1);
                Assert.AreEqual("My Value", val.Item2);
            }
        }

        [TestMethod]
        public void ReadEmptyIsNull()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("")))
            {
                var val = parser.Read();
                Assert.IsNull(val);
            }
        }

        [TestMethod]
        public void ReadAllGetsAllValues()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("MyKey=MyValue&MyKey2=MyValue2")))
            {
                var val = parser.ReadAll().ToArray();
                Assert.AreEqual(2, val.Length);
            }
        }

        [TestMethod]
        public void ReadGetsMultipleValues()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("MyKey=MyValue&MyKey2=MyValue2")))
            {
                var first = parser.Read();
                Assert.AreEqual("MyKey", first.Item1);
                Assert.AreEqual("MyValue", first.Item2);

                var second = parser.Read();
                Assert.AreEqual("MyKey2", second.Item1);
                Assert.AreEqual("MyValue2", second.Item2);
            }
        }

        [TestMethod]
        public void ReadPastEndIsNull()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("MyKey=MyValue")))
            {
                var first = parser.Read();
                Assert.IsNotNull(first);

                var second = parser.Read();
                Assert.IsNull(second);
            }
        }

        [TestMethod]
        public void ReadMultiplePastEndIsNull()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("MyKey=MyValue")))
            {
                var first = parser.Read();
                Assert.IsNotNull(first);

                var second = parser.Read();
                Assert.IsNull(second);

                var third = parser.Read();
                Assert.IsNull(third);
            }
        }

        [TestMethod]
        public void ReadEmptyValuesAsEmptyString()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("KeyIsNull=&KeyHasValue=Value")))
            {
                var first = parser.Read();
                Assert.AreEqual("KeyIsNull", first.Item1);
                Assert.AreEqual(string.Empty, first.Item2);

                var second = parser.Read();
                Assert.AreEqual("KeyHasValue", second.Item1);
                Assert.AreEqual("Value", second.Item2);
            }
        }
        
        [TestMethod]
        public void HexEncodedValueIsParsedAsync()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("My%20Key=My%20Value")))
            {
                var val = parser.ReadAsync().Result;
                Assert.AreEqual("My Key", val.Item1);
                Assert.AreEqual("My Value", val.Item2);
            }
        }

        [TestMethod]
        public void ReadEmptyIsNullAsync()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("")))
            {
                var val = parser.ReadAsync().Result;
                Assert.IsNull(val);
            }
        }
        
        [TestMethod]
        public void ReadGetsMultipleValuesAsync()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("MyKey=MyValue&MyKey2=MyValue2")))
            {
                var first = parser.ReadAsync().Result;
                Assert.AreEqual("MyKey", first.Item1);
                Assert.AreEqual("MyValue", first.Item2);

                var second = parser.ReadAsync().Result;
                Assert.AreEqual("MyKey2", second.Item1);
                Assert.AreEqual("MyValue2", second.Item2);
            }
        }

        [TestMethod]
        public void ReadPastEndIsNullAsync()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("MyKey=MyValue")))
            {
                var first = parser.ReadAsync().Result;
                Assert.IsNotNull(first);

                var second = parser.ReadAsync().Result;
                Assert.IsNull(second);
            }
        }

        [TestMethod]
        public void ReadMultiplePastEndIsNullAsync()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("MyKey=MyValue")))
            {
                var first = parser.ReadAsync().Result;
                Assert.IsNotNull(first);

                var second = parser.ReadAsync().Result;
                Assert.IsNull(second);

                var third = parser.ReadAsync().Result;
                Assert.IsNull(third);
            }
        }

        [TestMethod]
        public void ReadEmptyValuesAsEmptyStringAsync()
        {
            using (var parser = new FormEncodedResponseParser(new StringReader("KeyIsNull=&KeyHasValue=Value")))
            {
                var first = parser.ReadAsync().Result;
                Assert.AreEqual("KeyIsNull", first.Item1);
                Assert.AreEqual(string.Empty, first.Item2);

                var second = parser.ReadAsync().Result;
                Assert.AreEqual("KeyHasValue", second.Item1);
                Assert.AreEqual("Value", second.Item2);
            }
        }




    }
}
