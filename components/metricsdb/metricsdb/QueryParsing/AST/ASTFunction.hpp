#pragma once

#include <unordered_map>

#include <metricsdb/QueryParsing/AST/ASTBase.hpp>

namespace DataCat::DB::QueryParsing {

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

    void dump(std::ostream& ostr) override
    {
        std::unordered_map<Kind, std::string> kindToStr = {
            { Kind::CLAMP, "clamp" },
            { Kind::EXP, "exp" },
            { Kind::SORT, "sort" },
        };

        ostr << "Function(" << kindToStr[kind];
        for (auto arg : arguments) {
            ostr << ", ";
            arg->dump(ostr);
        }
        ostr << ")";
    }
};
}