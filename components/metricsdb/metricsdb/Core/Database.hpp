#pragma once

#include <metricsdb/Storage/MetricsSample.hpp>

namespace DataCat::DB {

class Database {
public:
    void prepare();
    void writeMetrics(Storage::MetricsSample sample);
};

}