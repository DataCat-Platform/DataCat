// Global using directives

global using System.Collections.Concurrent;
global using System.Diagnostics;
global using DataCat.Secrets.Vault.Core;
global using DataCat.Secrets.Vault.Telemetry;
global using DataCat.Server.Application.Security;
global using DataCat.Server.PDK;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using OpenTelemetry.Resources;
global using VaultSharp;
global using VaultSharp.V1.AuthMethods;
global using VaultSharp.V1.AuthMethods.Token;
global using VaultSharp.V1.AuthMethods.UserPass;