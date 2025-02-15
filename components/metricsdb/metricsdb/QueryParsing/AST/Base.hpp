#pragma once

#include <memory>

namespace DB::QueryParsing::AST {

class Base;
using ASTPtr = std::shared_ptr<Base>;

class Base {
public:
    void dumpTree();
};

}