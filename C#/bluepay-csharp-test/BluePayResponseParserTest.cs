using System;
using System.IO;
using BluePayLibrary.Interfaces.BluePay20Post;
using BluePayLibrary.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BluePayLibrary.Interfaces.Test
{
    [TestClass]
    public class BluePayResponseParserTest
    {
        [TestMethod]
        public void Parse()
        {
            var converter = new Mock<IBluePayResponseObjectConverter<TestResponseObject>>();
            converter.Setup(c => c.Create()).Returns(new TestResponseObject());
            converter.Setup(c => c.SetValue(It.IsAny<TestResponseObject>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<TestResponseObject, string, string>((o, key, value) =>
                {
                    if (key == "TRANS_ID")
                        o.TransId = value;
                    else if (key == "STATUS")
                        o.Status = value[0];
                    else
                        throw new ArgumentException();
                });

            var parser = new BluePayResponseParser<TestResponseObject>(converter.Object);

            var resp = parser.Parse(new StringReader("TRANS_ID=1234&STATUS=E"));
            Assert.AreEqual("1234", resp.TransId);
            Assert.AreEqual('E', resp.Status);
        }

        [TestMethod]
        public void ParseAsync()
        {
            var converter = new Mock<IBluePayResponseObjectConverter<TestResponseObject>>();
            converter.Setup(c => c.Create()).Returns(new TestResponseObject());
            converter.Setup(c => c.SetValue(It.IsAny<TestResponseObject>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<TestResponseObject, string, string>((o, key, value) =>
                {
                    if (key == "TRANS_ID")
                        o.TransId = value;
                    else if (key == "STATUS")
                        o.Status = value[0];
                    else
                        throw new ArgumentException();
                });

            var parser = new BluePayResponseParser<TestResponseObject>(converter.Object);

            var resp = parser.ParseAsync(new StringReader("TRANS_ID=1234&STATUS=E")).Result;
            Assert.AreEqual("1234", resp.TransId);
            Assert.AreEqual('E', resp.Status);
        }

        public class TestResponseObject
        {
            public string TransId { get; set; }
            public char Status { get; set; }
        }
    }
}
