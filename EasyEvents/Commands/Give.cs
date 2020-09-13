using System;
using System.Collections.Generic;
using EasyEvents.Types;

namespace EasyEvents.Commands
{
    public static class Give
    {
        public static void Run(List<string> args, int i)
        {
            if(args.Count < 2) throw new InvalidArgumentLengthException("Expected 2 arguments but got "+args.Count+" for command \"give\" at line "+i+".");
            
            var roleInfo = RoleInfo.parseRole(args[0], "give", i, 0);

            if(!Enum.TryParse<ItemType>(args[1].Trim(), true, out var item)) throw new InvalidArgumentException("Invalid argument for command \"give\" on line "+i+", argument 1. Expected \"ITEM_TYPE\" but got \""+args[0]+"\".");
            
            ScriptActions.giveData.Add(new GiveData(item, roleInfo));
        }
    }
}