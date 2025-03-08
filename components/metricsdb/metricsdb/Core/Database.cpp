#include <metricsdb/Config/Version.hpp>
#include <metricsdb/Core/Database.hpp>
#include <metricsdb/Logging/Macros.hpp>
#include <metricsdb/Server/Server.hpp>

namespace DataCat::DB {

void Database::prepare()
{
    DATACAT_LOG_INFO << "Replaying WAL" << std::endl;
    // TODO.
}

}