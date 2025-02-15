#pragma once

#include <metricsdb/Core/MetricKind.hpp>
#include <string>

namespace DB {

struct MetricInfo {
    MetricKind kind;
    std::string unit;
};

}