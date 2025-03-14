#pragma once

#include <metricsdb/QueryParsing/AST/ASTBase.hpp>

namespace DB::QueryParsing {

class Result {
public:
    Result()
        : isOk(true)
    {
    }

    void error(const std::string& message)
    {
        this->message = message;
        isOk = false;
    }

    ASTPtr getAST() { return tree; }
    void setAST(ASTPtr tree) { this->tree = tree; }

    std::string& getMessage() { return message; }

    bool ok() { return isOk; }

private:
    ASTPtr tree;
    std::string message;
    bool isOk;
};

}