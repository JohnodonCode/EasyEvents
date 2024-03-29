﻿using System;
using System.Collections.Generic;
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
        public string EventFilesPath { get; set; } = Path.Combine(Exiled.API.Features.Paths.Configs, "EasyEvents");

        [Description("Should each event require a seperate permission (on top of \"easyevents.use\"). If this is enabled, you will need the permission \"easyevents.event.EVENT_NAME\" to run the event \"EVENT_NAME\".")]
        public bool PerEventPermissions { get; set; } = false;
        
        [Description("A list of events that can run every round. Useful for event servers. \"None\" can be used to modify probability.")]
        public List<string> Events { get; set; } = new List<string>();

        [Description("Send debug messages (will spam console but useful for debugging errors).")]
        public bool Debug { get; set; } = false;
    }
}