using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace WriteDry.Utils
{
    public static class WindowsShell
    {
        public static void OpenFileInExplorer(string filename, bool fromCurrentLocation = false)
            => Process.Start(new ProcessStartInfo {
                FileName = "explorer.exe",
                Arguments = $"{(fromCurrentLocation ? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) : "")}\\{filename}"
            });
    }
}
