configure_file("version.cpp.in" "version.cpp")
add_library(metricsdb_config STATIC "version.h" "${CMAKE_CURRENT_BINARY_DIR}/version.cpp")
target_include_directories(metricsdb_config PUBLIC "..")