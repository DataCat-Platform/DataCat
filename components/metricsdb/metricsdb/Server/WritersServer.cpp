#include <cstdlib>
#include <stdlib.h>

#include <grpcpp/grpcpp.h>
#include <grpcpp/server.h>

#include <metrics.grpc.pb.h>
#include <metrics.pb.h>
#include <metricsdb/Logging/Macros.hpp>
#include <metricsdb/Server/WritersServer.hpp>
#include <metricsdb/Storage/MetricsSample.hpp>

namespace DataCat::DB {

namespace {

    using datacat::DataCatMetricExporter;
    using datacat::SendMetricsRequest;
    using google::protobuf::Empty;
    using grpc::ServerContext;
    using grpc::Status;

    class MetricsServiceImpl final : public DataCatMetricExporter::Service {
    public:
        MetricsServiceImpl(Database& db)
            : db(db)
        {
        }

    private:
        Status SendMetrics(
            ServerContext* context, const SendMetricsRequest* request, Empty* response) override
        {
            DATACAT_LOG_INFO << "Receive " << request->ByteSizeLong() << "B of metrics" << std::endl;

            auto metrics = request->metrics();

            Storage::MetricsSample sample;
            db.writeMetrics(sample);

            return grpc::Status::OK;
        }

        Database& db;
    };

}

WritersServer::WritersServer(Database& db)
    : db(db)
{
}

void WritersServer::run(const std::string& host, const std::string& port)
{
    DATACAT_LOG_INFO << "Starting server" << std::endl;

    std::string address = host + ":" + port;

    MetricsServiceImpl service(db);

    grpc::ServerBuilder builder;
    builder.AddListeningPort(address, grpc::InsecureServerCredentials());
    builder.RegisterService(&service);

    auto server = builder.BuildAndStart();

    if (!server) {
        DATACAT_LOG_INFO << "Unable to start a server" << std::endl;
        exit(EXIT_FAILURE);
    }

    DATACAT_LOG_INFO << "Server is listening on " << address << std::endl;
}

}