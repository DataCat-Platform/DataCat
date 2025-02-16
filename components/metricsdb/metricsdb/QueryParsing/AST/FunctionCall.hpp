#include <cstdint>
#include <metricsdb/QueryParsing/AST/Base.hpp>

namespace DB::QueryParsing::AST {

/*
A call to the built-in function or operator.
*/
class FunctionCall : public Base {
public:
    enum class Kind : uint8_t {
        ADD,
        SUBSTRACT,
        MULTIPLY,
        DIVIDE,
        SORT,
        MIN,
        MAX,
        CLAMP,
        EXP,
    };

    FunctionCall(ASTPtr operands, Kind kind)
        : operands(operands)
        , kind(kind)
    {
    }

    const ASTPtr getOperands() { return operands; }
    const Kind getKind() { return kind; }

private:
    ASTPtr operands;
    Kind kind;
};

}