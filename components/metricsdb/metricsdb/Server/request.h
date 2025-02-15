#pragma once

#include <istream>

namespace DB
{

class Request
{
public:
    void read();
    std::string readQuery();

private:
    std::istream contentStream;
};

}