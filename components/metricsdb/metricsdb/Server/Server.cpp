#include <metricsdb/Server/Server.hpp>

#include <grpcpp/grpcpp.h>

#include <metrics.grpc.pb.h>
#include <metrics.pb.h>

#include <metricsdb/Logging/Macros.hpp>

namespace DataCat::DB {

namespace {

    using datacat::DataCatMetricExporter;
    using datacat::Metric;
    using datacat::SendMetricsRequest;
    using google::protobuf::Empty;
    using grpc::ServerContext;
    using grpc::Status;

    class MetricsServiceImpl final : public DataCatMetricExporter::Service {
        Status SendMetrics(ServerContext* context,
            const SendMetricsRequest* request, Empty* response) override
        {
            // DATACAT_LOG_INFO << "Receive " << request->GetCachedSize()
            //                  << "B of metrics" << std::endl;

            // auto metrics = request->metrics();
            // for (auto& metric : metrics) {
            //     auto tags = metric.tags();

            //     switch (metric.data_points_case()) {
            //     case Metric::DATA_POINTS_NOT_SET: {
            //     }
            //     case Metric::kGauge: {
            //     }
            //     case Metric::kCounter: {
            //     }
            //     case Metric::kHistogram: {
            //     }
            //     }

            //     for (auto& tag : tags) {
            //         std::cout << tag.key() << ": " << tag.value() <<
            //         std::endl;
            //     }
            // }

            return grpc::Status::OK;
        }
    };

}

Server::Server(Database& db)
    : db(db)
{
}

void Server::run(const std::string& host, const std::string& port)
{
    DATACAT_LOG_INFO << "Starting gRPC server" << std::endl;

    std::string address = host + ":" + port;
    MetricsServiceImpl service;

    grpc::ServerBuilder builder;
    builder.AddListeningPort(address, grpc::InsecureServerCredentials());
    builder.RegisterService(&service);
    server = builder.BuildAndStart();

    DATACAT_LOG_INFO << "gRPC server listening on: " << address << std::endl;
}

void Server::wait()
{
    if (server) {
        server->Wait();
    }
}

}