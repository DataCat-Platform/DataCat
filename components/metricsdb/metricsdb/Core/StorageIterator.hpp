#pragma once

#include <metricsdb/Core/Types.hpp>

namespace DataCat::DB::Core {

class StorageIterator {
public:
    Time getCurrentTime();

    void operator++() {

    }

private:
    Time currentTime;
};

}