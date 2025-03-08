#pragma once

#include <iostream>
#include <sstream>

#define TEST_COUT TestCout()

class TestCout : public std::stringstream {
public:
    ~TestCout()
    {
        std::cout << "\u001b[32m[          ] \u001b[33m" << str() << "\u001b[0m"
                  << std::flush;
    }
};