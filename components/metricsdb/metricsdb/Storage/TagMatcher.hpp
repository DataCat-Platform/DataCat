#pragma once

#include <string>

namespace DataCat::DB::Storage {

struct TagMatcher {
    enum class Type {
        EQUAL,
    };

    Type type;
    std::string key;
    std::string value;
};

TagMatcher::Type stringToType(std::string op);

}