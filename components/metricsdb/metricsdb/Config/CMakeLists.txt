configure_file("Version.cpp.in" "Version.cpp")

add_library(
    mdb_config
    STATIC
    "${CMAKE_CURRENT_BINARY_DIR}/Version.cpp"
)

target_include_directories(
    mdb_config
    PUBLIC
    "${PROJECT_SOURCE_DIR}"
)