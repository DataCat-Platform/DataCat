#pragma once

#include <string>

#include <metricsdb/QueryParsing/AST/ASTBase.hpp>

namespace DataCat::DB::Core {

class Query {
public:
    using ASTPtr = QueryParsing::ASTPtr;

    Query(std::string text, ASTPtr AST)
        : text(text)
        , AST(AST)
    {
    }

    const std::string& getText() const { return text; }
    ASTPtr getAST() const { return AST; }
    void execute();

private:
    std::string text;
    ASTPtr AST;
};

}