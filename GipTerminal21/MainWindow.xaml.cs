using System;
using System.Windows;
using System.Windows.Controls;
using ThumbnailGenerator;

namespace GipTerminal21
{
    public partial class MainWindow
    {
        private static Db db= new Db();
        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            StackPanel stackPanel = new StackPanel(){Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center};
            foreach (Broodje broodje in db.Broodjes)
            {
                stackPanel.Children.Add(new BroodjeItem() { BroodjeName = broodje.BroodjeName, BroodjePrijs = broodje.BroodjePrice + " EUR", ImageUrl= Environment.CurrentDirectory +"\\"+ broodje.BroodjeId+"_128.png", Padding = new Thickness(5)});
                if (stackPanel.Children.Count == 3)
                {
                    ContentSP.Children.Add(stackPanel);
                    stackPanel = new StackPanel(){Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center};
                }
            }
            if(stackPanel.Children.Count != 3)
                ContentSP.Children.Add(stackPanel);
            StateChanged += (_, _)=>{ WindowState = WindowState.Maximized; };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(EnvOpts.IsRelease) e.Cancel = true;
        }
    }
}