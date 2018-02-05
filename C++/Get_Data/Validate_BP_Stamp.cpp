//
// BluePay C++ Sample code.
//
// This code sample reads the values from a BP10emu redirect
// and authenticates the message using the the BP_STAMP
// provided in the response. Point the REDIRECT_URL of your
// BP10emu request to the URI prefix specified below.
//

#include "Validate_BP_Stamp.h"
#include "../bluepay-cpp/BluePay.h"
#include <regex>
#include <map>
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sstream>
#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>

void error(const char *msg)
{
    perror(msg);
    exit(1);
}

void validateBPStamp(){
    // Set account credentials
    std::string accountId = "Merchant's Account ID Here";
    std::string secretKey = "Merchant's Secret Key Here";
    std::string mode = "TEST";
    
    int server_socket, client_socket;
    ssize_t n;
    int portno = 9000; // Merchant's port number here
    char buffer[2048];
    bzero(buffer,2048);
    
    // Create new socket
    server_socket = socket(AF_INET, SOCK_STREAM, 0);
    if (server_socket < 0)
        error("ERROR opening socket");
    struct sockaddr_in server_address, client_address;
    bzero((char *) &server_address, sizeof(server_address));
    
    // Define server address
    server_address.sin_family = AF_INET;
    server_address.sin_addr.s_addr = INADDR_ANY;
    server_address.sin_port = htons(portno);
    
    // Bind socket to server address
    if (bind(server_socket, (struct sockaddr *) &server_address,
             sizeof(server_address)) < 0)
        error("ERROR on binding");
    
    // Listen for incoming connections; max 5
    listen(server_socket,5);
    
    // Accept incoming connection
    while(1) {
        socklen_t client_length;
        client_length = sizeof(client_address);
        client_socket = accept(server_socket,
                               (struct sockaddr *) &client_address,
                               &client_length);
        if (client_socket < 0)
            error("ERROR on accept");
        
        // Receive request
        n = read(client_socket,buffer,2048);
        if (n < 0) error("ERROR reading from socket");
        
        // Parse query string
        std::istringstream iss(buffer);
        std::string method, query, protocol;
        if(!(iss >> method >> query >> protocol))
            error("ERROR on parsing request");
        iss.clear();
        iss.str(query);
        
        std::string url;
        if(!std::getline(iss, url, '?'))
            error("ERROR on parsing request");
        
        std::map<std::string, std::string> responsePairs;
        std::string keyval, key, val;
        
        while(std::getline(iss, keyval, '&'))
        {
            std::istringstream iss(keyval);
            if(std::getline(std::getline(iss, key, '='), val))
                responsePairs[key] = val;
        }
        
        // Build response body
        std::stringstream ss_body;
        
        // Validate BP_STAMP
        if (responsePairs.find("BP_STAMP") == responsePairs.end()) { // Check whether BP_STAMP is provided
            ss_body << "ERROR: BP_STAMP NOT FOUND. CHECK MESSAGE & RESPONSEVERSION";
        } else {
            BluePay bp(
                       accountId,
                       secretKey,
                       mode
                       );
            
            std::string bpStamp = responsePairs["BP_STAMP"];
            std::string bpStampDef = bp.urlDecode(responsePairs["BP_STAMP_DEF"]);
            std::string bpStampString = "";
            std::vector<std::string> bpStampFields = bp.split(bpStampDef, ' '); // Split BP_STAMP_DEF on whitespace
            for (std::string field : bpStampFields) {
                bpStampString += responsePairs[field]; // Concatenate values used to calculate expected BP_STAMP
            }
            bpStampString = bp.urlDecode(bpStampString);
            std::string calculatedStamp = bp.generateTps(bpStampString, responsePairs["TPS_HASH_TYPE"]); // Calculate expected BP_STAMP using hash function specified in response
            std::transform(calculatedStamp.begin(), calculatedStamp.end(),calculatedStamp.begin(), ::toupper);
            
            if(calculatedStamp == bpStamp){ // Compare expected BP_STAMP with received BP_STAMP
                // Validate BP_STAMP and reads the response results
                ss_body << "VALID BP_STAMP: TRUE<br/>";
                for(auto const& pair: responsePairs){
                    ss_body << pair.first << ": " << pair.second << "<br/>";
                }
            } else
            {
                ss_body << "ERROR: BP_STAMP VALUES DO NOT MATCH\n";
            }
        }
        std::string body = ss_body.str();
        
        // Build response headers
        std::stringstream ss_header;
        ss_header <<"HTTP/1.1 200 OK\r\n"
        << "Content-Type: text/html\n"
        << "Content-Length: " << body.length() << '\n'
        << "Connection: close\n"
        << '\n' << body;
        const char* http_headers = ss_header.str().c_str();
        
        // Write response
        n = write(client_socket,http_headers,strlen(http_headers));
        if (n < 0) error("ERROR writing to socket");
        close(client_socket);
        bzero(buffer,2048);
    }
}