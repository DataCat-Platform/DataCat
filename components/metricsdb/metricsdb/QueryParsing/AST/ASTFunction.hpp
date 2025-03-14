#pragma once

#include <unordered_map>

#include <metricsdb/QueryParsing/AST/ASTBase.hpp>

namespace DB::QueryParsing {

class ASTFunction : public ASTBase {
public:
    enum class Kind {
        CLAMP,
        EXP,
        SORT,
        // TODO: add more.
    };

    ASTFunction(const std::string& name, ASTPtrs arguments)
        : kind([](std::string _name) {
            std::unordered_map<std::string, Kind> nameToKindMap
                = { { "clamp", Kind::CLAMP }, { "exp", Kind::EXP }, { "sort", Kind::SORT } };

            return nameToKindMap[_name];
        }(name))
        , arguments(arguments)
    {
    }

    ASTFunction(Kind kind, ASTPtrs arguments)
        : kind(kind)
        , arguments(arguments)
    {
    }

    Kind kind;
    ASTPtrs arguments;
};
}