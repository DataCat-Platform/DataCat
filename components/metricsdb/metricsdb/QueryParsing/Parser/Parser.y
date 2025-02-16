%require "3.2"
%language "c++"

%header

%code top {
#include <metricsdb/QueryParsing/Lexer/Lexer.hpp>
#define yylex lexer.lex
}

%code requires {
#include <string>
#include <vector>
#include <metricsdb/QueryParsing/Parser/Result.hpp>
#include <metricsdb/QueryParsing/AST/Base.hpp>
#include <metricsdb/QueryParsing/AST/TagSelector.hpp>
#include <metricsdb/QueryParsing/AST/MetricSelector.hpp>
#include <metricsdb/QueryParsing/AST/SelectQuery.hpp>

namespace DB::QueryParsing {
    class Lexer;
}

}

%skeleton "lalr1.cc"
%locations


%define api.value.type variant
%define api.namespace {DB::QueryParsing}
%define api.parser.class {Parser}

%parse-param {Lexer& lexer} {Result& result}

%token <std::string> IDENTIFIER
%token <std::string> STRING
%token <double> NUMBER
%token EQUAL                    '='
%token FORWARD_OPERATOR         "|>"
%token OPEN_PARENTHESIS         '('
%token CLOSE_PARENTHESIS        ')'
%token OPEN_SQUARE_BRACKET      '['
%token CLOSE_SQUARE_BRACKET     ']'
%token OPEN_BRACE               '{'
%token CLOSE_BRACE              '}'
%token PLUS                     '+'
%token MINUS                    '-'
%token ASTERISK                 '*'
%token SLASH                    '/'
%token COLON                    ':'
%token DOT                      '.'
%token END
%token ERROR

%type <Result> select_query
%type <AST::ASTPtr> expr
%type <AST::ASTPtr> metric_selector
%type <AST::ASTPtr> tag_selector
%type <std::vector<AST::ASTPtr>> tag_selectors

%type <std::string> name
%type <std::string> dot_separated_name

%left '+' '-'
%left '*' '/'
%precedence NEG

%start select_query

%%
select_query:
    expr { result.setAST($1); }
    ;

expr:
    metric_selector { $$ = $1; }
    /* | expr '+' expr { $$ = AST::FunctionCall::Create($1, $3, AST::BinaryOperator::OperatorType::PLUS); }
    | expr '-' expr { $$ = AST::BinaryOperator($1, $3, ); }
    | expr '*' expr { $$ = AST::BinaryOperator($1, $3, ); }
    | expr '/' expr { $$ = AST::BinaryOperator($1, $3, ); }
    | '-' expr %prec NEG { $$ = AST::Base(); }
    | '(' expr ')'       { $$ = AST::Base(); }
    | expr "|>"  */
    ;

metric_selector:
    name '{' tag_selectors '}' { $$ = AST::MetricSelector::Create($1, $3); }
    ;

tag_selectors:
    %empty                       { $$ = std::vector<AST::ASTPtr>(); }
    | tag_selectors tag_selector { $$ = $1; $$.push_back($2); }
    ;

tag_selector:
    name '=' name { $$ = AST::TagSelector::Create($1, $3); }
    ;

name:
    STRING               { $$ = $1; }
    | dot_separated_name { $$ = $1; }
    ;

dot_separated_name:
    IDENTIFIER                          { $$ = $1; }
    | dot_separated_name '.' IDENTIFIER { $$ = $1; $$ += $3; }
    ;

%%

void DB::QueryParsing::Parser::error(const location_type& location, const std::string& errorMessage)
{
    result.setErrorMessage(errorMessage + " @" + std::to_string(location.begin.column) + "..." + std::to_string(location.end.column) + "\n");
}