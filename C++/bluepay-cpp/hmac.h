//
//  hmac.h
//  HMAC
//

#ifndef hmac_h
#define hmac_h

#include <stdio.h>
#include <vector>
#include "sha512.h"
#include "sha256.h"
#include "md5.h"

class Hmac {
private:
    std::string key = "";
    std::string message = "";
    std::string hashType = "";
    std::string (*hash)(std::string);
    std::string (*hmacHash)(std::string, std::string, std::string);
    std::string ipad = "";
    std::string opad = "";
    int blockSize = 128;

public:
    Hmac();
    Hmac(std::string, std::string, std::string);
    void parseHashType();
    void padKey();
    void xOrPads();
    std::string calcHmac();
    
};

#endif /* hmac_h */
