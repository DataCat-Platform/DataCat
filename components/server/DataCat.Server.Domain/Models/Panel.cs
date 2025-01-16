namespace DataCat.Server.Domain.Models;

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
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result.Fail<Panel>("Title cannot be null or empty");
        }

        if (panelType is null)
        {
            return Result.Fail<Panel>("PanelType cannot be null");
        }

        if (query is null)
        {
            return Result.Fail<Panel>("Query cannot be null");
        }

        if (dataCatLayout is null)
        {
            return Result.Fail<Panel>("DataCatLayout cannot be null");
        }

        if (parentDashboard is null)
        {
            return Result.Fail<Panel>("ParentDashboard cannot be null");
        }

        return Result.Success(new Panel(id, title, panelType, query, dataCatLayout, parentDashboard));
    }
}
