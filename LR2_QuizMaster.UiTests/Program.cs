internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        string output = args.FirstOrDefault(a => !a.StartsWith("--")) ??
            Path.Combine(Path.GetTempPath(), "QuizMasterUiTests", DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff"));
        Directory.CreateDirectory(output);
        Console.WriteLine("Test artifacts: " + output);
        if (args.Contains("--data-audit")) return BackendAudit.Run(output).GetAwaiter().GetResult();
        string[] options = [output, .. args.Where(a => a.StartsWith("--"))];
        if (args.Contains("--menu")) return MenuChecks.Run(options);
        return args.Contains("--layout") ? LayoutChecks.Run(options) : GameChecks.Run(options);
    }
}
