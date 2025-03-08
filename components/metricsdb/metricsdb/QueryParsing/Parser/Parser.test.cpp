#include <gtest/gtest.h>
#include <metricsdb/QueryParsing/Lexer/Lexer.hpp>
#include <metricsdb/QueryParsing/Parser/Result.hpp>
#include <tests/TestUtil/TestOutput.hpp>

TEST(Parser, MetricSelector)
{
    using namespace ::DB::QueryParsing;

    std::string query = "name{a=b}";

    Lexer lexer(query.c_str(), query.c_str() + query.size());

    Result result;
    Parser parser(lexer, result);

    parser.parse();

    auto tree = result.getAST();
    if (tree) {
        TEST_COUT << "tree: " << tree->dumpTree() << std::endl;
        SUCCEED();
    } else {
        FAIL() << result.getErrorMessage();
    }
}