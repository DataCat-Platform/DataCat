#pragma once

#include <string>

#include <metricsdb/QueryParsing/AST/ASTExpression.hpp>

namespace DataCat::DB::QueryParsing {

class ASTLiteral : public ASTExpression { };

class ASTIntegerLiteral : public ASTLiteral {
public:
    ASTIntegerLiteral(int64_t value)
        : value(value)
    {
    }

    int64_t value;

    void dump(std::ostream& ostr) override { ostr << "DurationLiteral(" << value << ")"; }
};

class ASTFloatLiteral : public ASTLiteral {
public:
    ASTFloatLiteral(double value)
        : value(value)
    {
    }

    double value;

    void dump(std::ostream& ostr) override { ostr << "FloatLiteral(" << value << ")"; }
};

class ASTStringLiteral : public ASTLiteral {
public:
    ASTStringLiteral(std::string value)
        : value(value)
    {
    }

    std::string value;

    void dump(std::ostream& ostr) override { ostr << "StringLiteral(" << value << ")"; }
};

class ASTDurationLiteral : public ASTLiteral {
public:
    ASTDurationLiteral(int64_t milliseconds)
        : milliseconds(milliseconds)
    {
    }

    int64_t milliseconds;

    void dump(std::ostream& ostr) override { ostr << "DurationLiteral(" << milliseconds << ")"; }
};

}