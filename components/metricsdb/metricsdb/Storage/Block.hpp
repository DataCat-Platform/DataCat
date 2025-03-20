#pragma once

#include <metricsdb/Storage/TimeSeries.hpp>
#include <metricsdb/Core/Types.hpp>

namespace DataCat::DB {

class Block {
public:
    int getID() const;
    int getMinTimestamp() const;
    int getMaxTimestamp() const;
    int getNumberOfSamples() const;
    int getNumberOfTimeSeries() const;

    void flushToDisk();

private:
    Time minTimestamp;
    Time maxTimestamp;
};

}