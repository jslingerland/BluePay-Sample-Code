using System;
using Transactions;
using Rebill;
using GetData;

namespace bluepayc
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// Uncomment line to run the corresponding sample code.
			
			// GET DATA
			// RetrieveSettlementData.Main();
			// RetrieveTransactionData.Main();
			// SingleTransactionQuery.Main();
			// TransactionNotification.Main();
            // ValidateBPStamp.Main();

			// REBILL
			// CancelRecurringPayment.Main();
			// CreateRecurringPaymentACH.Main();
			// CreateRecurringPaymentCC.Main();
			// GetRecurringPaymentStatus.Main();
			// UpdateRecurringPayment.Main();

			// TRANSACTIONS
			// CancelTransaction.Main();
			// ChargeCustomerACH.Main();
			// ChargeCustomerCC.Main();
            // ChargeCustomerCCLv2Lv3.Main();
			// CheckCustomerCredit.Main();
			// CreateCustomerToken.Main();
			// CustomerDefinedData.Main();
			// HowToUseToken.Main();
			// UpdateTransaction.Main();
			// ReturnFunds.Main();
			// StorePaymentInformation.Main();
			// Swipe.Main();
		}
	}
}


