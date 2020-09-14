using System;
using System.Collections.Generic;
using EasyEvents.Types;
using Exiled.API.Features;

namespace EasyEvents.Commands
{
    public static class ClearItems
    {
        public static void Run(List<string> args, int i)
        {
            if(args.Count < 1) throw new InvalidArgumentLengthException("Expected 1 argument but got "+args.Count+" for command \"clearitems\" at line "+i+".");
            
            var roleInfo = RoleInfo.parseRole(args[0], "clearitems", i, 0);
            
            ScriptActions.clearItems.Add(new RoleInfo(roleInfo.roleID, roleInfo.classId));
        }
    }
}