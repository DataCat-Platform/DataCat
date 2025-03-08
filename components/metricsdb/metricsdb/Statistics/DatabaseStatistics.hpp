#pragma once

namespace DataCat::DB {

class DatabaseStatistics {
public:
    static void BlockWritesInc();
    static void BlockReadsInc();
};

}