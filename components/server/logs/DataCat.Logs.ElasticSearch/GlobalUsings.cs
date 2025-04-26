// Global using directives

global using System.ComponentModel.DataAnnotations;
global using System.Data;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using DataCat.Logs.ElasticSearch.Constants;
global using DataCat.Logs.ElasticSearch.Models;
global using DataCat.Logs.ElasticSearch.Searching;
global using DataCat.Server.Application.Persistence;
global using DataCat.Server.Application.Persistence.Repositories;
global using DataCat.Server.Application.Queries.Common;
global using DataCat.Server.Application.Telemetry;
global using DataCat.Server.Application.Telemetry.Logs.Abstractions;
global using DataCat.Server.Application.Telemetry.Logs.Models;
global using DataCat.Server.Application.Telemetry.Logs.Queries.Search;
global using DataCat.Server.Domain.Core.Enums;
global using Elastic.Clients.Elasticsearch;
global using Elastic.Clients.Elasticsearch.Aggregations;
global using Elastic.Clients.Elasticsearch.QueryDsl;
global using Elastic.Transport;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;