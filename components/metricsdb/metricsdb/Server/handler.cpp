#include <server/handler.h>
#include <server/request.h>

namespace DB
{

void Handler::handleRequest(Request & request)
{
    std::string queryText = request.readQuery();
}

}