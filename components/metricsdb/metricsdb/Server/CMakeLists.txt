add_library(mdb_server
  STATIC
    "Server.hpp"
    "Server.cpp"
)
target_include_directories(mdb_server
  PUBLIC
    "${PROJECT_SOURCE_DIR}"
)

include("${PROJECT_SOURCE_DIR}/cmake/grpc.cmake")

target_link_libraries(mdb_server
  grpc++
  mdb_proto
)