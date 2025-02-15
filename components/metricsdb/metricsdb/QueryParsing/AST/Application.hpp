#pragma once

#include <absl/container/inlined_vector.h>
#include <metricsdb/QueryParsing/AST/Base.hpp>
#include <metricsdb/QueryParsing/Common/FunctionKind.hpp>

namespace DB::QueryParsing::AST {

using ArgsList = absl::InlinedVector<ASTPtr, 5>;

class Application : public Base {
public:
private:
    ArgsList args;
    FunctionKind functionKind;
};

}