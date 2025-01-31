# Using CMake

To build project using Cmake first make a directory to store build files:

```bash
mkdir cmake/build/
cd cmake/build/
```

Then call `cmake`:

```bash
cmake ../..
cmake --build .
```

This will create an executable called `mdb`, you can run it using:

```bash
./mdb
```
