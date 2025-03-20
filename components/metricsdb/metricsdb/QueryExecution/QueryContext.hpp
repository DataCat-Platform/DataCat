#pragma once

#include <ostream>

namespace DB::QueryExecution {

struct QueryContext {
    std::ostream& outputStream;
};

}