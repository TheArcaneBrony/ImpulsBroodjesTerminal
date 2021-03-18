using System.Windows;

namespace GipTerminal21
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static string[] args;
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            args = e.Args;
        }
    }
}