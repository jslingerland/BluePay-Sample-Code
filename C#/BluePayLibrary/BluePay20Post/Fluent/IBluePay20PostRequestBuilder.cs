using System;

namespace BluePayLibrary.Interfaces.BluePay20Post.Fluent
{
    public interface IBluePay20PostRequestBuilder
    {
        IBluePay20PostRequestBuilder ForCustomer(string name1, 
            string name2=null,
            string addr1=null,
            string addr2=null,
            string city=null,
            string state=null,
            string zip=null,
            string country=null,
            string phone=null,
            string email=null);

        IBluePay20PostRequestBuilder FromSwipe(string swipedata);
        IBluePay20PostRequestBuilder FromCreditCard(string ccNumber, DateTime expiration, string cvv2);
        IBluePay20PostRequestBuilder FromCreditCard(string ccNumber, string expiration, string cvv2);

        IBluePay20PostRequestBuilder FromAch(string routingNumber, string accountNumber, AccountType accountType, DocType docType);

        IBluePay20PostRequestBuilder Sale(decimal amount, string transactionId = null);
        IBluePay20PostRequestBuilder Auth(decimal amount, string transactionId = null);
        IBluePay20PostRequestBuilder Refund(string transactionId, decimal? amount = null);
        IBluePay20PostRequestBuilder Capture(string transactionId, decimal? amount = null);

        IBluePay20PostRequestBuilder Void(string transactionId);

        IBluePay20PostRequestBuilder WithFields(Action<IBluePay20PostRequestBuilderFields> action);

        BluePayMessage ToMessage(string secretKey);
    }
}