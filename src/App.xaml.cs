using System.IO;
using System.Windows;

namespace MarkDNext;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ShutdownMode = ShutdownMode.OnLastWindowClose;

        try
        {
            var files = e.Args
                .Select(TryResolveExistingFile)
                .Where(file => file is not null)
                .Cast<string>()
                .ToArray();

            if (files.Length == 0)
            {
                new MainWindow().Show();
                return;
            }

            foreach (var file in files)
            {
                new MainWindow(file).Show();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "MarkDNext could not start.\n\n" + ex.Message,
                "MarkDNext",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown(-1);
        }
    }

    private static string? TryResolveExistingFile(string arg)
    {
        if (string.IsNullOrWhiteSpace(arg))
        {
            return null;
        }

        try
        {
            var path = Path.GetFullPath(arg);
            return File.Exists(path) ? path : null;
        }
        catch
        {
            return null;
        }
    }
}
