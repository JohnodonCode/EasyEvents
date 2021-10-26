using System;
using System.Collections.Generic;
using System.IO;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace EasyEvents
{
    public static class ScriptStore
    {
        public static Dictionary<string, string> Scripts = new Dictionary<string, string>();

        public static void LoadScripts()
        {
            Scripts = new Dictionary<string, string>();
            
            var dir = EasyEvents.Singleton.Config.EventFilesPath;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                
                var events = GetDefaultEvents();
                foreach (var key in events.Keys)
                {
                    File.WriteAllText(Path.Combine(dir, key+".txt"), events[key]);
                }
                return;
            }

            var list = Directory.GetFiles(dir, "*.txt");
            
            foreach (var s in list)
            {
                var text = File.ReadAllText(s);
                var name = Path.GetFileNameWithoutExtension(s).Trim().ToLower().Replace(" ", "");
                
                Scripts.Add(name, text);

                Log.Info("Loaded event \""+name+"\".");
            }
        }

        private static Dictionary<string, string> GetDefaultEvents()
        {
            var events = new Dictionary<string, string>();
            
            events.Add("hideandseek", "//Please note that order doesn't matter in events. You can reorder all of your commands and your event will not change.\n\n//Create the hider and seeker classes\ncreateclass seeker Scientist\ncreateclass hider ClassD\n\n//Make it so that 10% of players will be seekers, with a minimum of 2 seekers. The rest will be hiders.\nspawn g:seeker,10,2 g:hider\n\n//Everytime a player is killed, they will become a seeker.\ninfect g:hider g:seeker\n\n//Teleport seekers to the guard tower\nteleport g:seeker 55 1020 -45\n\n//Send broadcasts to hiders and seekers giving them information about their role.\nbroadcast g:seeker 5 \"You are a seeker. Kill all hiders. You will be released in 2 minutes.\"\nbroadcast g:hider 5 \"You are a hider. You have 2 minutes to hide until the seekers get released.\"\n\n//Give the seekers their items\ngive g:seeker GunLogicer\ngive g:seeker Radio\ngive g:seeker KeycardO5\ngive g:seeker Flashlight\n\n//Give the seekers enough hp that they cannot be killed\nhp g:seeker 10000\n\n//Turn off the lights forever after 2 minutes (120 seconds)\nlights ALL 100000 120\n\n//Teleport seekers to the LCZ_ARMORY after 2 minutes (120 seconds)\nteleport g:seeker LCZ_ARMORY 120\n\n//Send broadcasts telling everyone that the seekers have been released after 2 minutes (120 seconds)\nbroadcast all 5 \"The seekers have been released!\" 120\n\n//Make the last hider standing the winner\nlast g:hider");
            events.Add("civmtf", "//Please note that order doesn't matter in events. You can reorder all of your commands and your event will not change.\n\n//Create the CI and MTF classes\ncreateclass ci ChaosConscript\ncreateclass mtf NtfSergeant\n\n//Lock doors so the CI and MTF cant get into the facility or attack eachother\ndoor GateA lock\ndoor GateB lock\ndoor SurfaceGate close\ndoor SurfaceGate lock\n\n//Spawn the CI and MTF\nspawn g:ci,50,0 g:mtf,50,0\n\nbroadcast all 5 \"MTF vs Chaos: Get ready to fight!\"\n\n//Open the gate!\ndoor SurfaceGate open 15");
            return events;
        }
    }
}
