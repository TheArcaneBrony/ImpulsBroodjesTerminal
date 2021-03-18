using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GipTerminal21
{
    /// <summary>
    /// Interaction logic for BroodjeItem.xaml
    /// </summary>
    public partial class BroodjeItem : UserControl
    {
        public BroodjeItem()
        {
            InitializeComponent();
            DataContext = this;
        }
        public string BroodjeName { get; set; } = "Sample Text";
        public string BroodjePrijs { get; set; } = "420.69 EUR";
        public string ImageUrl { get; set; } = "https://icatcare.org/app/uploads/2018/07/Thinking-of-getting-a-cat.png";
        public BitmapImage BitmapSource
        {
            get => new(new Uri(ImageUrl));
        }
    }
}
