using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ThumbnailGenerator;

namespace GipTerminal21
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            if (EnvOpts.IsRelease) Process.Start("taskkill", "/f /im explorer*");
            DataContext = this;
        }

        private double ItemCount = (double) Program.db.Broodjes.Count();
        public double Progress = 0;

        public string ProgressText = "";
        private bool Finished = false;

        private int i = 0;
        private static Db db = new Db();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressBar.Maximum = 100;
            //Threads: https://stackoverflow.com/a/33412865/9453469
            //Update UI from other thread: https://stackoverflow.com/a/41193406/9453469
            //Update UI
            new Thread(() =>
            {
                //while starting, update screen every 100ms
                while (!Finished)
                {
                    Debug.WriteLine("a");
                    Dispatcher.Invoke(() =>
                    {
                        ProgressBar.Value = Progress;
                        ProgressTextLabel.Content = ProgressText;
                    });
                    Thread.Sleep(100);
                }
                //startup finished, start terminal and exit this thread
                Dispatcher.Invoke(()=>
                {
                    MainWindow mw = new();
                    mw.Closed += (_, _) =>
                    {
                        Close();
                    }; 
                    mw.Show();
                });
            }).Start();
            //handle startup
            new Thread(() =>
            {
                //check if installing
                if (App.args.Contains("--install"))
                {
                    ProgressText = "Installing...";
                }
                double lastProgress = 0;
                Progress = 0;
                //generate thumbnails
                foreach (Broodje broodje in db.Broodjes)
                {
                    Program.ProcessImage(broodje.BroodjeId, broodje.BroodjeImage);
                    i++;
                    if (Math.Abs(lastProgress - Progress) > 0.25)
                    {
                        lastProgress = Progress;
                        Progress = i / ItemCount * 100;
                        ProgressText = $"Progress: {i}/{ItemCount} thumbnails processed ({Math.Round(Progress)} %)";
                    }
                }
                Finished = true;
            }).Start();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if(EnvOpts.IsRelease) Process.Start("explorer");
        }
    }
}
