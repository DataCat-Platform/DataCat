#pragma once

#include <cstdint>

namespace DB::QueryParsing {

struct TimeSpan {
    int64_t from;
    int64_t to;
};

}