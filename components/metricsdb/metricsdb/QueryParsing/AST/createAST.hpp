#pragma once

#include <memory>
#include <metricsdb/QueryParsing/AST/ASTBase.hpp>

namespace DataCat::DB::QueryParsing {

template <class T, class... Args> ASTPtr createAST(Args... args) { return std::make_shared<T>(args...); }

}