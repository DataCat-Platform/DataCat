#pragma once

#include <metricsdb/QueryParsing/AST/ASTBase.hpp>

namespace DataCat::DB::QueryParsing {

struct QueryAST {
    ASTPtr ast;
};

}