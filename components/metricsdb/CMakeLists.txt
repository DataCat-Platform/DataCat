cmake_minimum_required(VERSION 3.20)

project(DatacatMetricsDB LANGUAGES CXX)

# Outputs compile_commands.json file. This is needed
# for clangd to work properly.
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

set(CMAKE_CXX_STANDARD 23)
set(CMAKE_CXX_STANDARD_REQUIRED ON)