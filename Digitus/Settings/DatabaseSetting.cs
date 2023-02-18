namespace Digitus.Settings;

public class DatabaseSetting : IDatabaseSetting
{
    public string? LoginCollectionName { get; set; }
    public string? SignupCollectionName { get; set; }
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
}