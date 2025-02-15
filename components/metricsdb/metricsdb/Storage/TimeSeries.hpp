#pragma once

#include <iterator>
#include <metricsdb/Storage/MetricInfo.hpp>
#include <string>

namespace DB {

class TimeSeries : std::iterator<std::bidirectional_iterator_tag, int> {
public:
    MetricInfo getMetricInfo();
    std::string getMetricName();
};

}