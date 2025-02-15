%require "3.2"
%language "c++"

%header

%code requires {
    #include <metricsdb/QueryParsing/Lexer/Lexer.hpp>
    #include <metricsdb/QueryParsing/AST/Base.hpp>
    #include <metricsdb/QueryParsing/AST/Tag.hpp>
    #include <metricsdb/QueryParsing/AST/MetricSelector.hpp>
    #include <metricsdb/QueryParsing/AST/SelectQuery.hpp>
    #include <string>
    #include <vector>

    #define yylex lexer.lex
}

%skeleton "lalr1.cc"
/* %locations */


%define api.value.type variant
%define api.namespace {DB::QueryParsing}
%define api.parser.class {Parser}
/* %define api.header.include {<metricsdb/QueryParsing/Parser/Parser.hpp>} */

%parse-param {Lexer& lexer} {AST::Query& result}

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

%type <AST::Base> expr
%type <AST::Tag> tag
%type <std::vector<AST::Tag>> tags
%type <AST::Query> query
%type <AST::MetricSelector> metric_selector

%left '+' '-'
%left '*' '/'
%precedence NEG

%start query

%%
query:
    %empty                  { result = AST::SelectQuery(); }
    | expr                  { result = AST::SelectQuery(); }
    ;

expr:
    metric_selector         { $$ = $1; }
    | expr[l] '+' expr[r]   { $$ = AST::Base(); }
    | expr[l] '-' expr[r]   { $$ = AST::Base(); }
    | expr[l] '*' expr[r]   { $$ = AST::Base(); }
    | expr[l] '/' expr[r]   { $$ = AST::Base(); }
    | '-' expr[e] %prec NEG { $$ = AST::Base(); }
    | '(' expr[e] ')'       { $$ = AST::Base(); }
    ;

metric_selector:
    STRING '{' tags '}'     { $$ = AST::MetricSelector(); }
    ;

tags:
    %empty                  { $$ = std::vector<AST::Tag>(); }
    | tags[t] tag[h]        { $$ = $t; $$.push_back($h); }
    ;

tag:
    STRING[key] ':' STRING[value] { $$ = AST::Tag{$key, $value}; }
    ;
%%

/* void DB::Parser::error(const location_type& loc, const std::string& err)
{
    message = err;
} */
