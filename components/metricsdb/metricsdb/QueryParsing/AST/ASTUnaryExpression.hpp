#pragma once

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
};

}