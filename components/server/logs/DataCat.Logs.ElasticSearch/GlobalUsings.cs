// Global using directives

global using System.ComponentModel.DataAnnotations;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using DataCat.Logs.ElasticSearch.Constants;
global using DataCat.Logs.ElasticSearch.Models;
global using DataCat.Server.Application.Logs;
global using DataCat.Server.Application.Logs.Abstractions;
global using DataCat.Server.Application.Logs.Models;
global using DataCat.Server.Application.Logs.Queries.Search;
global using DataCat.Server.Application.Queries.Common;
global using Elastic.Clients.Elasticsearch;
global using Elastic.Clients.Elasticsearch.Aggregations;
global using Elastic.Clients.Elasticsearch.QueryDsl;
global using Elastic.Transport;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;