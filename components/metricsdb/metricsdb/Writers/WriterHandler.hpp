#pragma once

#include <metricsdb/Writers/WriteRequest.hpp>

namespace DataCat::DB::Storage {

class WriterHandler {
public:
    void handle(const WriteRequest& request);
};

}