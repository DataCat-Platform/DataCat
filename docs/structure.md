# How to organize folders and projects

```
├── src
|  ├── DataCat.Server
|  |   ├── DataCat.Api
|  |   ├── DataCat.QL (Lexer, Parser)
|  |   ├── DataCat.DbClient
|  |   └── DataCat.AlertConfigurator
|  |       └── DataCat.AlertConfigurator.Telegram
|  |
|  ├── DataCat.Collector
|  |  ├── DataCat.Collector.Api
|  |  └── DataCat.Collector.Core
|  |
|  ├── DataCat.Adapter
|  |   ├── DataCat.Adapter.DataHawk
|  |   └── DataCat.Adapter.PosgreSQL
|  |
|  ├── DataCat.UI
|  |
|  ├── DataCat.CLI
|  |
|  ├── DataCat.Clients
|  |   ├── DataCat.Client.Py
|  |   └── DataCat.Client.Csharp
|  |
|  ├── DataCat.Archiver
|  |
|  └── DataCat.Coordinator
|
├── docs
└── cicd
```
