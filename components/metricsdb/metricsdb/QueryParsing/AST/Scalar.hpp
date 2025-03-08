#pragma once

#include <memory>
#include <metricsdb/QueryParsing/AST/Base.hpp>

namespace DB::QueryParsing::AST {

class Scalar : public Base {
public:
    Scalar(double value)
        : value(value)
    {
    }

    double getValue() const { return value; };

    static ASTPtr create(double value)
    {
        return std::make_shared<Scalar>(value);
    }

private:
    double value;
};

}