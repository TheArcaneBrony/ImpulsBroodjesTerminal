using System;
using System.DirectoryServices;
using System.IO;
using Microsoft.Win32;

namespace GipTerminalInstaller.Installers
{
    public class WindowsExplorer
    {
        public static void PreInit()
        {
        }
        public static void Clean()
        {
            UnpinAllFromTaskbar();
            SetPolicies();
            // SetupAccounts();
        }

        public static void UnpinAllFromTaskbar()
        {
            string PinnedPath =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + 
                @"\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar";
            //cleanup taskbar
            if(Directory.Exists(PinnedPath)) foreach (string file in Directory.GetFiles(
                PinnedPath))
                File.Delete(file);
        }

        public static void SetPolicies()
        { 
            Wallpaper.Set("https://wallup.net/wp-content/uploads/2018/09/26/214755-anime-Touhou-Inubashiri_Momiji-anime_girls.jpg", Wallpaper.Style.Stretched);
            //explorer policies (current user)
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies", true).CreateSubKey("Explorer");
            key.SetValue("NoDesktop", 1, RegistryValueKind.DWord);
            key.SetValue("NoUserNameInStartMenu", 1, RegistryValueKind.DWord);
            key.SetValue("NoTrayItemsDisplay", 1, RegistryValueKind.DWord);
            key.SetValue("NoTrayContextMenu", 1, RegistryValueKind.DWord);
            key.SetValue("NoTaskGrouping", 1, RegistryValueKind.DWord);
            key.SetValue("HideClock", 1, RegistryValueKind.DWord);
            key.SetValue("NoControlPanel", 1, RegistryValueKind.DWord);
            key.SetValue("NoStartMenuSubFolders", 1, RegistryValueKind.DWord);
            key.SetValue("NoStartMenuNetworkPlaces", 1, RegistryValueKind.DWord);
            key.SetValue("NoSMConfigurePrograms", 1, RegistryValueKind.DWord);
            key.SetValue("NoSetTaskbar", 1, RegistryValueKind.DWord);
            key.SetValue("NoSetFolders", 1, RegistryValueKind.DWord);
            key.SetValue("NoRun", 1, RegistryValueKind.DWord);
            key.SetValue("NoRecentDocsMenu", 1, RegistryValueKind.DWord);
            key.SetValue("NoStartMenuPinnedList", 1, RegistryValueKind.DWord);
            key.SetValue("NoNetworkConnections", 1, RegistryValueKind.DWord);
            key.SetValue("NoStartMenuMorePrograms", 1, RegistryValueKind.DWord);
            key.SetValue("NoSMHelp", 1, RegistryValueKind.DWord);
            key.SetValue("NoStartMenuMFUprogramsList", 1, RegistryValueKind.DWord);
            key.SetValue("NoFind", 1, RegistryValueKind.DWord);
            key.SetValue("NoFavoritesMenu", 1, RegistryValueKind.DWord);
            key.SetValue("NoCommonGroups", 1, RegistryValueKind.DWord);
            key.SetValue("NoClose", 1, RegistryValueKind.DWord);
            key.SetValue("NoChangeStartMenu", 1, RegistryValueKind.DWord);
            key.SetValue("NoAutoTrayNotify", 1, RegistryValueKind.DWord);
            key.SetValue("LockTaskbar", 1, RegistryValueKind.DWord);
            key.SetValue("ClearRecentDocsOnExit", 1, RegistryValueKind.DWord);
            key.SetValue("NoUserFolderInStartMenu", 1, RegistryValueKind.DWord);
            key.SetValue("NoSearchProgramsInStartMenu", 1, RegistryValueKind.DWord);
            key.SetValue("NoSearchFilesInStartMenu", 1, RegistryValueKind.DWord);
            key.SetValue("NoSearchInternetInStartMenu", 1, RegistryValueKind.DWord);
            key.SetValue("NoSearchComputerLinkInStartMenu", 1, RegistryValueKind.DWord);
            key.SetValue("NoControlPanel", 1, RegistryValueKind.DWord);
            key.SetValue("PreXPSP2ShellProtocolBehavior", 1, RegistryValueKind.DWord);
            key.SetValue("NoComputersNearMe", 1, RegistryValueKind.DWord);
            key.SetValue("NoWinKeys", 1, RegistryValueKind.DWord);
            key.SetValue("NoViewOnDrive", 1, RegistryValueKind.DWord);
            key.SetValue("NoViewContextMenu", 1, RegistryValueKind.DWord);
            key.SetValue("NoShellSearchButton", 1, RegistryValueKind.DWord);
            key.SetValue("ClassicShell", 1, RegistryValueKind.DWord);
            key.SetValue("TaskbarNoThumbnail", 1, RegistryValueKind.DWord);
            key.SetValue("TaskbarNoResize", 1, RegistryValueKind.DWord);
            key.SetValue("TaskbarNoRedock", 1, RegistryValueKind.DWord);
            key.SetValue("TaskbarNoNotification", 1, RegistryValueKind.DWord);
            key.SetValue("TaskbarLockAll", 1, RegistryValueKind.DWord);
            key.SetValue("AlwaysShowClassicMenu", 1, RegistryValueKind.DWord);
            key.SetValue("ForceActiveDesktopOn", 1, RegistryValueKind.DWord);
            key.SetValue("SettingsPageVisibility", 0, RegistryValueKind.DWord);
            key.SetValue("StartMenuLogOff", 0, RegistryValueKind.DWord);
            key.SetValue("QuickLaunchEnabled", 0, RegistryValueKind.DWord);
            key.SetValue("NoViewOnDrive", 4, RegistryValueKind.DWord);
            //System policies (current user)
            key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies", true).CreateSubKey("System");
            key.SetValue("NoDispCPL", 1, RegistryValueKind.DWord);
            key.SetValue("DisableTaskMgr", 1, RegistryValueKind.DWord);
            key.SetValue("DisableCMD", 1, RegistryValueKind.DWord);
            
            //System policies (local machine)
            // key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion").CreateSubKey("PersonalizationCSP");
            // key.SetValue("LockScreenImageUrl", "", RegistryValueKind.String);
            
        }

        public static void SetupAccounts()
        {
            string PasswdPrompt = "Enter a password for the administrator account: ";
            Console.Write(PasswdPrompt);
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            Util.GetProcessOutput($"net user {Environment.UserName} {pass}", true);
            CreateUser("Terminal");
        }
        public static void CreateUser(string Name, string Pass = "", string Description = "") {
            try  
            {  
                DirectoryEntry AD = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                DirectoryEntry NewUser = AD.Children.Add(Name, "user");
                NewUser.Invoke("SetPassword", new object[] { Pass });  
                NewUser.Invoke("Put", new object[] { "Description", "Test User from .NET" });  
                NewUser.CommitChanges();
                DirectoryEntry grp;
                //grp = AD.Children.Find("Administrators", "group");  
                //if (grp != null) { grp.Invoke("Add", new object[] { NewUser.Path.ToString() }); }  
                Util.Log($"User {Name} created successfully!", true);  
                //Console.WriteLine("Press Enter to continue....");  
                //Console.ReadLine();  
            }  
            catch (Exception ex)  
            {  
                Util.Log(ex.Message,true);  
                Console.ReadLine();
            }  
  
        }  
    }
}