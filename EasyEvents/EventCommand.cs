using System;
using System.Linq;
using CommandSystem;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using Exiled.Loader;

namespace EasyEvents
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EventCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var permission = false;
            var perPermission = false;

            if (arguments.Array == null || arguments.Array.Length < 2)
            {
                response = "Usage: event <event name>";
                return true;
            }

            var command = arguments.Array[1].Trim().ToLower().Replace(" ", "");

            if (sender is PlayerCommandSender player)
            {
                permission = player.CheckPermission("easyevents.use");
                perPermission = player.CheckPermission("easyevents.event." + command);
            }
            else
            {
                permission = true;
                perPermission = true;
            }

            if (EasyEvents.Singleton.Config.PerEventPermissions && !perPermission)
            {
                response = "You do not have permission to run this event.";
                return true;
            }

            if (!permission)
            {
                response = "You do not have permission to run this command.";
                return true;
            }

            if (!ScriptStore.Scripts.ContainsKey(command))
            {
                response = "Event \"" + command + "\" does not exist!";
                return true;
            }

            if (!ScriptStore.Scripts.TryGetValue(command, out var text))
            {
                response = "Event \"" + command + "\" does not exist!";
                return true;
            }

            if (Exiled.API.Features.Round.IsStarted)
            {
                response = "Events can only be ran before the round is started.";
                return true;
            }

            if (ScriptActions.scriptData.eventRan)
            {
                response = "Only one event can be ran per round. Restart the round to run this event.";
                return true;
            }

            try
            {
                ScriptHandler.RunScript(text);
                response = "Event \"" + command + "\" started successfully";
                ScriptActions.scriptData.eventRan = true;
                Loader.Plugins.FirstOrDefault(pl => pl.Name == "ScpStats")?.Assembly?.GetType("SCPStats.EventHandler")?.GetField("PauseRound")?.SetValue(null, true);
                return true;
            }
            catch (Exception e)
            {
                response = e.ToString();
                return true;
            }
        }

        public string Command => "event";
        public string[] Aliases => new string[] {"runevent", "eventrun", "events" };
        public string Description => "This is the command used to run custom events with EasyEvents.";
    }
}
