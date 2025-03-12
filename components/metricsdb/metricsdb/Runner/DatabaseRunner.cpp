#include <csignal>

#include <Poco/Poco.h>

#include <metricsdb/Config/Version.hpp>
#include <metricsdb/Core/Database.hpp>
#include <metricsdb/Logging/Macros.hpp>
#include <metricsdb/Runner/DatabaseRunner.hpp>
#include <metricsdb/Server/ReadersServer.hpp>
#include <metricsdb/Server/WritersServer.hpp>

namespace DataCat::DB {

void DatabaseRunner::run(int argc, char** argv)
{
    DATACAT_LOG_INFO << "DataCat Metrics Database [v." << VERSION_MAJOR << '.'
                     << VERSION_MINOR << '.' << VERSION_PATCH
                     << "]\nCopyright (C) 2025 DataCat\n"
                     << std::endl;

    Database db;
    db.prepare();

    WritersServer writersServer(db);
    writersServer.run("0.0.0.0", "30000");

    ReadersServer readersServer;
    readersServer.run("0.0.0.0", 30001);

    sigset_t sset;
    sigemptyset(&sset);
    sigaddset(&sset, SIGTERM);
    int sig;
    sigwait(&sset, &sig);
}

}