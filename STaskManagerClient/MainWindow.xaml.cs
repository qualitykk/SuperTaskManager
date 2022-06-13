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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

#nullable disable

namespace STaskManagerClient
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadSettingsAsync();
            UpdateFromSettings(Settings.Instance);
            Console.WriteLine("Finished Loading Window!");
        }

        private const string SettingsFile = "settings.json";
        public static async Task LoadSettingsAsync()
        {
            Console.WriteLine("Loading settings...");
            await Settings.ReadAsync(SettingsFile);
        }
        public void UpdateFromSettings(Settings s)
        {
            SolidColorBrush bg = new(s.BackgroundColor);
            Background = bg;
            tcMain.Background = bg;
        }

        #region Tab Content Creation
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.Source is TabControl tc && tc.SelectedItem == tbiSettings)
            {
                BuildSettings();
            }
        }
        Settings currentSettings;
        private void BuildSettings()
        {
            // https://stackoverflow.com/questions/28159636/binding-a-textbox-to-a-property-of-a-class

            if (currentSettings != null)
                return;

            currentSettings = Settings.Instance != null ? Settings.Instance : new();
            foreach(var prop in currentSettings.GetType().GetProperties())
            {
                var get = prop.GetGetMethod();
                if (get == null || get.IsStatic)
                    continue;

                Label name = new() { Content = prop.Name };
                name.Foreground = new SolidColorBrush(Colors.Black);

                TextBox editor = new();
                editor.Foreground = new SolidColorBrush(Colors.Black);

                Binding binding = new(prop.Name);
                binding.Source = currentSettings;
                editor.SetBinding(TextBox.TextProperty, binding);

                StackPanel group = new();
                group.Children.Add(name);
                group.Children.Add(editor);
                spSettings.Children.Add(group);
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            UpdateFromSettings(currentSettings);
            await currentSettings.SaveAsync(SettingsFile);

            MessageBox.Show("Settings Applied and Saved!");
        }
        #endregion
    }
}
