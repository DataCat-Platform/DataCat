// Global using directives

global using System.Reflection;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using DataCat.Server.Application.Assembly;
global using DataCat.Server.Application.Behaviors;
global using DataCat.Server.Application.Behaviors.TransactionScope;
global using DataCat.Server.Application.Behaviors.Validation;
global using DataCat.Server.Application.Commands.Plugin.Add;
global using DataCat.Server.Application.Utils;
global using DataCat.Server.Domain.Plugins.Repos;
global using DataCat.Server.Infrastructure.Migrations;
global using DataCat.Server.Infrastructure.Options;
global using DataCat.Server.Infrastructure.Repositories;
global using DataCat.Server.PDK;
global using DataCat.Server.Postgres;
global using DataCat.Server.Postgres.Runners;
global using FluentValidation;
global using MediatR;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;
global using Serilog;
global using Serilog.Events;
