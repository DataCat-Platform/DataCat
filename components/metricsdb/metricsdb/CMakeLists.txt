target_sources(mdb PUBLIC main.cpp)

add_subdirectory("config")

target_link_libraries(mdb metricsdb_config)