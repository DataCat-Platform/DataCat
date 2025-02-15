#pragma once

#include <filesystem>

namespace DB {

using Path = std::filesystem::path;

class Storage {
public:
    Path getRootDirectoryPath();

private:
    Path rootDirectoryPath;
};

}