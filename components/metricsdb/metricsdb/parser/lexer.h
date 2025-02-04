#pragma once

#include <metricsdb/parser/parser.h>

namespace DB {

class Lexer {
    using ValueType = Parser::semantic_value;
    using LocationType = Parser::location_type;

public:
    Lexer();

    void lex(ValueType value, LocationType location);

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