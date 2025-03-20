#pragma once

#include <cstdint>

namespace DataCat::DB::Storage {

enum class MetricKind : int8_t {
    Counter,
    Gauge,
    Histogram,
};

}