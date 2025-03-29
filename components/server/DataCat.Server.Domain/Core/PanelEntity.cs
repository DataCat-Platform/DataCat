namespace DataCat.Server.Domain.Core;

public sealed record PanelEntity
{
    private PanelEntity(
        Guid id,
        string title,
        PanelType type,
        QueryEntity queryEntity,
        DataCatLayout dataCatLayout,
        Guid dashboardId)
    {
        Id = id;
        Title = title;
        Type = type;
        QueryEntity = queryEntity;
        DataCatLayout = dataCatLayout;
        DashboardId = dashboardId;
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public PanelType Type { get; private set; }

    public QueryEntity QueryEntity { get; private set; }

    public DataCatLayout DataCatLayout { get; private set; }
    
    public Guid DashboardId { get; private set; }

    public void UpdateTitle(string title) => Title = title;
    
    public void UpdatePanelType(PanelType panelType, QueryEntity queryEntity)
    {
        Type = panelType;
        QueryEntity = queryEntity;
    }

    public void UpdateLayout(DataCatLayout dataCatLayout)
    {
        DataCatLayout = dataCatLayout;
    }
    
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
