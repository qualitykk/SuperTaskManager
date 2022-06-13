using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media;

#nullable disable

namespace STaskManagerClient
{
    public sealed class Settings
    {
        public static Settings Instance { get; set; }
        public Color BackgroundColor { get; set; } = Colors.White;
        public Color TextColor { get; set; } = Colors.Black;
        public bool SaveLoginInfo { get; set; } = true;
        public Settings()
        {
            if (Instance == null)
                Instance = this;
        }

        public static async Task<Settings> ReadAsync(string path)
        {
            if (!File.Exists(path))
                return new();

            var text = await File.ReadAllTextAsync(path);
            var settings = JsonSerializer.Deserialize<Settings>(text);
            return settings;
        }

        public async Task SaveAsync(string path)
        {
            string json = JsonSerializer.Serialize(this);
            await File.WriteAllTextAsync(path, json);
        }
    }
}
