#pragma once

#include <cstdint>

namespace DB::QueryParsing {

enum class FunctionKind : int8_t {
    Sum,
    Max,
    Min,
    Clamp,
    Abs,
    Ceil,
    Floor,
    Exp,
    Sort,
};

}