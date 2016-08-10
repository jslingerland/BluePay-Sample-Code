using System;
using BluePayLibrary.Interfaces.BluePay20Post;
using BluePayLibrary.Interfaces.BluePay20Post.BindataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BluePayLibrary.Test
{
    [TestClass]
    public class BinDataStringConverterTest
    {
        [TestMethod]
        public void CanConvertFromIsTrueForString()
        {
            var converter = new BinDataStringConverter();
            Assert.IsTrue(converter.CanConvertFrom(typeof(string)));
        }

        [TestMethod]
        public void ConvertFromStringWorks()
        {
            var val = string.Join("~",
                6, //binlen
                CardType.Discover, //cardtype
                CardUsage.DebitHybrid, //cardusage
                Networks.Base24Interac, //networks
                "EBTVALUE", //ebt
                "FSAVALUE", //fsa
                "",//issbin(unused)
                "",//processbin(unused)
                "",//ica(unused)
                Prepaid.PrepaidCard,//prepaid
                CardProductSubCategory.MastercardBusinessCard,//prodid
                Regulated.IssRegulatedIssuer,//regulated
                Subtype.Construction, //subtype
                LargeTicket.VisaLargeTicket, //largeticket
                AccountLevelProcessing.Yes, //accountlevelprocessing
                AccountFundSource.Charge//accountfundsource
            );

            var converter = new BinDataStringConverter();
            var obj = (BinData)converter.ConvertFromString(val);
            Assert.AreEqual(6, obj.BinLength);
            Assert.AreEqual(CardType.Discover, obj.CardType);
            Assert.AreEqual(CardUsage.DebitHybrid, obj.CardUsage);
            Assert.AreEqual(Networks.Base24Interac, obj.Networks);
            Assert.AreEqual("EBTVALUE", obj.Ebt);
            Assert.AreEqual("FSAVALUE", obj.Fsa);
            Assert.AreEqual(Prepaid.PrepaidCard, obj.Prepaid);
            Assert.AreEqual(CardProductSubCategory.MastercardBusinessCard, obj.ProdId);
            Assert.AreEqual(Regulated.IssRegulatedIssuer, obj.Regulated);
            Assert.AreEqual(Subtype.Construction, obj.Subtype);
            Assert.AreEqual(LargeTicket.VisaLargeTicket, obj.LargeTicket);
            Assert.AreEqual(AccountLevelProcessing.Yes, obj.AccountLevelProcessing);
            Assert.AreEqual(AccountFundSource.Charge, obj.AccountFundSource);
        }

        [TestMethod]
        public void ConvertFromPartialStringWorks()
        {
            var val = string.Join("~",
                6, //binlen
                CardType.Discover, //cardtype
                CardUsage.DebitHybrid, //cardusage
                Networks.Base24Interac, //networks
                "EBTVALUE", //ebt
                "FSAVALUE", //fsa
                "",//issbin(unused)
                "",//processbin(unused)
                ""//ica(unused)
                //prepaid
                //prodid
                //regulated
                //subtype
                //largeticket
                //accountlevelprocessing
                //accountfundsource
            );

            var converter = new BinDataStringConverter();
            var obj = (BinData)converter.ConvertFromString(val);
            Assert.IsTrue(obj.BinLength != 0);
            Assert.IsTrue(obj.CardType.IsSet);
            Assert.IsTrue(obj.CardUsage.IsSet);
            Assert.IsTrue(obj.Networks.IsSet);
            Assert.IsNotNull(obj.Ebt);
            Assert.IsNotNull(obj.Fsa);
            Assert.IsFalse(obj.Prepaid.IsSet);
            Assert.IsFalse(obj.ProdId.IsSet);
            Assert.IsFalse(obj.Regulated.IsSet);
            Assert.IsFalse(obj.Subtype.IsSet);
            Assert.IsFalse(obj.LargeTicket.IsSet);
            Assert.IsFalse(obj.AccountLevelProcessing.IsSet);
            Assert.IsFalse(obj.AccountFundSource.IsSet);
        }

    }
}
