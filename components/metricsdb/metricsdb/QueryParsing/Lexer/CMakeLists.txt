add_custom_command(
  OUTPUT
    "${CMAKE_CURRENT_BINARY_DIR}/Lexer.cpp"
  COMMAND
    ragel -o "${CMAKE_CURRENT_BINARY_DIR}/Lexer.cpp" "${CMAKE_CURRENT_SOURCE_DIR}/Lexer.l"
  DEPENDS
    "${CMAKE_CURRENT_SOURCE_DIR}/Lexer.l"
    "${CMAKE_CURRENT_SOURCE_DIR}/Lexer.hpp"
)

add_library(
  mdb_lexer
  STATIC
  "${CMAKE_CURRENT_BINARY_DIR}/Lexer.cpp"
  "${PROJECT_BINARY_DIR}/metricsdb/QueryParsing/Parser/Parser.hpp"
)

target_include_directories(
  mdb_lexer
  PUBLIC
  "${PROJECT_SOURCE_DIR}"
  "${PROJECT_BINARY_DIR}"
)