using System;
using System.Collections.Generic;
using System.Linq;
using EasyEvents.Types;

namespace EasyEvents.Commands
{
    public static class Teleport
    {
        public static void Run(List<string> args, int i)
        {
            if(args.Count < 1) throw new InvalidArgumentLengthException("Expected 1 argument but got 0 for command \"teleport\" at line "+i+".");

            var teleports = new List<TeleportData>();
            
            for (var y = 0; y < args.Count; y++)
            {
                var argEls = args[y].Split(',');
                
                if(argEls.Length < 2) throw new InvalidArgumentException("Invalid argument for command \"teleport\" on line "+i+", argument "+y+". Expected \"(0-17),DOOR_NAME\" but got \""+args[y]+"\".");

                var roleInfo = RoleInfo.parseRole(argEls[0], "teleport", i, y);
                var classId = roleInfo.classId;
                CustomRole customRole = roleInfo.role;

                var door = UnityEngine.Object.FindObjectsOfType<Door>().FirstOrDefault(_door => _door.DoorName.Trim().ToUpper() == argEls[1].Trim().ToUpper());
                
                if(door == null) throw new InvalidArgumentException("Invalid argument for command \"teleport\" on line "+i+", argument "+y+". The door name specified is not valid.");
                
                teleports.Add(new TeleportData(classId, door, customRole));
            }
            
            ScriptActions.SetTeleport(teleports, i);
        }
    }
}