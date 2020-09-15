using System;
using System.Collections.Generic;
using EasyEvents.Types;

namespace EasyEvents.Commands
{
    public static class HP
    {
        public static void Run(List<string> args, int i)
        {
            if(args.Count < 2) throw new InvalidArgumentLengthException("Expected 2 arguments but got "+args.Count+" for command \"hp\" at line "+i+".");
            
            var roleInfo = RoleInfo.parseRole(args[0], "hp", i, 0);
            if(!int.TryParse(args[1].Trim(), out var amount)) throw new InvalidArgumentException("Invalid argument for command \"hp\" on line "+i+", argument 1. Expected \"INT\" but got \""+args[1]+"\".");
            
            ScriptActions.hpData.Add(new HPData(roleInfo, amount));
        }
    }
}