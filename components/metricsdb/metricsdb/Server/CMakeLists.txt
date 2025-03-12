add_library(mdb_server
  STATIC
    "WritersServer.hpp"
    "WritersServer.cpp"
    "ReadersServer.hpp"
    "ReadersServer.cpp"
    "ReaderRequestHandler.hpp"
    "ReaderRequestHandler.cpp"
)
target_include_directories(mdb_server
  PUBLIC
    "${PROJECT_SOURCE_DIR}"
)

include("${PROJECT_SOURCE_DIR}/cmake/grpc.cmake")

target_link_libraries(mdb_server
  PRIVATE
    grpc++
    mdb_proto
)

find_package(Poco REQUIRED Foundation Net)

target_link_libraries(mdb_server
  PUBLIC
    Poco::Foundation
    Poco::Net
)