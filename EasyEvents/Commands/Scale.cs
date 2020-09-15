using System.Collections.Generic;
using EasyEvents.Types;
using UnityEngine;

namespace EasyEvents.Commands
{
    public class Scale
    {
        public static void Run(List<string> args, int i)
        {
            if(args.Count < 4) throw new InvalidArgumentLengthException("Expected 3 arguments but got "+args.Count+" for command \"scale\" at line "+i+".");
            
            var roleInfo = RoleInfo.parseRole(args[0], "scale", i, 0);
            
            if(!int.TryParse(args[1].Trim(), out var x)) throw new InvalidArgumentException("Invalid argument for command \"hp\" on line "+i+", argument 1. Expected \"INT\" but got \""+args[1]+"\".");
            if(!int.TryParse(args[2].Trim(), out var y)) throw new InvalidArgumentException("Invalid argument for command \"hp\" on line "+i+", argument 2. Expected \"INT\" but got \""+args[2]+"\".");
            if(!int.TryParse(args[3].Trim(), out var z)) throw new InvalidArgumentException("Invalid argument for command \"hp\" on line "+i+", argument 3. Expected \"INT\" but got \""+args[3]+"\".");
            
            
            ScriptActions.sizeData.Add(new SizeData(roleInfo, new Vector3(x, y, z)));
        }
    }
}