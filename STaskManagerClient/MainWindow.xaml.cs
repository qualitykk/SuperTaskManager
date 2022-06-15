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
using System.Collections.ObjectModel;
using STaskManagerLibrary;

#nullable disable

namespace STaskManagerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<STask> Tasks { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();

            lbTasks.ItemsSource = Tasks;

            API.userId = 1; // TEMP
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Loading categories...");
            var cs = await API.GetCategoriesAsync();
            if (cs != null)
                foreach (var c in cs)
                    Categories.Add(c);

            Console.WriteLine("Loading tasks...");
            var ts = await API.GetTasksAsync();
            if(ts != null)
                foreach (var t in ts)
                    Tasks.Add(t);

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

            SolidColorBrush brush = new(s.TextColor);
            foreach (var label in spSettings.Children.OfType<Label>())
                label.Foreground = brush;
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
            SolidColorBrush labelBrush = new(currentSettings.TextColor);
            SolidColorBrush editorBrush = new(Colors.Black);
            foreach (var prop in currentSettings.GetType().GetProperties())
            {
                var get = prop.GetGetMethod();
                if (get == null || get.IsStatic)
                    continue;

                Label name = new() { Content = prop.Name };
                name.Foreground = labelBrush;

                TextBox editor = new();
                editor.Foreground = editorBrush;

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

        private async void btnReset_Click(object sender, RoutedEventArgs e)
        {
            currentSettings = new();
            UpdateFromSettings(currentSettings);
            await currentSettings.SaveAsync(SettingsFile);

            MessageBox.Show("Settings reset to default!");
        }
    }
}
