using System.Reflection;

namespace SignalRGB.AzureFunction;
public class ProjectInfo
{
    public static readonly Assembly Assembly = typeof(ProjectInfo).Assembly;
    public static readonly Version AssemblyVersion = Assembly.GetName().Version ?? new Version();
    public static readonly string ApplicationName = Assembly.GetName().Name ?? string.Empty;
}