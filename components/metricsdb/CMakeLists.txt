cmake_minimum_required(VERSION 3.20)

project(DatacatMetricsDB VERSION 1.0.0 LANGUAGES C CXX)

set(CMAKE_CXX_STANDARD 23)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

option(ENABLE_TESTS "Build tests." ON)
if (ENABLE_TESTS)
  include("${PROJECT_SOURCE_DIR}/cmake/gtest.cmake")
  # file(
  #   GLOB_RECURSE
  #   all_test_sources
  #   CONFIGURE_DEPENDS
  #   "*.test.cpp"
  # )
  add_executable(
    mdb_unit_tests
    # ${all_test_sources}
  )
  target_link_libraries(mdb_unit_tests
    PRIVATE
      gtest_main
  )
  target_include_directories(mdb_unit_tests
    PUBLIC
      "${PROJECT_SOURCE_DIR}"
      "${PROJECT_BINARY_DIR}"
  )
endif()

add_subdirectory("metricsdb")