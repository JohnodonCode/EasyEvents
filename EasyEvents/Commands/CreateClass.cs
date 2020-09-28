using System;
using System.Collections.Generic;

namespace EasyEvents.Commands
{
    public static class CreateClass
    {
        public static void Run(List<string> args, int i)
        {
            if(args.Count < 2) throw new InvalidArgumentLengthException("Expected 2 arguments but got "+args.Count+" for command \"createclass\" at line "+i+".");

            var customId = "g:"+args[0].Trim().ToLower();

            var roleInfo = RoleInfo.parseRole(args[1], "createclass", i, 1);
            var classId = roleInfo.classId;
            
            if(classId < 0 || classId > 17) throw new InvalidArgumentException("Invalid argument for command \"createclass\" on line "+i+", argument 1. Expected \"(0-17)\" but got \""+args[1]+"\".");
            
            CustomRoles.roles.Add(customId, new CustomRole(customId, classId));
        }
    }
}