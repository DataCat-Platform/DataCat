#include <metricsdb/QueryParsing/Lexer/Lexer.hpp>
#include <iostream>

namespace DB::QueryParsing {

%%{
    machine lexer;
    main := |*
        digit+ => {
            token = Parser::token::NUMBER;
            Parser::semantic_type number(strtol(std::string(ts, te).c_str(), nullptr, 10));
            value->move<int>(number);
            fbreak;
        };

        alpha.alnum* => {
            token = Parser::token::IDENTIFIER;
            Parser::semantic_type identifier(std::string(ts, te));
            value->move<std::string>(identifier);
            fbreak;
        };

        '"' alnum* '"' => {
            token = Parser::token::STRING;
            Parser::semantic_type str(std::string(ts + 1, te - 1));
            value->move<std::string>(str);
            fbreak;
        };

        '|>'    => { token = Parser::token::FORWARD_OPERATOR; fbreak; };
        '.'     => { token = Parser::token::DOT; fbreak; };
        ':'     => { token = Parser::token::COLON; fbreak; };
        '+'     => { token = Parser::token::PLUS; fbreak; };
        '-'     => { token = Parser::token::MINUS; fbreak; };
        '*'     => { token = Parser::token::ASTERISK; fbreak; };
        '/'     => { token = Parser::token::SLASH; fbreak; };
        '('     => { token = Parser::token::OPEN_PARENTHESIS; fbreak; };
        ')'     => { token = Parser::token::CLOSE_PARENTHESIS; fbreak; };
        '{'     => { token = Parser::token::OPEN_BRACE; fbreak; };
        '}'     => { token = Parser::token::CLOSE_BRACE; fbreak; };
        '['     => { token = Parser::token::OPEN_SQUARE_BRACKET; fbreak; };
        ']'     => { token = Parser::token::CLOSE_SQUARE_BRACKET; fbreak; };
        '='     => { token = Parser::token::EQUAL; fbreak; };

        space;
    *|;
    write data;
}%%


Lexer::Lexer(const char *p, const char *pe)
    : p(p)
    , pe(pe)
    , eof(pe)
    , s(p) { 
    %%write init;
}

Parser::token_type Lexer::lex(ValueType value, LocationType location) {
    Parser::token_type token = Parser::token::END;
    
    %%write exec;
    
    location->begin.column = ts - s;
    location->end.column = te - s;

    if (token == Parser::token::END && p != pe && te != pe) {
        std::cerr << "Unexpected input: \"" << std::string(te, pe) << "\"" << std::endl;
        token = Parser::token::ERROR;
    }

    return token;
}

}