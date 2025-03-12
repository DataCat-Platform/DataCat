#pragma once

#include <metricsdb/Core/Database.hpp>
#include <string>

namespace DataCat::DB {

class WritersServer {
public:
    WritersServer(Database& db);

    void run(const std::string& host, const std::string& port);

private:
    Database& db;
};

}