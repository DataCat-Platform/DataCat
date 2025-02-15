#include <iostream>
#include <metricsdb/Config/Version.hpp>
#include <metricsdb/Server/Server.hpp>

int main(int argc, char** argv)
{
    std::cout << "DataCat Metrics Database [v." << DB::VERSION_MAJOR << '.'
              << DB::VERSION_MINOR << '.' << DB::VERSION_PATCH
              << "]\nCopyright (C) 2025 DataCat\n"
              << std::endl;

    DB::Server server;
    server.run();
}