namespace DataCat.Server.Domain.Core;

public sealed record Panel
{
    private Panel(
        Guid id,
        string title,
        PanelType type,
        Query query,
        DataCatLayout dataCatLayout,
        Guid dashboardId)
    {
        Id = id;
        Title = title;
        Type = type;
        Query = query;
        DataCatLayout = dataCatLayout;
        DashboardId = dashboardId;
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public PanelType Type { get; private set; }

    public Query Query { get; private set; }

    public DataCatLayout DataCatLayout { get; private set; }
    
    public Guid DashboardId { get; private set; }

    public void UpdateTitle(string title) => Title = title;
    
    public void UpdatePanelType(PanelType panelType, Query query)
    {
        Type = panelType;
        Query = query;
    }

    public void UpdateLayout(DataCatLayout dataCatLayout)
    {
        DataCatLayout = dataCatLayout;
    }
    
    public static Result<Panel> Create(
        Guid id,
        string title,
        PanelType? panelType,
        Query? query,
        DataCatLayout? dataCatLayout,
        Guid parentDashboardId)
    {
        var validationList = new List<Result<Panel>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(title))
        {
            validationList.Add(Result.Fail<Panel>(BaseError.FieldIsNull(nameof(title))));
        }

        if (panelType is null)
        {
            validationList.Add(Result.Fail<Panel>(BaseError.FieldIsNull(nameof(panelType))));
        }

        if (query is null)
        {
            validationList.Add(Result.Fail<Panel>(BaseError.FieldIsNull(nameof(query))));
        }

        if (dataCatLayout is null)
        {
            validationList.Add(Result.Fail<Panel>(BaseError.FieldIsNull(nameof(dataCatLayout))));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new Panel(id, title, panelType!, query!, dataCatLayout!, parentDashboardId));
    }
}
