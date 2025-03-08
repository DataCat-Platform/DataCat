#pragma once

#include <grpcpp/grpcpp.h>
#include <grpcpp/server.h>
#include <metricsdb/Core/Database.hpp>
#include <string>

namespace DataCat::DB {

class Server {
public:
    Server(Database& db);

    void run(const std::string& host, const std::string& port);
    void wait();

private:
    Database& db;
    std::unique_ptr<grpc::Server> server;
};

}