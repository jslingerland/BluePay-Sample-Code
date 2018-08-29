//
// BluePay C++ Sample code.
//
// This code sample runs a $0.00 AUTH transaction
// and creates a customer token using test payment information,
// which is then used to run a separate $3.99 sale.


#include "Create_Customer_Token.h"
#include "../bluepay-cpp/BluePay.h"
#include <iostream>
using namespace std;

void createCustomerToken(){
    string accountId = "Merchant's Account ID Here";
    string secretKey = "Merchant's Secret Key Here";
    string mode = "TEST";
    
    BluePay token(
        accountId,
        secretKey,
        mode
    );
    
    token.setCustomerInformation(
        "Bob", // First Name
        "Tester", // Last Name
        "123 Test St.", // Address1
        "Apt #500", // Address2
        "Testville", // City
        "IL", // State
        "54321", // Zip
        "USA", // Country
        "1231231234", // Phone Number
        "test@bluepay.com" // Email Address
        );
    
    token.setCCInformation(
        "4111111111111111", // Card Number
        "1225", // Card Expire
        "123" // Card CVV2
   );
    
    std::map<std::string, std::string> authParams = {
        { "amount", "0.00" },
        { "newCustToken", "true" } // "true" generates random string. Other values will be used literally
    };
    
    // Auth Amount: $0.00
    token.auth(authParams);
    
    // Makes the API request with BluePay
    token.process();
    
    // Try again if we accidentally create a non-unique token
    std::string tokenResult = token.getMessage();
    if (tokenResult.find("Customer Tokens must be unique") != std::string::npos) {
        std::map<std::string, std::string> newAuthParams = {
            { "amount", "0.00" },
            { "newCustToken", "true" }
        };
        token.auth(newAuthParams);
        token.process();
    }

    // Reads the responses from BluePay if transaction was approved
    if (token.isSuccessfulTransaction()){
        BluePay payment(
            accountId,
            secretKey,
            mode
        );
        
        std::map<std::string, std::string> saleParams = {
            { "amount", "3.99" },
            { "custToken", token.getCustToken() }
        };
        
        payment.sale(saleParams);
        
        payment.process();
        
        if (payment.isSuccessfulTransaction()){
            cout << string("Transaction Status: ") + payment.getResult() + "\n";
            cout << string("Transaction Message: ") + payment.getMessage() + "\n";
            cout << string("Transaction ID: ") + payment.getTransId() + "\n";
            cout << string("AVS Result: ") + payment.getAvs() + "\n";
            cout << string("CVV2 Result: ") + payment.getCvv2() + "\n";
            cout << string("Masked Payment Account: ") + payment.getMaskedPaymentAccount() + "\n";
            cout << string("Card Type: ") + payment.getCardType() + "\n";
            cout << string("Authorization Code: ") + payment.getAuthCode() + "\n";
        }
        else {
            cout << string("Error: ") + payment.getMessage();
        }
    }
    else {
        cout << string("Error: ") + token.getMessage();
    }
}
