using System.IO;

public static class Logger
{
    private static readonly string logFile = "ApiTestLog.txt";

    public static void Log(string message)
    {
        var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
        Console.WriteLine(logMessage);
        File.AppendAllText(logFile, logMessage + Environment.NewLine);
    }

    public static void Clear()
    {
        if (File.Exists(logFile)) File.Delete(logFile);
    }
}
