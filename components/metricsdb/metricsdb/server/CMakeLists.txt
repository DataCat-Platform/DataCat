add_library(metricsdb_server STATIC "server.h" "server.cpp")
target_include_directories(metricsdb_server PUBLIC "..")