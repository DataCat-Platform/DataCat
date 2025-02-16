#include <gtest/gtest.h>
#include <metricsdb/QueryParsing/Lexer/Lexer.hpp>
#include <metricsdb/QueryParsing/Parser/Result.hpp>

TEST(Parser, MetricSelector)
{
    using namespace DB::QueryParsing;

    std::string query = "name{a=b c=d}";

    Lexer lexer(query.c_str(), query.c_str() + query.size());

    Result result;
    Parser parser(lexer, result);

    parser.parse();

    auto tree = result.getAST();
    if (tree) {
        SUCCEED() << tree->dumpTree();
    } else {
        FAIL() << result.getErrorMessage();
    }
}