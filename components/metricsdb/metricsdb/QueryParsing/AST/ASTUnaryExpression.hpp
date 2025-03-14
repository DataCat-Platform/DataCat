#pragma once

#include <unordered_map>

#include <metricsdb/QueryParsing/AST/ASTExpression.hpp>

namespace DB::QueryParsing {

class ASTUnaryExpression : public ASTExpression {
public:
    enum class Kind {
        NEGATION,
    };

    ASTUnaryExpression(Kind kind, ASTPtr expression)
        : kind(kind)
        , expression(expression)
    {
    }

    Kind kind;
    ASTPtr expression;

    void dump(std::ostream& ostr) override
    {
        std::unordered_map<Kind, std::string> kindToStr = {
            { Kind::NEGATION, "-" },
        };

        ostr << "UnaryExpression(";
        ostr << kindToStr[kind];
        ostr << ", ";
        expression->dump(ostr);
        ostr << ")";
    }
};

}