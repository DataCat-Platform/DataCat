#include <server/server.h>

#include <grpcpp/grpcpp.h>

#include <metrics.grpc.pb.h>
#include "metrics.pb.h"

namespace DB
{

namespace
{

using datacat::DataCatMetricExporter;
using datacat::SendMetricsRequest;
using google::protobuf::Empty;
using grpc::ServerContext;
using grpc::Status;

class MetricsServiceImpl final : public DataCatMetricExporter::Service
{
    Status SendMetrics(
        ServerContext * context, const SendMetricsRequest * request, Empty * response) override
    {
        return grpc::Status::OK;
    }
};

}

void Server::run()
{
    std::string address = "0.0.0.0:8000";
    MetricsServiceImpl service;

    grpc::ServerBuilder builder;
    builder.AddListeningPort(address, grpc::InsecureServerCredentials());
    builder.RegisterService(&service);
    auto server = builder.BuildAndStart();

    std::cout << "Server listening on: " << address << std::endl;

    server->Wait();
}

}