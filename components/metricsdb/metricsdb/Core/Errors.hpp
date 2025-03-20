#pragma once

#include <ostream>

namespace DataCat::DB {


class QueryParseError {
    
};

std::ostream& operator<<(std::ostream& ostr, QueryParseError error) {
    ostr << "Parser error";
    return ostr;
}


}