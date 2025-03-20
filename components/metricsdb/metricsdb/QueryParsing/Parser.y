%require "3.2"
%language "c++"

%header


%code top {
#include <sstream>

#include <metricsdb/QueryParsing/Lexer.hpp>
#define yylex lexer.lex
}

%code requires {
#include <string>
#include <vector>
#include <optional>

#include <metricsdb/QueryParsing/AST/ASTBase.hpp>
#include <metricsdb/QueryParsing/AST/ASTBinaryExpression.hpp>
#include <metricsdb/QueryParsing/AST/ASTDataPointsSelector.hpp>
#include <metricsdb/QueryParsing/AST/ASTExpression.hpp>
#include <metricsdb/QueryParsing/AST/ASTFunction.hpp>
#include <metricsdb/QueryParsing/AST/ASTFunctionCall.hpp>
#include <metricsdb/QueryParsing/AST/ASTLiteral.hpp>
#include <metricsdb/QueryParsing/AST/ASTTagMatcher.hpp>
#include <metricsdb/QueryParsing/AST/ASTUnaryExpression.hpp>
#include <metricsdb/QueryParsing/AST/TimeSpan.hpp>
#include <metricsdb/QueryParsing/AST/createAST.hpp>
#include <metricsdb/QueryParsing/Result.hpp>

namespace DataCat::DB::QueryParsing {
    class Lexer;
}
}


%skeleton "lalr1.cc"
%locations

// Bison API values.
%define api.value.type variant
%define api.namespace {DataCat::DB::QueryParsing}
%define api.parser.class {Parser}

// Parameters accepted by the parser.
%parse-param {Lexer& lexer} {Result& result}


// Tokens (terminal symbols).

// With internal value.
%token <std::string>    IDENTIFIER
%token <std::string>    STRING
%token <int64_t>        DURATION
%token <int64_t>        INTEGER
%token <double>         FLOAT

// Without internal value.
%token EQUAL                    "="
%token FORWARD_OPERATOR         "|>"
%token OPEN_PARENTHESIS         "("
%token CLOSE_PARENTHESIS        ")"
%token OPEN_SQUARE_BRACKET      "["
%token CLOSE_SQUARE_BRACKET     "]"
%token OPEN_BRACE               "{"
%token CLOSE_BRACE              "}"
%token PLUS                     "+"
%token MINUS                    "-"
%token ASTERISK                 "*"
%token SLASH                    "/"
%token COLON                    ":"
%token AT                       "@"

// Special.
%token END 0
%token ERROR


// Operators precedence (from high to low).
%left "+" "-"
%left "*" "/"
%precedence NEGATION
%left "|>"


// Grammar rules types.

// AST nodes.
%type <ASTPtr>      data_points_selector
%type <ASTPtr>      expression
%type <ASTPtr>      function
%type <ASTPtr>      tag_matcher

// Helpers.
%type <ASTPtrs>     function_call_arguments_list
%type <ASTPtrs>     tag_matchers_list
%type <int64_t>     opt_timestamp
%type <TimeSpan>    opt_time_span
%type <int64_t>     opt_duration
%type <std::string> tag_key
%type <std::string> tag_value


// Initial rule.
%start query


// Grammar rules definitions.
%%
query:
    expression END { result.setAST($1); }
    ;

expression:
    data_points_selector
        { $$ = $1; }
    | INTEGER
        { $$ = createAST<ASTIntegerLiteral>($1); }
    | FLOAT
        { $$ = createAST<ASTFloatLiteral>($1); }
    | DURATION
        { $$ = createAST<ASTDurationLiteral>($1); }
    | STRING
        { $$ = createAST<ASTStringLiteral>($1); }
    | "(" expression ")"
        { $$ = $2; }
    | expression "+" expression
        { $$ = createAST<ASTBinaryExpression>(ASTBinaryExpression::Kind::ADDITION, $1, $3); }
    | expression "-" expression
        { $$ = createAST<ASTBinaryExpression>(ASTBinaryExpression::Kind::SUBSTRACTION, $1, $3); }
    | expression "*" expression
        { $$ = createAST<ASTBinaryExpression>(ASTBinaryExpression::Kind::MULTIPLICATION, $1, $3); }
    | expression "/" expression
        { $$ = createAST<ASTBinaryExpression>(ASTBinaryExpression::Kind::DIVISION, $1, $3); }
    | "-" expression %prec NEGATION
        { $$ = createAST<ASTUnaryExpression>(ASTUnaryExpression::Kind::NEGATION, $2); }
    | expression "|>" function
        { $$ = createAST<ASTFunctionCall>($1, $3); }
    ;

// Function call. Example: clamp 1 -1
function:
    IDENTIFIER function_call_arguments_list { $$ = createAST<ASTFunction>($1, $2); }
    ;
function_call_arguments_list:
    %empty                                      { $$ = std::vector<ASTPtr>(); }
    | function_call_arguments_list expression   { $$ = $1; $$.push_back($2); }
    ;

// Data points selector. Example: {method=GET cid=1}[2h:] @ 12345
data_points_selector:
    "{" tag_matchers_list "}" opt_time_span opt_timestamp { $$ = createAST<ASTDataPointsSelector>($2, $4, $5); }
    ;
opt_timestamp:
    %empty          { $$ = static_cast<int64_t>(0); }
    | "@" INTEGER   { $$ = $2; }
    ;
opt_time_span:
    %empty          { $$ = TimeSpan{}; }
    | "[" opt_duration ":" opt_duration "]"   { $$ = TimeSpan{$2, $4}; }
    ;
opt_duration:
    %empty      { $$ = static_cast<int64_t>(0); }
    | DURATION  { $$ = $1; }
    ;
tag_matchers_list:
    %empty                          { $$ = std::vector<ASTPtr>(); }
    | tag_matchers_list tag_matcher { $$ = $1; $$.push_back($2); }
    ;
tag_matcher:
    tag_key "=" tag_value { $$ = createAST<ASTTagMatcher>(ASTTagMatcher::Kind::EQUAL, $1, $3); }
    // TODO: add != and other operators.
    ;
tag_key:
    STRING          { $$ = $1; }
    | IDENTIFIER    { $$ = $1; }
tag_value:
    STRING          { $$ = $1; }
    | INTEGER       { $$ = std::to_string($1); }
    | IDENTIFIER    { $$ = $1; }
%%

void DB::QueryParsing::Parser::error(const location_type& location, const std::string& errorMessage)
{
    std::stringstream ss(errorMessage);
    ss << "Bad query: parsing failed at position (" << location.begin.line << "," << location.begin.column << ").";
    result.error(ss.str());
}