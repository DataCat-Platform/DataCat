target_sources(mdb PUBLIC main.cpp)

add_subdirectory("config")
add_subdirectory("server")

target_link_libraries(mdb metricsdb_config metricsdb_server)