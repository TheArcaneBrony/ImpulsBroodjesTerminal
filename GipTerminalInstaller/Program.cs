using System;
using System.IO;
using System.Net;

namespace GipTerminalInstaller
{
    internal class Program
    {
        private static WebClient wc = new WebClient();
        public static void Main(string[] args)
        {
            Util.CheckAdmin(args);
            if (Environment.UserName != "Terminal")
            {
                Util.Log("Installation step 2: create Terminal account");
                Installers.WindowsExplorer.SetupAccounts();

            }
            else
            {
                Util.Log("Installation step 2: configure Terminal account");
            }
            
            Util.Log("Configuring windows...");
            Installers.WindowsExplorer.Clean();
            
            Console.Write("\nInstallation finished, press any key to exit...");
            Console.ReadKey(true);
        }

        
    }
}