using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
using ThumbnailGenerator;

namespace UserManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Db db = new();
        private List<UserListItem> users = new();

        private void LoadUserList()
        {
            users = new();
            foreach (User dbUser in db.Users.OrderBy(x=>x.UserFirstname))
            {
                UserListItem user = new UserListItem(dbUser);
                users.Add(user);
            }
            FilterItems();
        }

        private void FilterItems(string filter = "")
        {
            UserList.Items.Clear();
            foreach (UserListItem uli in users.Where(x => x.DisplayName.ToLower().Contains(filter.ToLower())))
            {
                UserList.Items.Add(uli);
            }
        }
        
        
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadUserList();
        }

        private UserListItem EmptyUserListItem => new UserListItem(new User());
        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = (e.AddedItems.Count > 0 ? ((UserListItem) e.AddedItems[0]) : EmptyUserListItem)?.user;
            UserInfo.Content = new UserConfiguration(user);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
            LoadUserList();
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterItems(FilterBox.Text);
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            LoadUserList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            UserInfo.Content = new UserConfiguration(user);
            db.Users.Add(user);
            UserList.Items.Insert(0, new UserListItem(user));
            UserList.SelectedIndex = 0;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            db.Users.Remove(((UserListItem) UserList.SelectedItem).user);
            UserList.Items.Remove(UserList.SelectedItem);
        }

        private void AddUserCsvButton_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("temp.csv","voornaam,achternaam,email,wachtwoord,actief,administrator,kan bestellingen weergeven\n" +
                                                    "Test,Gebruiker,laatmestaan@campusimpuls.be,Wachtwoord8907,1,0,0");
            string executable = "notepad";
            if (File.Exists(@"C:\Program Files\Microsoft Office\root\Office16\EXCEL.EXE"))
                executable = @"C:\Program Files\Microsoft Office\root\Office16\EXCEL.EXE";
            else if (File.Exists(@"C:\Program Files (x86)\Microsoft Office\root\Office16\EXCEL.EXE"))
                executable = @"C:\Program Files (x86)\Microsoft Office\root\Office16\EXCEL.EXE";
            Process proc = Process.Start(executable,"temp.csv");
            proc.WaitForExit();
            bool ExcelExited = false;
            while (!ExcelExited)
            {
                try
                {
                    File.ReadAllLines("temp.csv");
                    ExcelExited = true;
                }
                catch{}
            }
            string[] lines = File.ReadAllLines("temp.csv");
            foreach (string line in lines.Skip(2))
            {
                string[] fields = line.Split(',');
                User user = new User()
                {
                    UserFirstname = fields[0], UserLastname = fields[1], UserEmail = fields[2],
                    UserPassword = BCrypt.Net.BCrypt.HashPassword(fields[3], 10),
                    UserEnabled = (sbyte?) (fields[4] == "1" ? 1 : 0), UserIsAdmin = (sbyte?) (fields[5] == "1" ? 1 : 0),
                    UserCanViewOrders = (sbyte?) (fields[6] == "1" ? 1 : 0)
                };
                db.Users.Add(user);
            }
            db.SaveChanges();
        }
    }
}