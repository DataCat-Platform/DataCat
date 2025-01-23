cmake_minimum_required(VERSION 3.20)

project(DatacatMetricsDB VERSION 1.0.0 LANGUAGES CXX)

# Outputs compile_commands.json file. This is needed
# for clangd to work properly.
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

set(CMAKE_CXX_STANDARD 23)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_executable(mdb)

# This set helps in avoiding absl errors.
# See this issue: https://github.com/protocolbuffers/protobuf/issues/12185
set(ABSL_ENABLE_INSTALL ON)

include(FetchContent)
FetchContent_Declare(
  gRPC
  GIT_REPOSITORY https://github.com/grpc/grpc
  GIT_TAG        v1.69.0
)
set(FETCHCONTENT_QUIET OFF)
FetchContent_MakeAvailable(gRPC)

target_link_libraries(mdb grpc++)

add_subdirectory("metricsdb")