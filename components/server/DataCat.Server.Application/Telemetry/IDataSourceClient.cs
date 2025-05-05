namespace DataCat.Server.Application.Telemetry;

public interface IDataSourceClient : IDisposable
{
    string Name { get; }
}