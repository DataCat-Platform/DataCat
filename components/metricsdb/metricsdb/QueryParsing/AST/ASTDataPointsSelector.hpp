#pragma once

#include <metricsdb/QueryParsing/AST/ASTBase.hpp>
#include <metricsdb/QueryParsing/AST/ASTTagMatcher.hpp>
#include <metricsdb/QueryParsing/AST/TimeSpan.hpp>

namespace DataCat::DB::QueryParsing {

class ASTDataPointsSelector : public ASTBase {
public:
    ASTDataPointsSelector(ASTPtrs& tagMatchers, TimeSpan timeSpan, int64_t pivot)
        : tagMatchers(tagMatchers)
        , timeSpan(timeSpan)
        , pivot(pivot) {};

    ASTPtrs tagMatchers;
    TimeSpan timeSpan;
    int64_t pivot;

    void dump(std::ostream& ostr) override
    {
        ostr << "DataPointsSelector(";
        for (auto tagMatcher : tagMatchers) {
            tagMatcher->dump(ostr);
            ostr << ", ";
        }
        ostr << "[" << timeSpan.from << ":" << timeSpan.to << "], " << pivot << ")";
    }
};

}