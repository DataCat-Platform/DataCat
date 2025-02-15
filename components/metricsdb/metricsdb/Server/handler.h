#pragma once

#include <server/request.h>

namespace DB
{

class Handler
{
public:
    void handleRequest(Request &);
};

}