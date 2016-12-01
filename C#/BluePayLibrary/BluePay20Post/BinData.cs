using System;
using System.ComponentModel;
using System.Globalization;
using BluePayLibrary.Interfaces.BluePay20Post.BindataTypes;

namespace BluePayLibrary.Interfaces.BluePay20Post
{
    /// <summary>
    /// Wraps the bindata data from the credit card processing network.
    /// </summary>
    [TypeConverter(typeof(BinDataStringConverter))]
    public class BinData
    {
        /// <summary>
        /// Length of the Bank Identification Number (BIN). A value between 1 and 16
        /// </summary>
        public byte BinLength { get; set; }

        public CardType CardType { get; set; }

        public CardUsage CardUsage { get; set; }

        public Networks Networks { get; set; }

        /// <summary>
        /// Electronic Benefit Transfer State
        /// </summary>
        public string Ebt { get; set; }

        /// <summary>
        /// Flexible Spending Account
        /// </summary>

        public string Fsa { get; set; }

        public Prepaid Prepaid { get; set; }

        public CardProductSubCategory ProdId { get; set; }

        public Regulated Regulated { get; set; }

        public Subtype Subtype { get; set; }

        public LargeTicket LargeTicket { get; set; }

        public AccountLevelProcessing AccountLevelProcessing { get; set; }

        public AccountFundSource AccountFundSource { get; set; }
    }

    public class BinDataStringConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if(s == null)
                return base.ConvertFrom(context, culture, value);

            var ret = new BinData();
            
            /*
             * Tilde(~) seperated list of of transaction information returned by credit card processing network.
             * Example: BINDATA=6~V~X~~~~~~~~A~N~~~Y~C
             * 0    binlen
             * 1    cardtype
             * 2    cardusage
             * 3    networks
             * 4    ebt
             * 5    fsa
             * 6    issbin (unused)
             * 7    processbin (unused)
             * 8    ica (unused)
             * 9    prepaid
             * 10   prodid 
             * 11   regulated
             * 12   subtype
             * 13   largeticket
             * 14   accountlevelprocessing
             * 15   accountfundsource
            */

            var arr = s.Split(new char[] {'~'}, StringSplitOptions.None);
            if(arr.Length < 16)
                Array.Resize(ref arr, 16);

            byte binLen;
            byte.TryParse(arr[0] ?? "", out binLen);
            ret.BinLength = binLen;

            ret.CardType = new CardType(arr[1]);
            ret.CardUsage = new CardUsage(arr[2]);
            ret.Networks = new Networks(arr[3]);
            ret.Ebt = arr[4];
            ret.Fsa = arr[5];
            //skip issbin
            //skip processbin
            //skip ica
            ret.Prepaid = new Prepaid(arr[9]);
            ret.ProdId = new CardProductSubCategory(arr[10]);
            ret.Regulated = new Regulated(arr[11]);
            ret.Subtype = new Subtype(arr[12]);
            ret.LargeTicket = new LargeTicket(arr[13]);
            ret.AccountLevelProcessing = new AccountLevelProcessing(arr[14]);
            ret.AccountFundSource = new AccountFundSource(arr[15]);

            return ret;
        }
    }


    

}
