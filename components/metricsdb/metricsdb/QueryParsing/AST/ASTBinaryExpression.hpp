#pragma once

#include <unordered_map>

#include <metricsdb/QueryParsing/AST/ASTBase.hpp>
#include <metricsdb/QueryParsing/AST/ASTExpression.hpp>

namespace DataCat::DB::QueryParsing {

class ASTBinaryExpression : public ASTExpression {
public:
    enum class Kind {
        ADDITION,
        SUBSTRACTION,
        MULTIPLICATION,
        DIVISION,
    };

    ASTBinaryExpression(Kind kind, ASTPtr left, ASTPtr right)
        : kind(kind)
        , left(left)
        , right(right)
    {
    }

    Kind kind;
    ASTPtr left;
    ASTPtr right;

    void dump(std::ostream& ostr) override
    {
        std::unordered_map<Kind, std::string> kindToStr = {
            { Kind::ADDITION, "+" },
            { Kind::SUBSTRACTION, "-" },
            { Kind::DIVISION, "/" },
            { Kind::MULTIPLICATION, "*" },
        };

        ostr << "BinaryExpression(";
        ostr << kindToStr[kind];
        ostr << ", ";
        left->dump(ostr);
        ostr << ", ";
        right->dump(ostr);
        ostr << ")";
    }
};

}