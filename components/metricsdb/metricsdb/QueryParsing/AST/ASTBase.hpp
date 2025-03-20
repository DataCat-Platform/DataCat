#pragma once

#include <memory>
#include <vector>

namespace DataCat::DB::QueryParsing {

class ASTBase;
using ASTPtr = std::shared_ptr<ASTBase>;
using ASTPtrs = std::vector<std::shared_ptr<ASTBase>>;

class ASTBase {
public:
    virtual void dump(std::ostream&) = 0;
};

}