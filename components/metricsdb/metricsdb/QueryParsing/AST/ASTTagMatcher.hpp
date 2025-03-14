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

    Kind kind;
    std::string key;
    std::string value;

    void dump(std::ostream& ostr) override { ostr << "TagMatcher(=, " << key << ", " << value << ")"; }
};

}