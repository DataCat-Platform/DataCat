namespace DataCat.Server.Domain.Core.Enums;

public abstract class PanelType(string name, int value)
    : SmartEnum<PanelType, int>(name, value)
{
    public static readonly PanelType Graph = new GraphPanelType();
    public static readonly PanelType Table = new TablePanelType();
    public static readonly PanelType PieChart = new PieChartPanelType();

    private sealed class GraphPanelType() : PanelType("Graph", 1);

    private sealed class TablePanelType() : PanelType("Table", 2);

    private sealed class PieChartPanelType() : PanelType("Pie Chart", 3);
}