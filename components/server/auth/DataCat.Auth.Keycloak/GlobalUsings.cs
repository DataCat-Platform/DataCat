// Global using directives

global using System.IdentityModel.Tokens.Jwt;
global using System.Net.Http.Json;
global using System.Security.Claims;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using DataCat.Auth.Keycloak.Models;
global using DataCat.Auth.Keycloak.Services;
global using DataCat.Server.Application.Auth;
global using DataCat.Server.Application.Scheduling;
global using DataCat.Server.Domain.Common.ResultFlow;
global using DataCat.Server.Domain.Identity;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Quartz;
global using AuthenticationOptions = DataCat.Auth.Keycloak.Models.AuthenticationOptions;