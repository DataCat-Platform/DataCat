# Metrics Database

Metrics database allows for a better efficiency in processing CatQL queries.

# Metric Types

Supported metric types are `Counter`, `Gauge`, and `Histogram`, as described in Prometheus https://prometheus.io/docs/concepts/metric_types/.

# Build using Cmake

To build project using Cmake first make a directory to store build files:

```bash
mkdir build
cd build
```

Then call `cmake`:

```bash
cmake ..
cmake --build .
```

This will create an executable called `mdb`, you can run it using:

```bash
./mdb
```