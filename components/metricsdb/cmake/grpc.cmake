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

get_filename_component(proto_file "../../../../api/proto/metrics/v1/metrics.proto" ABSOLUTE)
get_filename_component(proto_file_dir "${proto_file}" DIRECTORY)

if (NOT EXISTS "${proto_file}")
  message(FATAL_ERROR "The proto file \"${proto_file}\" does not exist.")
endif()

set(
  output_headers_and_sources
  "${PROJECT_BINARY_DIR}/metrics.pb.cc"
  "${PROJECT_BINARY_DIR}/metrics.pb.h"
  "${PROJECT_BINARY_DIR}/metrics.grpc.pb.cc"
  "${PROJECT_BINARY_DIR}/metrics.grpc.pb.h"
)

add_custom_command(
  OUTPUT ${output_headers_and_sources}
  COMMAND $<TARGET_FILE:protoc>
  ARGS
    --grpc_out ${PROJECT_BINARY_DIR}
    --cpp_out ${PROJECT_BINARY_DIR}
    -I "${proto_file_dir}"
    -I "/usr/include"
    --plugin=protoc-gen-grpc=$<TARGET_FILE:grpc_cpp_plugin>
    "${proto_file}"
  DEPENDS "${proto_file}"
)

add_library(mdb_proto ${output_headers_and_sources})
include_directories("${PROJECT_BINARY_DIR}")
target_link_libraries(mdb_proto absl::check grpc++_reflection grpc++ libprotobuf)