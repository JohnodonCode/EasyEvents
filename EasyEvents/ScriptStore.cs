using System;
using System.Collections.Generic;
using System.IO;
using Exiled.API.Features;

namespace EasyEvents
{
    public class ScriptStore
    {
        public static Dictionary<string, string> Scripts = new Dictionary<string, string>();

        public static void LoadScripts()
        {
            Scripts = new Dictionary<string, string>();
            
            var dir = EasyEvents.Singleton.Config.EventFilesPath;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                return;
            }

            var list = Directory.GetFiles(dir, "*.txt");
            
            foreach (var s in list)
            {
                var text = File.ReadAllText(s);
                var name = Path.GetFileNameWithoutExtension(s).Trim().ToLower();
                
                Scripts.Add(name, text);

                Log.Info("Loaded event \""+name+"\".");
            }
        }
    }
}