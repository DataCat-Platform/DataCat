# # TODO: think about making lexer into its own lib. 
# add_library(
#   mdb_lexer
#   STATIC
#   "${CMAKE_CURRENT_SOURCE_DIR}/lexer.h"
#   "${CMAKE_CURRENT_BINARY_DIR}/lexer.c"
# )

add_library(
  mdb_parser
  STATIC
  "parser.h"
  "parser.cpp"
  "${CMAKE_CURRENT_BINARY_DIR}/parser.hh"
  "${CMAKE_CURRENT_BINARY_DIR}/lexer.cc"
)
target_include_directories(
  mdb_parser
  PUBLIC
  "${PROJECT_SOURCE_DIR}"
  "${PROJECT_BINARY_DIR}"
)

add_custom_command(
  OUTPUT
    "${CMAKE_CURRENT_BINARY_DIR}/parser.c"
    "${CMAKE_CURRENT_SOURCE_DIR}/parser.h"
  COMMAND
    bison -o "${CMAKE_CURRENT_BINARY_DIR}/parser.c" "${CMAKE_CURRENT_SOURCE_DIR}/parser.y"
  DEPENDS
    "${CMAKE_CURRENT_SOURCE_DIR}/lexer.h"
    "${CMAKE_CURRENT_SOURCE_DIR}/parser.y"
)

add_custom_command(
  OUTPUT
    "${CMAKE_CURRENT_BINARY_DIR}/lexer.c"
  COMMAND
    ragel -o "${CMAKE_CURRENT_BINARY_DIR}/lexer.c" "${CMAKE_CURRENT_SOURCE_DIR}/lexer.l"
  DEPENDS
    "${CMAKE_CURRENT_SOURCE_DIR}/lexer.l"
)