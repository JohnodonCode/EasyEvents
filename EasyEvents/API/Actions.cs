using EasyEvents.Handlers;
using EasyEvents.Types;
using Exiled.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEvents.API
{
    public static class Actions
    {
        public static string startEvent(string command)
        {
            string response;
            if (!ScriptStore.Scripts.ContainsKey(command))
            {
                response = "Event \"" + command + "\" does not exist!";
                return response;
            }

            if (!ScriptStore.Scripts.TryGetValue(command, out var text))
            {
                response = "Event \"" + command + "\" does not exist!";
                return response;
            }

            if (Exiled.API.Features.Round.IsStarted)
            {
                response = "Events can only be ran before the round is started.";
                return response;
            }

            if (ScriptActions.scriptData.eventRan)
            {
                response = "Only one event can be ran per round. Restart the round to run this event.";
                return response;
            }

            try
            {
                ScriptHandler.RunScript(text);
                EventData eventData = new EventData(text, command);
                var _ev = new API.EventArgs.StartingEventEventArgs(null, eventData);

                Events.OnStartingEvent(_ev);

                if (_ev.IsAllowed)
                {
                    ScriptActions.scriptData.eventRan = true;
                    Loader.Plugins.FirstOrDefault(pl => pl.Name == "SCPStats")?.Assembly?.GetType("SCPStats.EventHandler")?.GetField("PauseRound")?.SetValue(null, true);
                    response = "Event \"" + command + "\" started successfully";
                    return response;
                }
                else
                {
                    response = "Event was not allowed.";
                    return response;
                }

            }
            catch (Exception e)
            {
                response = e.ToString();
                return response;
            }
        }
    }
}
