#pragma once

#include <memory>
#include <vector>

namespace DB::QueryParsing {

class ASTBase;
using ASTPtr = std::shared_ptr<ASTBase>;
using ASTPtrs = std::vector<std::shared_ptr<ASTBase>>;

class ASTBase { };

}