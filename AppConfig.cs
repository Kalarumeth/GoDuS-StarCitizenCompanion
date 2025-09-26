using System.IO;
using System.Text.Json;

public class AppConfig
{
    public string LogFilePath { get; set; } = "";
    public int OverlayDuration { get; set; } = 3; // default 3 secondi
}

public static class ConfigManager
{
    private static string configFile = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "StarCitizenCompanion",
        "config.json");

    public static AppConfig Load()
    {
        if (!File.Exists(configFile)) return new AppConfig();
        var json = File.ReadAllText(configFile);
        return JsonSerializer.Deserialize<AppConfig>(json)!;
    }

    public static void Save(AppConfig config)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(configFile)!);
        File.WriteAllText(configFile, JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));
    }
}