namespace DataCat.Storage.Postgres.Mappers;

public class TagListTypeHandler : SqlMapper.TypeHandler<List<Tag>>
{
    public override void SetValue(IDbDataParameter parameter, List<Tag>? tags)
    {
        parameter.Value = tags?.Select(tag => tag.Value).ToArray() ?? [];
    }

    public override List<Tag> Parse(object value)
    {
        var strings = (string[])value;
        return strings.Select(s => new Tag(s)).ToList();
    }
}