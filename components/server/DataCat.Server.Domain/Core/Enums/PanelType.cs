namespace DataCat.Server.Domain.Core.Enums;

public abstract class PanelType(string name, int value)
    : SmartEnum<PanelType, int>(name, value)
{
    public static readonly PanelType LineChart = new LineChartPanelType();
    public static readonly PanelType PieChart = new PieChartPanelType();
    public static readonly PanelType BarChart = new BarChartPanelType();

    private sealed class LineChartPanelType() : PanelType("LineChart", 1);

    private sealed class PieChartPanelType() : PanelType("PieChart", 2);
    
    private sealed class BarChartPanelType() : PanelType("BarChart", 3);
}