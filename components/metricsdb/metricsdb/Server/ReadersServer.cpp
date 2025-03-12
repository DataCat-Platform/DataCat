#include <Poco/Net/HTTPServer.h>
#include <Poco/Net/HTTPServerParams.h>

#include <metricsdb/Logging/Macros.hpp>
#include <metricsdb/Server/ReaderRequestHandler.hpp>
#include <metricsdb/Server/ReadersServer.hpp>

namespace DataCat::DB {

void ReadersServer::run(const std::string& host, int port)
{
    DATACAT_LOG_INFO << "Starting readers server" << std::endl;

    socket = Poco::Net::ServerSocket(port);

    Poco::Net::HTTPServerParams* params = new Poco::Net::HTTPServerParams;
    params->setMaxQueued(100);
    params->setMaxThreads(1);

    auto requestHandlerFactory = new ReaderRequestHandlerFactory;

    httpServerPtr = std::make_unique<Poco::Net::HTTPServer>(requestHandlerFactory, socket, params);

    httpServerPtr->start();

    DATACAT_LOG_INFO << "Readers server is listening on " << host << ":" << port << std::endl;
}

}