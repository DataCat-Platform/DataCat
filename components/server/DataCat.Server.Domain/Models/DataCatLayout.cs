namespace DataCat.Server.Domain.Models;

public class DataCatLayout
{
    private DataCatLayout(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public int X { get; private set; }

    public int Y { get; private set; }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public static Result<DataCatLayout> Create(int x, int y, int width, int height)
    {
        if (width <= 0)
        {
            return Result.Fail<DataCatLayout>("Width must be greater than 0");
        }

        if (height <= 0)
        {
            return Result.Fail<DataCatLayout>("Height must be greater than 0");
        }

        return Result.Success(new DataCatLayout(x, y, width, height));
    }
}
