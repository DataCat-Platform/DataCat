#pragma once

#include <cstdint>
namespace DB {

class Value {
public:
    enum class Kind : int8_t {
        Null,
        Double,
        String,
    };

    Value();

private:
    Kind kind;
};

}