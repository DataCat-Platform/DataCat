namespace DataCat.Server.Domain.Core;

public class PanelEntity
{
    private PanelEntity(
        Guid id,
        string title,
        PanelType panelType,
        QueryEntity queryEntity,
        DataCatLayout dataCatLayout,
        Guid parentDashboardId)
    {
        Id = id;
        Title = title;
        PanelType = panelType;
        QueryEntity = queryEntity;
        DataCatLayout = dataCatLayout;
        ParentDashboardId = parentDashboardId;
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public PanelType PanelType { get; private set; }

    public QueryEntity QueryEntity { get; private set; }

    public DataCatLayout DataCatLayout { get; private set; }
    public Guid ParentDashboardId { get; private set; }

    public static Result<PanelEntity> Create(
        Guid id,
        string title,
        PanelType? panelType,
        QueryEntity? query,
        DataCatLayout? dataCatLayout,
        Guid parentDashboardId)
    {
        var validationList = new List<Result<PanelEntity>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(title))
        {
            validationList.Add(Result.Fail<PanelEntity>(BaseError.FieldIsNull(nameof(title))));
        }

        if (panelType is null)
        {
            validationList.Add(Result.Fail<PanelEntity>(BaseError.FieldIsNull(nameof(panelType))));
        }

        if (query is null)
        {
            validationList.Add(Result.Fail<PanelEntity>(BaseError.FieldIsNull(nameof(query))));
        }

        if (dataCatLayout is null)
        {
            validationList.Add(Result.Fail<PanelEntity>(BaseError.FieldIsNull(nameof(dataCatLayout))));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new PanelEntity(id, title, panelType!, query!, dataCatLayout!, parentDashboardId));
    }
}
