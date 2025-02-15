#pragma once

#include <string>

namespace DB::QueryParsing::AST {

struct MetricSelector {
    std::string metricID;
};

}