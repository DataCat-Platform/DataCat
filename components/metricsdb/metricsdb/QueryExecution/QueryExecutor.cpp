#include <metricsdb/QueryExecution/QueryExecutor.hpp>

namespace DB::QueryExecution {

void QueryExecutor::execute()
{
    outputStream << TimeSeries {};
    outputStream << Tags {};
}

}