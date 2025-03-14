add_custom_command(
  OUTPUT
    "${CMAKE_CURRENT_BINARY_DIR}/Lexer.cpp"
  COMMAND
    ragel -o "${CMAKE_CURRENT_BINARY_DIR}/Lexer.cpp" "${CMAKE_CURRENT_SOURCE_DIR}/Lexer.l"
  DEPENDS
    "${CMAKE_CURRENT_SOURCE_DIR}/Lexer.l"
    "${CMAKE_CURRENT_BINARY_DIR}/Parser.hpp"
)

add_custom_command(
  OUTPUT
    "${CMAKE_CURRENT_BINARY_DIR}/Parser.hpp"
    "${CMAKE_CURRENT_BINARY_DIR}/Parser.cpp"
  COMMAND
    bison
    -o "${CMAKE_CURRENT_BINARY_DIR}/Parser.cpp"
    --header="${CMAKE_CURRENT_BINARY_DIR}/Parser.hpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/Parser.y"
  DEPENDS
    "${CMAKE_CURRENT_SOURCE_DIR}/Lexer.hpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/Parser.y"
)

add_library(mdb_query_parsing
  STATIC
    "${CMAKE_CURRENT_SOURCE_DIR}/Lexer.hpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/Result.hpp"
    "${CMAKE_CURRENT_BINARY_DIR}/Parser.hpp"
    "${CMAKE_CURRENT_BINARY_DIR}/Parser.cpp"
    "${CMAKE_CURRENT_BINARY_DIR}/Lexer.cpp"
    "${PROJECT_BINARY_DIR}/metricsdb/QueryParsing/Parser.hpp"
)

target_include_directories(mdb_query_parsing
  PUBLIC
    "${PROJECT_SOURCE_DIR}"
    "${PROJECT_BINARY_DIR}"
)

if (ENABLE_TESTS)
  target_link_libraries(mdb_unit_tests
    PRIVATE
      mdb_query_parsing
  )
  target_sources(mdb_unit_tests
    PUBLIC
      "Lexer.test.cpp"
      "Parser.test.cpp"
  )
endif()