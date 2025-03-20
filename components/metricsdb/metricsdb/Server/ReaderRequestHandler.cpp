#include <ostream>
#include <sstream>

#include <Poco/StreamCopier.h>
#include <Poco/Net/HTTPRequestHandler.h>
#include <Poco/Net/HTTPServerRequest.h>
#include <Poco/Net/HTTPServerResponse.h>

#include <metricsdb/Logging/Macros.hpp>
#include <metricsdb/Server/ReaderRequestHandler.hpp>
#include <metricsdb/Core/Query.hpp>
#include <metricsdb/Core/Errors.hpp>

namespace DataCat::DB {

using Poco::Net::HTTPServerRequest;
using Poco::Net::HTTPServerResponse;

void ReaderRequestHandler::handleRequest(HTTPServerRequest& request, HTTPServerResponse& response)
{
    DATACAT_LOG_INFO << "New request" << std::endl;

    response.setContentType("application/json");

    std::ostringstream oss;
    Poco::StreamCopier::copyStream(request.stream(), oss);
    
    std::string queryText = oss.str();
    ParserResult parserResult = QueryParser().parse(queryText);

    if (!parserResult) {
        std::ostream& ostr = response.send();
        ostr << QueryParseError();
        response.setStatus(HTTPServerResponse::HTTPStatus::HTTP_BAD_REQUEST);
    } 

    Query query(queryText);

    
    ostr << "";
}

Poco::Net::HTTPRequestHandler* ReaderRequestHandlerFactory::createRequestHandler(
    const HTTPServerRequest& request)
{
    return new ReaderRequestHandler();
}

}