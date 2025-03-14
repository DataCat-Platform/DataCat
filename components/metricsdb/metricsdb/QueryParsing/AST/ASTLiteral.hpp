#pragma once

#include <string>

#include <metricsdb/QueryParsing/AST/ASTBase.hpp>

namespace DB::QueryParsing {

class ASTLiteral : public ASTBase { };

class ScalarLiteral : public ASTLiteral {
public:
    ScalarLiteral(double scalar)
        : scalar(scalar)
    {
    }

    double scalar;
};

class StringLiteral : public ASTBase {
public:
    StringLiteral(std::string string)
        : string(string)
    {
    }

    std::string string;
};

class DurationLiteral : public ASTBase {
public:
    DurationLiteral(int64_t milliseconds)
        : milliseconds(milliseconds)
    {
    }

    int64_t milliseconds;
};

}