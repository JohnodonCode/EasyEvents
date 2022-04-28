using System.Collections.Generic;
using System;
using System.Linq;
using EasyEvents.Types;
using Exiled.API.Enums;
using Exiled.API.Features;
using Door2 = Exiled.API.Features.Door;

namespace EasyEvents.Commands
{
    public static class Door
    {
        public static void Run(List<string> args, int i)
        {
            if (args.Count < 2) throw new InvalidArgumentLengthException("Expected 2 arguments but got " + args.Count + " for command \"door\" at line " + i + ".");
            DoorType targetDoorType;
            bool isDoor = Enum.TryParse(args[0], out targetDoorType);
            if (!isDoor)
            {
                throw new InvalidArgumentException("Invalid argument for command \"door\" on line " + i + ", argument 0. \"" + args[0] + "\" is not a valid door name. Door names are case sensitive.");
            }
            else
            {
                List<Door2> targetDoors = new List<Door2>();
                foreach(Door2 door in Map.Doors)
                {
                    if (door.Type == targetDoorType) targetDoors.Add(door);
                }
                if (args.Count == 3)
                {
                    if (!int.TryParse(args[2].Trim(), out var delay)) throw new InvalidArgumentException("Invalid argument for command \"door\" on line " + i + ", argument 2. Expected \"INT\" but got \"" + args[2] + "\".");

                    ScriptActions.GetDelay(delay).doorData.Add(new DoorData(targetDoors, args[1], i));
                }
                else
                {
                    ScriptActions.scriptData.doorData.Add(new DoorData(targetDoors, args[1], i));
                }
            }
        }
    }
}