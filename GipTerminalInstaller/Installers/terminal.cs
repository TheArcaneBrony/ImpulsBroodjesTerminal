using Microsoft.Win32;

namespace GipTerminalInstaller.Installers
{
    public class terminal
    {
        public static void install()
        {
            //Registry: adapted from https://stackoverflow.com/a/675347/9453469
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.SetValue("GIPTerminal", Util.CurrentFile);
        }
    }
}