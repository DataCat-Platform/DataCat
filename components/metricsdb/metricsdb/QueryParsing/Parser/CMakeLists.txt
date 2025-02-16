add_custom_command(
  OUTPUT
    "${CMAKE_CURRENT_BINARY_DIR}/Parser.cpp"
    "${CMAKE_CURRENT_BINARY_DIR}/Parser.hpp"
  COMMAND
    bison
    -o "${CMAKE_CURRENT_BINARY_DIR}/Parser.cpp"
    --header="${CMAKE_CURRENT_BINARY_DIR}/Parser.hpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/Parser.y"
  DEPENDS
    "${PROJECT_SOURCE_DIR}/metricsdb/QueryParsing/Lexer/Lexer.hpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/Parser.y"
)

add_library(
  mdb_parser
  STATIC
  "${PROJECT_SOURCE_DIR}/metricsdb/QueryParsing/Lexer/Lexer.hpp"
  "${CMAKE_CURRENT_BINARY_DIR}/Parser.hpp"
  "${CMAKE_CURRENT_BINARY_DIR}/Parser.cpp"
  "${CMAKE_CURRENT_SOURCE_DIR}/Result.hpp"
)

target_include_directories(
  mdb_parser
  PUBLIC
  "${PROJECT_SOURCE_DIR}"
  "${PROJECT_BINARY_DIR}"
)
