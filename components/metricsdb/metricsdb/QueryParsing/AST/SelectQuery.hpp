#pragma once

#include <metricsdb/QueryParsing/AST/Base.hpp>

namespace DB::QueryParsing::AST {

class SelectQuery : public Base {
public:
    SelectQuery(ASTPtr expression)
        : expression(expression)
    {
    }

    static ASTPtr create(ASTPtr expression)
    {
        return std::make_shared<SelectQuery>(expression);
    }

private:
    ASTPtr expression;
};

}