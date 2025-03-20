#include <cstdio>
#include <string>
#include <vector>

#include <gtest/gtest.h>

#include <metricsdb/QueryParsing/Lexer.hpp>
#include <metricsdb/QueryParsing/Parser.hpp>

namespace DataCat::DB::QueryParsing {

namespace {
    struct Case {
        std::string text;
        std::vector<Parser::token_type> tokens;
    };

    std::vector<Case> getTestCases()
    {
        return {
            { "1", { Parser::token::INTEGER } },
            { "1ms", { Parser::token::DURATION } },
            { "1s", { Parser::token::DURATION } },
            { "1m", { Parser::token::DURATION } },
            { "1h", { Parser::token::DURATION } },
            { "1d", { Parser::token::DURATION } },
            { "1w", { Parser::token::DURATION } },
            { "1y", { Parser::token::DURATION } },
            { "\"string\"", { Parser::token::STRING } },
            { "id", { Parser::token::IDENTIFIER } },
        };
    }
}

TEST(Lexer, Lex)
{
    auto testCases = getTestCases();

    auto value = new Parser::value_type;
    auto location = new Parser::location_type;

    for (auto& testCase : testCases) {
        Lexer lexer(testCase.text.data(), testCase.text.data() + testCase.text.length());

        for (auto trueToken : testCase.tokens) {
            auto token = lexer.lex(value, location);
            ASSERT_EQ(token, trueToken);
        }
    }

    delete value;
    delete location;
}

}