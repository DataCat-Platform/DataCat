#pragma once

#include <metricsdb/QueryParsing/AST/Base.hpp>
#include <string>

namespace DB::QueryParsing::AST {

class TagSelector : public Base {
public:
    enum class Kind : uint8_t {
        EXACT_MATCH,
    };

    TagSelector(const std::string& key, const std::string& value)
        : key(std::move(key))
        , value(std::move(value))
    {
    }

    Kind getKind() { return kind; }
    const std::string& getValue() { return value; }
    const std::string& getKey() { return key; }

    static ASTPtr Create(const std::string& key, const std::string& value)
    {
        return std::make_shared<TagSelector>(key, value);
    }

private:
    std::string key;
    std::string value;
    Kind kind;
};

}