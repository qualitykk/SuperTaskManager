using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Security.Cryptography;

namespace STaskManagerClient
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private static SHA512 hash = SHA512.Create();
        public Login()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string user = tbUsername.Text;
            string password = tbPassword.Password;
            if(string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("No username and password specified!");
                return;
            }

            // var result = true;
            var result = await API.LoginAsync(user, Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(password))));
            if(!result)
            {
                MessageBox.Show("Incorrect password or username!");
            }
            else
            {
                MainWindow window = new();
                window.Show();
                Application.Current.MainWindow = window;
                Close();
            }
            return;
        }
    }
}
