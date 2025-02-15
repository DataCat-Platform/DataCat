add_custom_command(
  OUTPUT
    "${CMAKE_CURRENT_BINARY_DIR}/Lexer.cpp"
  COMMAND
    ragel -o "${CMAKE_CURRENT_BINARY_DIR}/Lexer.cpp" "${CMAKE_CURRENT_SOURCE_DIR}/Lexer.l"
  DEPENDS
    "${CMAKE_CURRENT_SOURCE_DIR}/Lexer.l"
)

add_library(
  mdb_lexer
  STATIC
  "${CMAKE_CURRENT_BINARY_DIR}/Lexer.cpp"
)

target_include_directories(
  mdb_lexer
  PUBLIC
  "${PROJECT_SOURCE_DIR}"
  "${PROJECT_BINARY_DIR}"
)
