#include "lexer.h"

namespace DB {

%%{
    machine lexer;
    main := |*
        digit+ => {
            token = Parser::token::NUMBER;
            Parser::semantic_type num(strtol(std::string(ts, te).c_str(), 0, 10));
            val->move<int>(num);
            fbreak;
        };

        '|>' => { token = Parser::token::FORWARD; fbreak; };

        alpha.alnum* => {
            token = Parser::token::NAME;
            Parser::semantic_type str(std::string(ts, te));
            value->move<std::string>(str);
            fbreak;
        };

        ':' => { token = Parser::token::COLON; fbreak; };
        '+' => { token = Parser::token::PLUS; fbreak; };
        '-' => { token = Parser::token::MINUS; fbreak; };
        '*' => { token = Parser::token::MULTIPLY; fbreak; };
        '/' => { token = Parser::token::DIVIDE; fbreak; };
        '(' => { token = Parser::token::OPENING_PARENTHESIS; fbreak; };
        ')' => { token = Parser::token::CLOSING_PARENTHESIS; fbreak; };
        '{' => { token = Parser::token::OPENING_CURVY_BRACE; fbreak; };
        '}' => { token = Parser::token::CLOSING_CURVY_BRACE; fbreak; };
        '[' => { token = Parser::token::OPENING_SQUARE_BRACKET; fbreak; };
        ']' => { token = Parser::token::CLOSING_SQUARE_BRACKET; fbreak; };

        // '\n' => { token = Parser::token::EOL; fbreak; };

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

Parser::token_type Lexer::lex(Parser::semantic_type* value, Parser::location_type* location) {
    Parser::token_type token = Parser::token::END;
    
    %%write exec;
    
    location->begin.column = ts - s;
    location->end.column = te - s;

    if (token == Parser::token::END && p != pe && te != pe) {
        // std::cerr << "Unexpected input: \"" << std::string(te, pe) << "\"" << std::endl;
        token = Parser::token::ERROR;
    }

    return token;
}

}