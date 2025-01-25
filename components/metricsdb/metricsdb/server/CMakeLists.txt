add_library(metricsdb_server STATIC "server.h" "server.cpp")
target_link_libraries(metricsdb_server grpc++ metrics_proto)
target_include_directories(metricsdb_server PUBLIC "..")