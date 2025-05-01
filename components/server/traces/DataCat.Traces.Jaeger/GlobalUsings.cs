// Global using directives

global using System.Data;
global using System.Net.Http.Headers;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using DataCat.Server.Application.Persistence;
global using DataCat.Server.Application.Persistence.Repositories;
global using DataCat.Server.Application.Telemetry;
global using DataCat.Server.Application.Telemetry.Traces.Abstractions;
global using DataCat.Server.Application.Telemetry.Traces.Models;
global using DataCat.Server.Domain.Core;
global using DataCat.Server.Domain.Core.Enums;
global using DataCat.Traces.Jaeger.Constants;
global using DataCat.Traces.Jaeger.Core;
global using DataCat.Traces.Jaeger.Models;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;