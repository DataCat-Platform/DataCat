#include <metricsdb/Runner/DatabaseRunner.hpp>

int main(int argc, char** argv)
{
    using DataCat::DB::DatabaseRunner;

    DatabaseRunner runner;
    runner.run(argc, argv);
}