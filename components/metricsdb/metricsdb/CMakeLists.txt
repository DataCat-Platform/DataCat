add_executable(mdb main.cpp)

add_subdirectory("Config")
add_subdirectory("Server")
add_subdirectory("QueryParsing")

target_link_libraries(
  mdb
  mdb_config
  mdb_server
  mdb_parser
  mdb_lexer
)

target_include_directories(mdb PUBLIC "${PROJECT_SOURCE_DIR}")

set_target_properties(
  mdb
  PROPERTIES
  RUNTIME_OUTPUT_DIRECTORY "${PROJECT_BINARY_DIR}"
)