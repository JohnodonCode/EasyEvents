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
        public string EventFilesPath { get; set; } = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Configs"), "EasyEvents");

        [Description("Should each event require a seperate permission (on top of \"easyevents.use\"). If this is enabled, you will need the permission \"easyevents.event.EVENT_NAME\" to run the event \"EVENT_NAME\".")]
        public bool PerEventPermissions { get; set; } = false;
    }
}