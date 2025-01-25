namespace DataCat.Server.Domain.Core;

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
        var validationList = new List<Result<DataCatLayout>>();

        #region Validation

        if (width <= 0)
        {
            validationList.Add(Result.Fail<DataCatLayout>(AlertError.NegativeWidth));
        }

        if (height <= 0)
        {
            validationList.Add(Result.Fail<DataCatLayout>(AlertError.NegativeHeight));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new DataCatLayout(x, y, width, height));
    }
}
