#pragma once

#include <metricsdb/QueryParsing/AST/ASTBase.hpp>
#include <metricsdb/QueryParsing/AST/ASTExpression.hpp>

namespace DB::QueryParsing {

class ASTBinaryExpression : public ASTExpression {
public:
    enum class Kind {
        ADDITION,
        SUBSTRACTION,
        MULTIPLICATION,
        DIVISION,
    };

    ASTBinaryExpression(Kind kind, ASTPtr left, ASTPtr right)
        : kind(kind)
        , left(left)
        , right(right)
    {
    }

    Kind kind;
    ASTPtr left;
    ASTPtr right;
};

}