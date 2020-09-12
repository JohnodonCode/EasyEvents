using System;
using System.ComponentModel;
using System.IO;
using Exiled.API.Interfaces;

namespace EasyEvents
{
    public class Config : IConfig
    {
        [Description("Enables/disables the plugin.")]
        public bool IsEnabled { get; set; } = true;
        
        [Description("Absolute path to the directory where custom event files are stored.")]
        public string EventFilesPath { get; set; } = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Configs"), "EasyEvent");
    }
}