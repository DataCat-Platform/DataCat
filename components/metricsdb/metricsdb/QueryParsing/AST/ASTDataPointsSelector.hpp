#pragma once

#include <memory>
#include <vector>

#include <metricsdb/QueryParsing/AST/ASTBase.hpp>
#include <metricsdb/QueryParsing/AST/ASTTagMatcher.hpp>
#include <metricsdb/QueryParsing/AST/TimeSpan.hpp>

namespace DB::QueryParsing {

class ASTDataPointsSelector : public ASTBase {
public:
    ASTDataPointsSelector(ASTPtrs& tagMatchers, TimeSpan timeSpan, int64_t pivot)
        : tagMatchers(tagMatchers)
        , timeSpan(timeSpan)
        , pivot(pivot) {};

    ASTPtrs tagMatchers;
    TimeSpan timeSpan;
    int64_t pivot;
};

}