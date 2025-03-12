#include <Poco/Net/HTTPMessage.h>
#include <ostream>

#include <Poco/Net/HTTPRequestHandler.h>
#include <Poco/Net/HTTPServerRequest.h>
#include <Poco/Net/HTTPServerResponse.h>

#include <metricsdb/Logging/Macros.hpp>
#include <metricsdb/Server/ReaderRequestHandler.hpp>

namespace DataCat::DB {

using Poco::Net::HTTPServerRequest;
using Poco::Net::HTTPServerResponse;

void ReaderRequestHandler::handleRequest(HTTPServerRequest& request, HTTPServerResponse& response)
{
    DATACAT_LOG_INFO << "New request" << std::endl;

    response.setContentType("application/json");
    std::ostream& outputStream = response.send();
    outputStream << "";
}

Poco::Net::HTTPRequestHandler* ReaderRequestHandlerFactory::createRequestHandler(
    const HTTPServerRequest& request)
{
    return new ReaderRequestHandler();
}

}