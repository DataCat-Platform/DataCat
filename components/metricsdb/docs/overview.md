# DataCat Metrics DB Overview

## CatQL

Each expression in CatQL can produce either of the following data types: `Number`, `String`, `TimeSpan` or `TimeSeries`.

```pgsql
metric{method=GET}[-5m:1m]
```
