#include <metricsdb/Config/Version.hpp>
#include <metricsdb/Core/Database.hpp>
#include <metricsdb/Logging/Macros.hpp>
#include <metricsdb/Runner/DatabaseRunner.hpp>
#include <metricsdb/Server/Server.hpp>

namespace DataCat::DB {

void DatabaseRunner::run(int argc, char** argv)
{
    DATACAT_LOG_INFO << "DataCat Metrics Database [v." << VERSION_MAJOR << '.'
                     << VERSION_MINOR << '.' << VERSION_PATCH
                     << "]\nCopyright (C) 2025 DataCat\n"
                     << std::endl;

    Database db;
    db.prepare();

    // Start writers server.
    std::string host = "0.0.0.0";
    std::string port = "30000";
    Server server(db);
    server.run(host, port);
    server.wait();
}

}