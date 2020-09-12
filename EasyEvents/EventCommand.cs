using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using RemoteAdmin;
using UnityEngine;

namespace EasyEvents
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EventCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var permission = false;

            if (sender is PlayerCommandSender player)
            {

            }
            else
            {
                permission = true;
            }

            if (!permission)
            {
                response = "You do not have permission to run this command.";
                return true;
            }

            var args = arguments.Array;

            if (args == null || args.Length < 1)
            {
                response = "Usage: event <event name>";
                return true;
            }

            var command = String.Join(" ", args).Trim().ToLower();

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

            try
            {
                ScriptHandler.RunScript(text);
                response = "Event \"" + command + "\" started successfully";
                return true;
            }
            catch (Exception e)
            {
                response = e.ToString();
                return true;
            }
        }

        public string Command => "event";
        public string[] Aliases => new string[] {"runevent", "eventrun" };

    public string Description => "This is the command used to run custom events with EasyEvents.";
    }
}