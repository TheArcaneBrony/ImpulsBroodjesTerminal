using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace GipTerminalInstaller
{
    internal class Program
    {
        private static WebClient wc = new WebClient();
        
        public static Assembly EA = Assembly.GetExecutingAssembly();
        public static void Main(string[] args)
        {
            Util.CheckAdmin(args);
            if (Environment.UserName != "Terminal")
            {
                File.Copy(EA.Location, "\\tmp\\"+new FileInfo(EA.Location).Name);
                Util.Log("Installation step 1: create Terminal account", true);
                Installers.WindowsExplorer.SetupAccounts();
                Util.GetProcessOutput(@"cmd /c mkdir C:\Users\Terminal\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\");
                File.Copy(EA.Location, @"C:\Users\Terminal\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\"+new FileInfo(EA.Location).Name);

            }
            else
            {
                Util.Log("Installation step 2: configure Terminal account", true);
                Util.Log("Configuring windows...");
                Installers.WindowsExplorer.Clean();
            }
            
            
            Console.Write("\nInstallation finished, press any key to exit...");
            Console.ReadKey(true);
        }

        
    }
}