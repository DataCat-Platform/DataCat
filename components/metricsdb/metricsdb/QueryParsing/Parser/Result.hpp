#pragma once

#include <metricsdb/QueryParsing/AST/Base.hpp>

namespace DB::QueryParsing {

class Result {
public:
    AST::ASTPtr getAST() const { return tree; }
    void setAST(AST::ASTPtr tree) { this->tree = tree; }

    void setErrorMessage(std::string message) { errorMessage = message; }
    const std::string& getErrorMessage() const { return errorMessage; }

private:
    AST::ASTPtr tree;
    std::string errorMessage;
};

}