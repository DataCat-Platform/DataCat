add_executable(mdb
  main.cpp
)
target_include_directories(mdb
  PUBLIC
    "${PROJECT_SOURCE_DIR}"
)
set_target_properties(mdb
  PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY "${PROJECT_BINARY_DIR}"
)

add_subdirectory("Config")        # mdb_config
add_subdirectory("Runner")        # mdb_runner
add_subdirectory("Server")        # mdb_server
add_subdirectory("Core")          # mdb_core
add_subdirectory("QueryParsing")  # mdb_lexer, mdb_parser
# add_subdirectory("Storage")
# add_subdirectory("Writers")

target_link_libraries(mdb
  mdb_runner
)