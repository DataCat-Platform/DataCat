#include <iostream>
#include <config/version.h>
#include <server/server.h>

int main(int argc, char ** argv)
{
    std::cout << 'v' << DB::VERSION_MAJOR << '.' << DB::VERSION_MINOR << '.' << DB::VERSION_PATCH << std::endl;

    DB::Server server;
    server.run();
}