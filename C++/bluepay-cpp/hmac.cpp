//
//  hmac.cpp
//  HMAC
//

#include <string>
#include <iostream>
#include "hmac.h"

Hmac::Hmac(std::string key, std::string message, std::string hashType){
    this->key = key;
    this->message = message;
    this->hashType = hashType;
}

void Hmac::parseHashType(){
    if (hashType == "SHA256") {
        hash = &sha256;
        hmacHash = &sha256Hmac;
        blockSize = 64;
    } else if (hashType == "SHA512") {
        hash = &sha512;
        hmacHash = &sha512Hmac;
    }
}

void Hmac::padKey(){
    if (key.length() > blockSize) {
        key = hash(key);
    } else if (key.length() < blockSize){
        long int i = blockSize - key.length();
        key = key.append(std::string(i, 0x00));
    }
}

void Hmac::xOrPads() {
    for (int i = 0; i < blockSize; i++)
    {
        ipad.push_back(key[i] ^ 0x36);
        opad.push_back(key[i] ^ 0x5c);
    }
}

std::string Hmac::calcHmac(){
    parseHashType();
    padKey();
    xOrPads();
    
    std::string result = hmacHash(message, ipad, opad);
    return result;
}
