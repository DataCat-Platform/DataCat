#pragma once

#include <string>

namespace DB::QueryParsing::AST {

struct Tag {
    std::string key;
    std::string value;
};

}