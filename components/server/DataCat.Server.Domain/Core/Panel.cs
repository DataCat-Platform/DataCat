namespace DataCat.Server.Domain.Core;

public class Panel
{
    private Panel(
        Guid id,
        string title,
        PanelType panelType,
        Query query,
        DataCatLayout dataCatLayout,
        Dashboard parentDashboard)
    {
        Id = id;
        Title = title;
        PanelType = panelType;
        Query = query;
        DataCatLayout = dataCatLayout;
        ParentDashboard = parentDashboard;
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public PanelType PanelType { get; private set; }

    public Query Query { get; private set; }

    public DataCatLayout DataCatLayout { get; private set; }

    public Dashboard ParentDashboard { get; private set; }

    public static Result<Panel> Create(
        Guid id,
        string title,
        PanelType? panelType,
        Query? query,
        DataCatLayout? dataCatLayout,
        Dashboard? parentDashboard)
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

        if (parentDashboard is null)
        {
            validationList.Add(Result.Fail<Panel>(BaseError.FieldIsNull(nameof(parentDashboard))));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new Panel(id, title, panelType!, query!, dataCatLayout!, parentDashboard!));
    }
}
