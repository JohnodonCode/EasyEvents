using System;
using System.Collections.Generic;
using System.Linq;
using EasyEvents.Types;
using Interactables.Interobjects.DoorUtils;
using UnityEngine;

namespace EasyEvents.Commands
{
    public static class Teleport
    {
        public static void Run(List<string> args, int i)
        {
            if(args.Count < 1) throw new InvalidArgumentLengthException("Expected 1 argument but got 0 for command \"teleport\" at line "+i+".");

            if (args.Count < 4)
            {
                var roleInfo = RoleInfo.parseRole(args[0], "teleport", i, 0);
                
                var door = UnityEngine.Object.FindObjectsOfType<DoorVariant>().FirstOrDefault(_door => _door.name.Trim().ToUpper() == args[1].Trim().ToUpper());
                if(door == null) throw new InvalidArgumentException("Invalid argument for command \"teleport\" on line "+i+", argument 1. The door name specified is not valid.");
                
                if (!PlayerMovementSync.FindSafePosition(door.transform.position, out var pos))
                {
                    throw new CommandErrorException("No safe position could be found for door \""+door.name+"\".");
                }

                if (args.Count == 3)
                {
                    if(!int.TryParse(args[2].Trim(), out var delay)) throw new InvalidArgumentException("Invalid argument for command \"teleport\" on line "+i+", argument 2. Expected \"INT\" but got \""+args[2]+"\".");
                    
                    ScriptActions.GetDelay(delay).teleportIds.Add(new TeleportData(pos, roleInfo));
                }
                else ScriptActions.scriptData.teleportIds.Add(new TeleportData(pos, roleInfo));
            }
            else
            {
                var roleInfo = RoleInfo.parseRole(args[0], "teleport", i, 0);

                if(!int.TryParse(args[1].Trim(), out var x)) throw new InvalidArgumentException("Invalid argument for command \"teleport\" on line "+i+", argument 1. Expected \"INT\" but got \""+args[1]+"\".");
                if(!int.TryParse(args[2].Trim(), out var y)) throw new InvalidArgumentException("Invalid argument for command \"teleport\" on line "+i+", argument 2. Expected \"INT\" but got \""+args[2]+"\".");
                if(!int.TryParse(args[3].Trim(), out var z)) throw new InvalidArgumentException("Invalid argument for command \"teleport\" on line "+i+", argument 3. Expected \"INT\" but got \""+args[3]+"\".");

                if (args.Count == 5)
                {
                    if(!int.TryParse(args[4].Trim(), out var delay)) throw new InvalidArgumentException("Invalid argument for command \"teleport\" on line "+i+", argument 2. Expected \"INT\" but got \""+args[2]+"\".");
                    
                    ScriptActions.GetDelay(delay).teleportIds.Add(new TeleportData(new Vector3(x, y, z), roleInfo));
                }
                else ScriptActions.scriptData.teleportIds.Add(new TeleportData(new Vector3(x, y, z), roleInfo));
            }
        }
    }
}