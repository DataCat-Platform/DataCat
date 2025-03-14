#pragma once

#include <metricsdb/QueryParsing/AST/ASTExpression.hpp>

namespace DB::QueryParsing {

class ASTFunctionCall : public ASTExpression {
public:
    ASTFunctionCall(ASTPtr expression, ASTPtr function)
        : expression(expression)
        , function(function)
    {
    }

    ASTPtr expression;
    ASTPtr function;
};

}