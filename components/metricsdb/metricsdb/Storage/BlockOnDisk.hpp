#pragma once

#include <metricsdb/Storage/Encoding.hpp>

namespace DB::Storage {

class BlockOnDisk {
public:
    const char* getRawData() const;
    EncodingType getCompressionMethod() const;
};

}