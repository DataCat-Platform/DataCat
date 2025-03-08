#pragma once

namespace DataCat::DB {

class GlobalConfig {
public:
    int setRetentionDuration(int);
    inline int getRetentionDuration() { return retentionDuration; }

private:
    static int retentionDuration;
};

}