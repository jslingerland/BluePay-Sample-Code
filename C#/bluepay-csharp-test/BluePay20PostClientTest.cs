using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BluePayLibrary.Interfaces.BluePay20Post;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;

namespace BluePayLibrary.Interfaces.Test
{
    [TestClass]
    public class BluePay20PostClientTest
    {
        [TestMethod]
        public void ProcessSendsUrlEncodedMessage()
        {
            var requestMock = new MockWebRequest();
            requestMock.SetupResponse("");
            
            var proxy = new BluePay20PostClient(
                endpoint: "https://example.org",
                parser: new Mock<IBluePayResponseParser<BluePayPost20ResponseV3>>().Object,
                webRequestProvider: (url) => { Assert.AreEqual("https://example.org", url);
                                               return requestMock.Object; }
                );
            
            proxy.Process(new BluePayMessage(new Dictionary<string, object>
            {
                {"My Field", "My Value!#"}
            }));

            Assert.AreEqual("My+Field=My+Value!%23", requestMock.RequestText);
        }

        [TestMethod]
        public void ProcessSendsUrlEncodedMessageAsync()
        {
            var requestMock = new MockWebRequest();
            requestMock.SetupResponse("");

            var proxy = new BluePay20PostClient(
                endpoint: "https://example.org",
                parser: new Mock<IBluePayResponseParser<BluePayPost20ResponseV3>>().Object,
                webRequestProvider: (url) => {
                    Assert.AreEqual("https://example.org", url);
                    return requestMock.Object;
                });

            var result = proxy.ProcessAsync(new BluePayMessage(new Dictionary<string, object>
            {
                {"My Field", "My Value!#"}
            })).Result;

            Assert.AreEqual("My+Field=My+Value!%23", requestMock.RequestText);
        }

        [TestMethod]
        public void ProcessParsesResult()
        {
            var requestMock = new MockWebRequest();
            requestMock.SetupResponse("responsetext");

            var expected = new BluePayPost20ResponseV3();

            var parserMock = new Mock<IBluePayResponseParser<BluePayPost20ResponseV3>>();
            parserMock.Setup(p => p.Parse(It.Is((TextReader tr) => tr.ReadToEnd() == "responsetext")))
                      .Returns(expected);

            var proxy = new BluePay20PostClient(
                endpoint: "https://example.org",
                parser: parserMock.Object,
                webRequestProvider: (url) => {
                    Assert.AreEqual("https://example.org", url);
                    return requestMock.Object;
                });

            var response = proxy.Process(new BluePayMessage());

            Assert.AreSame(expected, response);
        }

        [TestMethod]
        public void ProcessParsesResultAsync()
        {
            var requestMock = new MockWebRequest();
            requestMock.SetupResponse("responsetext");

            var expected = new BluePayPost20ResponseV3();

            var parserMock = new Mock<IBluePayResponseParser<BluePayPost20ResponseV3>>();
            parserMock.Setup(p => p.ParseAsync(It.Is((TextReader tr) => tr.ReadToEnd() == "responsetext")))
                      .ReturnsAsync(expected);

            var proxy = new BluePay20PostClient(
                endpoint: "https://example.org",
                parser: parserMock.Object,
                webRequestProvider: (url) => {
                    Assert.AreEqual("https://example.org", url);
                    return requestMock.Object;
                });

            var response = proxy.ProcessAsync(new BluePayMessage()).Result;

            Assert.AreSame(expected, response);
        }

        [TestMethod]
        public void ProcessWrapsException()
        {
            var responseMock = new Mock<WebResponse>();
            responseMock.Setup(r => r.GetResponseStream())
                        .Returns(new MemoryStream(Encoding.UTF8.GetBytes("response error")));

            var expected = new BluePayPost20ResponseV3();
            var parserMock = new Mock<IBluePayResponseParser<BluePayPost20ResponseV3>>();
            parserMock.Setup(p => p.Parse(It.Is((TextReader tr) => tr.ReadToEnd() == "response error")))
                      .Returns(expected);

            var inner = new WebException("inner", null, WebExceptionStatus.UnknownError, responseMock.Object);
            
            var proxy = new BluePay20PostClient(
                endpoint: "https://example.org",
                parser: parserMock.Object,
                webRequestProvider: (url) => { throw inner; });

            var response = proxy.Process(new BluePayMessage());
            Assert.AreSame(expected, response);
        }

        [TestMethod]
        public void ProcessWrapsExceptionAsync()
        {
            var responseMock = new Mock<WebResponse>();
            responseMock.Setup(r => r.GetResponseStream())
                        .Returns(new MemoryStream(Encoding.UTF8.GetBytes("response error")));

            var expected = new BluePayPost20ResponseV3();
            var parserMock = new Mock<IBluePayResponseParser<BluePayPost20ResponseV3>>();
            parserMock.Setup(p => p.ParseAsync(It.Is((TextReader tr) => tr.ReadToEnd() == "response error")))
                      .ReturnsAsync(expected);

            var inner = new WebException("inner", null, WebExceptionStatus.UnknownError, responseMock.Object);

            var proxy = new BluePay20PostClient(
                endpoint: "https://example.org",
                parser: parserMock.Object,
                webRequestProvider: (url) => { throw inner; });

            var response = proxy.ProcessAsync(new BluePayMessage()).Result;
            Assert.AreSame(expected, response);
        }

        private class MockWebRequest
        {
            private readonly Mock<WebRequest> _mock = new Mock<WebRequest>();
            private readonly MemoryStream _requestStream = new MemoryStream();

            public MockWebRequest()
            {
                _mock.Setup(m => m.GetRequestStream()).Returns(_requestStream);
                _mock.Setup(m => m.GetRequestStreamAsync()).Returns(() => Task.FromResult((Stream)_requestStream));
            }

            public ISetup<WebRequest> Setup(Expression<Action<WebRequest>> expr)
            {
                return _mock.Setup(expr);
            }

            public ISetup<WebRequest, TResult> Setup<TResult>(Expression<Func<WebRequest, TResult>> expr)
            {
                return _mock.Setup(expr);
            }

            public void SetupResponse(string text)
            {
                var resp = new Mock<WebResponse>();
                resp.Setup(r => r.GetResponseStream()).Returns(new MemoryStream(Encoding.UTF8.GetBytes(text)));

                _mock.Setup(r => r.GetResponse())
                     .Returns(resp.Object);

                _mock.Setup(r => r.GetResponseAsync())
                    .ReturnsAsync(resp.Object);
            }

            public ISetup<WebRequest, Stream> SetupResponse(Expression<Func<WebRequest, Stream>> expr)
            {
                return _mock.Setup(expr);
            }

            public WebRequest Object { get { return _mock.Object; } }

            public string RequestText
            {
                get
                {
                    if (_requestStream == null)
                        return null;

                    return Encoding.UTF8.GetString(_requestStream.ToArray());
                }
            }
        }


    }
}
