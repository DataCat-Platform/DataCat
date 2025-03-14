#include <sstream>

#include <gtest/gtest.h>

#include <metricsdb/QueryParsing/AST/ASTFunctionCall.hpp>
#include <metricsdb/QueryParsing/Lexer.hpp>
#include <metricsdb/QueryParsing/Parser.hpp>
#include <metricsdb/QueryParsing/Result.hpp>

namespace DB::QueryParsing {

TEST(Parser, Query)
{
    std::string query = "{x=1 x.y=\"v\"}\n"
                        "|> exp\n"
                        "|> clamp 1 2";

    Result result;
    Lexer lexer(query.data(), query.data() + query.length());
    Parser parser(lexer, result);

    if (parser.parse()) {
        FAIL() << result.getMessage();
    }

    auto tree = result.getAST().get();

    std::stringstream ss;
    tree->dump(ss);

    FAIL() << ss.str();
}

}