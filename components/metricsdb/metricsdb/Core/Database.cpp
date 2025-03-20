#include <metricsdb/Config/Version.hpp>
#include <metricsdb/Core/Database.hpp>
#include <metricsdb/Logging/Macros.hpp>
#include <metricsdb/Storage/MetricsSample.hpp>

namespace DataCat::DB {

void Database::prepare()
{
    DATACAT_LOG_INFO << "Replaying WAL" << std::endl;
    // TODO.
}

void Database::writeMetrics(Storage::MetricsSample sample)
{
    // TODO.
}

}