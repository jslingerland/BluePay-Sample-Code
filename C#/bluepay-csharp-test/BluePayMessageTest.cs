using System;
using System.Collections.Generic;
using System.IO;
using BluePayLibrary.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BluePayLibrary.Test
{
    [TestClass]
    public class BluePayMessageTest
    {
        [TestMethod]
        public void DynamicGetWorks()
        {
            dynamic resp = new BluePayMessage(new Dictionary<string, object>
            {
                { "Field", "Value" }
            });

            Assert.AreEqual("Value", resp.Field);
        }

        [TestMethod]
        public void DynamicGetMissingFieldIsNull()
        {
            dynamic resp = new BluePayMessage(new Dictionary<string, object>());
            Assert.IsNull(resp.Field);
        }

        [TestMethod]
        public void ParseDynamic()
        {
            dynamic resp = BluePayMessage.Parse(new StringReader("TRANS_ID=1234&STATUS=E"));
            Assert.AreEqual("1234", resp.TRANS_ID);
            Assert.AreEqual("E", resp.STATUS);
        }
    }
}
