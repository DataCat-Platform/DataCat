namespace DataCat.Server.Domain.Common.ResultFlow;

public static class ResultExtensions
{
    public static Result<T>? FoldResults<T>(this IEnumerable<Result<T>> results)
    {
        Result<T>? aggregatedValue = null;
        foreach (var result in results)
        {
            if (result.IsSuccess)
            {
                continue;
            }
            
            var error = result.Errors!.First();

            if (aggregatedValue == null)
            {
                aggregatedValue ??= new Result<T>(error);
                continue;
            }

            aggregatedValue.Append(error.Exception, error.ErrorMessage);
        }

        return aggregatedValue;
    }
}