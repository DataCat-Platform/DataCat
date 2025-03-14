#include <gtest/gtest.h>
#include <metricsdb/QueryParsing/Lexer.hpp>
#include <metricsdb/QueryParsing/Result.hpp>

TEST(Parser, MetricSelector)
{
    // using namespace ::DB::QueryParsing;

    // std::string query = "name{a=b} @ 123242224";

    // ASTBase tree = ASTSelectQuery(ASTFunctionCall("clamp",
    //     {
    //         ASTLiteral(1),
    //         ASTLiteral(-1),
    //     },
    //     ASTSelector(ASTTagMatcher("a", "b"))));

    // Lexer lexer(query.c_str(), query.c_str() + query.size());

    // Result result;
    // Parser parser(lexer, result);

    // parser.parse();

    // auto tree = result.getAST();
    // if (tree) {
    //     TEST_COUT << "tree: " << tree->dumpTree() << std::endl;
    //     SUCCEED();
    // } else {
    //     FAIL() << result.getErrorMessage();
    // }
}