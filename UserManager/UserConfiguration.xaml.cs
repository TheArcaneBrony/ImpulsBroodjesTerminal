using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
using ThumbnailGenerator;

namespace UserManager
{
    /// <summary>
    /// Interaction logic for UserConfiguration.xaml
    /// </summary>
    public partial class UserConfiguration : UserControl
    {
        public UserConfiguration(User dataUser)
        {
            InitializeComponent();
            user = dataUser;
            DataContext = this;
            Console.WriteLine(JsonSerializer.Serialize(user));
        }

        public User user { get; set; }
    }
}
