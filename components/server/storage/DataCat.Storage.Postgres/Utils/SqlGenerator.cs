namespace DataCat.Storage.Postgres.Utils;

internal static class SqlGenerator
{
    public static StringBuilder BuildQuery(
        this StringBuilder sqlBuilder,
        DynamicParameters parameters,
        SearchFilters? filters,
        Dictionary<string, string> columnMappings)
    {
        if (filters is not null && filters.Filters.Count > 0)
        {
            var conditions = new List<string>();

            foreach (var filter in filters.Filters)
            {
                if (!columnMappings.TryGetValue(filter.Key, out var sqlField))
                {
                    throw new ArgumentException($"Column mapping does not exist for {filter.Key}");
                }

                var paramName = $"@{filter.Key}";

                switch (filter.MatchMode)
                {
                    case MatchMode.Equals:
                        AddEqualsCondition(filter, parameters, conditions, sqlField, paramName);
                        break;

                    case MatchMode.StartsWith:
                        if (filter.FieldType != SearchFieldType.String)
                        {
                            throw new InvalidOperationException("StartsWith only supports string fields.");
                        }

                        conditions.Add($"{sqlField} LIKE {paramName}");
                        parameters.Add(paramName, $"{filter.Value}%");
                        break;

                    case MatchMode.Contains:
                        AddContainsCondition(filter, parameters, conditions, sqlField, paramName);
                        break;

                    default:
                        throw new NotSupportedException($"MatchMode {filter.MatchMode} is not supported.");
                }
            }

            if (conditions.Count > 0)
            {
                sqlBuilder.Append(" AND (");
                sqlBuilder.Append(string.Join(" AND ", conditions));
                sqlBuilder.Append(')');
            }
        }

        sqlBuilder.Append('\n');
        return sqlBuilder;
    }

    public static StringBuilder ApplyOrderBy(
        this StringBuilder sqlBuilder,
        Sort? sort,
        Dictionary<string, string> columnMappings)
    {
        if (sort is null)
        {
            return sqlBuilder;
        }
        
        if (!columnMappings.TryGetValue(sort.FieldName, out var sqlField))
        {
            throw new ArgumentException($"Column mapping does not exist for {sort.FieldName}");
        }

        var direction = sort.SortOrder == SortOrder.Asc ? "ASC" : "DESC";
        sqlBuilder.Append($" ORDER BY {sqlField} {direction} \n");

        return sqlBuilder;
    }

    public static StringBuilder ApplyPagination(this StringBuilder sqlBuilder)
    {
        sqlBuilder.Append(" OFFSET @offset \n");
        sqlBuilder.Append(" LIMIT @limit \n");
        return sqlBuilder;
    }

    private static void AddEqualsCondition(
        SearchFilter filter,
        DynamicParameters parameters,
        List<string> conditions,
        string sqlField,
        string paramName)
    {
        switch (filter.FieldType)
        {
            case SearchFieldType.String:
                conditions.Add($"{sqlField} = {paramName}");
                parameters.Add(paramName, filter.Value.ToString());
                break;

            case SearchFieldType.Number:
                var value = double.Parse(filter.Value.ToString()!);
                conditions.Add($"{sqlField} = {paramName}");
                parameters.Add(paramName, value);
                break;

            case SearchFieldType.Boolean:
                conditions.Add($"{sqlField} = {paramName}");
                parameters.Add(paramName, Convert.ToBoolean(filter.Value));
                break;

            case SearchFieldType.Date:
                conditions.Add($"{sqlField} = {paramName}");
                parameters.Add(paramName, Convert.ToDateTime(filter.Value));
                break;

            case SearchFieldType.Array:
                throw new NotImplementedException("Array filter is not implemented for Equals");
        }
    }
    
    private static void AddContainsCondition(
        SearchFilter filter, 
        DynamicParameters parameters, 
        List<string> conditions,
        string sqlField, 
        string paramName)
    {
        switch (filter.FieldType)
        {
            case SearchFieldType.String:
                conditions.Add($"{sqlField} ILIKE {paramName}");
                parameters.Add(paramName, $"%{filter.Value}%");
                break;

            case SearchFieldType.Array:
                var arrayParamName = $"{filter.Key}_contains_array";

                if (filter.Value is not IEnumerable<object> containsValues)
                {
                    throw new InvalidOperationException("Array field must be IEnumerable for Contains");
                }

                // PostgreSQL array overlap: column && ARRAY[...]
                conditions.Add($"{sqlField} && @{arrayParamName}");
                parameters.Add($"@{arrayParamName}", containsValues.ToArray());
                break;

            default:
                throw new InvalidOperationException($"Contains is only supported for string and array fields. Got: {filter.FieldType}");
        }
    }
}