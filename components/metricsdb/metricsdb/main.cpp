#include <iostream>
#include <config/version.h>


int main(int argc, char ** argv)
{
    std::cout << DB::VERSION_MAJOR << '.' << DB::VERSION_MINOR << '.' << DB::VERSION_PATCH << std::endl;
}