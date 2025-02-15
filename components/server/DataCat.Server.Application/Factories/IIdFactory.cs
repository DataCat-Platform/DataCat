namespace DataCat.Server.Application.Factories;

public interface IIdFactory<out TId>
{
    TId NewId();
}