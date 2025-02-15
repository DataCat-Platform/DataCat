#pragma once

#include <cstdint>

namespace DB {

enum class MetricKind : int8_t {
    Counter,
    Gauge,
    Histogram,
};

}