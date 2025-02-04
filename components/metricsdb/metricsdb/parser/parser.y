%require "3.2"
%language "c++"

%header

%code top {
    #include <metricsdb/parser/lexer.h>
    #include <metricsdb/parser/ast/ast.h>
    #include <string>
    #include <vector>

    #define yylex lexer.lex

    using DB::Parser;
}

/* %skeleton "lalr1.cc"
%locations
*/

%define api.value.type variant
%define api.namespace {DB}
%define api.parser.class {Parser}
%define api.header.include {<metricsdb/parser/parser.h>}

%parse-param {DB::Lexer& lexer} {std::shared_ptr<Ast>& result}

%token <std::string> NAME
%token <std::string> STRING
%token <double> NUMBER
%token FORWARD                  "|>"
%token OPENING_PARENTHESIS      '('
%token CLOSING_PARENTHESIS      ')'
%token OPENING_SQUARE_BRACKET   '['
%token CLOSING_SQUARE_BRACKET   ']'
%token OPENING_CURVY_BRACE      '{'
%token CLOSING_CURVY_BRACE      '}'
%token PLUS                     '+'
%token MINUS                    '-'
%token MULTIPLY                 '*'
%token DIVIDE                   '/'
%token COLON                    ':'

%nterm <Tag> tag
%nterm <std::vector<Tag>> tags
%nterm <>

%left '+' '-'
%left '*' '/'
%precedence NEG

%start query
%%
query:
    %empty { result = ?; }
    | expr { result = $expr; }
    ;

expr:
    metric_selector
    | expr "|>" funcs
    | expr '+' expr
    | expr '-' expr
    | expr '*' expr
    | expr '/' expr
    | '-' expr %prec NEG
    | '(' expr ')'
    ;

metric_selector:
    name '{' tags '}' timespan.opt
    ;

tags:
    %empty { $$ = std::vector<Tag>(); }
    | tags tag { $$ = $tags; $$.push_back($tag); }
    ;

tag:
    name[key] ':' STRING[value] { $$ = DB::AST::Tag($key, $value); }
    ;

name:
    STRING;

%%

void DB::Parser::error(const location_type& loc, const std::string& err)
{
    message = err;
}
