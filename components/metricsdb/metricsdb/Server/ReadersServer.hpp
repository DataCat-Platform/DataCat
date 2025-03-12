#pragma once

#include <memory>
#include <string>

#include <Poco/Net/HTTPServer.h>
#include <memory.h>

namespace DataCat::DB {

class ReadersServer {
public:
    void run(const std::string& host, int port);

private:
    std::unique_ptr<Poco::Net::HTTPServer> httpServerPtr;
    Poco::Net::Socket socket;
};

}