#pragma once

#include <metricsdb/Storage/TimeSeries.hpp>

namespace DB::Storage {

class Block {
public:
    int getID() const;
    int getMinTimestamp() const;
    int getMaxTimestamp() const;
    int getNumberOfSamples() const;
    int getNumberOfTimeSeries() const;

private:
    int minTimestamp;
    int maxTimestamp;
};

}