#pragma once

#include <iostream>
#include <ostream>

namespace DB::QueryExecution {

class QueryExecutor {
public:
    QueryExecutor();
    void execute();

private:
    std::ostream& outputStream;
};

}