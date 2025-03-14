#pragma once

#include <string>

#include <metricsdb/QueryParsing/AST/ASTBase.hpp>

namespace DB::QueryParsing {

class ASTTagMatcher : public ASTBase {
public:
    enum class Kind {
        EQUAL,
    };

    ASTTagMatcher(Kind kind, std::string& key, std::string& value)
        : kind(kind)
        , key(key)
        , value(value)
    {
    }

    std::string key;
    std::string value;
    Kind kind;
};

}