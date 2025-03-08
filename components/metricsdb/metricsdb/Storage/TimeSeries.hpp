#pragma once

#include "metricsdb/Storage/DataPoints.hpp"
#include <iterator>
#include <metricsdb/Storage/MetricInfo.hpp>
#include <string>

namespace DB::Storage {

class TimeSeries : std::iterator<std::bidirectional_iterator_tag, int> {
public:
    MetricInfo getMetricInfo();
    std::string getMetricName();

    DataPoints getDataPoints();
};

}