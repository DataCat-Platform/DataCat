#pragma once

#include <metricsdb/QueryParsing/Parser/Parser.hpp>

namespace DB::QueryParsing {

class Lexer {
    using ValueType = Parser::value_type;

public:
    Lexer();

    void lex(ValueType value);

private:
    /*
    These variables are needed by Ragel.

        - cs:   current state
        - p:    data pointer
        - pe:   data end pointer
        - eof:  end of file pointer
        - act:  a variable sometimes used by scanner
        - ts:   a pointer to character data
        - te:   a pointer to character data
    */

    int cs;
    const char* p;
    const char* pe;

    const char* eof;

    int act;
    const char* ts;
    const char* te;
};

}