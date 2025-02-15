#pragma once

#include <metricsdb/Core/Value.hpp>
#include <metricsdb/QueryParsing/AST/Base.hpp>

namespace DB::QueryParsing::AST {

class Literal : public Base {
public:
    const Value& getValue() const { return value; };

private:
    Value value;
};

}