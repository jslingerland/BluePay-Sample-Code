using System;
using BluePayLibrary.Interfaces.BluePay20Post.BindataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BluePayLibrary.Test
{
    [TestClass]
    public class StrinumTest
    {
        [TestMethod]
        public void EquitableEqualsTrue()
        {
            IEquatable<CardType> ct = new CardType("V");
            Assert.IsTrue(ct.Equals(new CardType("V")));
        }

        [TestMethod]
        public void EquitableEqualsFalse()
        {
            IEquatable<CardType> ct = new CardType("V");
            Assert.IsFalse(ct.Equals(new CardType("M")));
        }

        [TestMethod]
        public void ObjectEqualsTrue()
        {
            object ct = new CardType("V");
            Assert.IsTrue(ct.Equals(new CardType("V")));
        }

        [TestMethod]
        public void ObjectEqualsFalse()
        {
            object ct = new CardType("V");
            Assert.IsFalse(ct.Equals(new CardType("M")));
        }

        [TestMethod]
        public void ObjectEqualsNullFalse()
        {
            object ct = new CardType("V");
            Assert.IsFalse(ct.Equals(null));
        }

        [TestMethod]
        public void ObjectEqualsWrongTypeFalse()
        {
            object ct = new CardType("V");
            Assert.IsFalse(ct.Equals(1));
        }

        [TestMethod]
        public void EqualsOperatorTrue()
        {
            var ct = new CardType("V");
            Assert.IsTrue(ct == new CardType("V"));
        }

        [TestMethod]
        public void EqualsOperatorFalse()
        {
            var ct = new CardType("V");
            Assert.IsFalse(ct == new CardType("M"));
        }

        [TestMethod]
        public void NotEqualsOperatorTrue()
        {
            var ct = new CardType("V");
            Assert.IsTrue(ct != new CardType("M"));
        }

        [TestMethod]
        public void NotEqualsOperatorFalse()
        {
            var ct = new CardType("V");
            Assert.IsFalse(ct != new CardType("V"));
        }

        [TestMethod]
        public void GetHashCodeIsSameForEqualObjects()
        {
            var a = new CardType("V");
            var b = new CardType("V");
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [TestMethod]
        public void ComparableSortsAbove()
        {
            var a = new CardType("V");
            var b = new CardType("M");

            Assert.AreEqual(1, a.CompareTo(b));
        }

        [TestMethod]
        public void ComparableSortsBelow()
        {
            var a = new CardType("V");
            var b = new CardType("M");

            Assert.AreEqual(-1, b.CompareTo(a));
        }

        [TestMethod]
        public void ComparableSortsEqual()
        {
            var a = new CardType("V");
            var b = new CardType("V");

            Assert.AreEqual(0, a.CompareTo(b));
        }
    }
}
