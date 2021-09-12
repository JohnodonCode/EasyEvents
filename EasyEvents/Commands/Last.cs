using System.Collections.Generic;

namespace EasyEvents.Commands
{
    public class Last
    {
        public static void Run(List<string> args, int i)
        {
            if(args.Count < 1) throw new InvalidArgumentLengthException("Expected 1 argument but got "+args.Count+" for command \"last\" at line "+i+".");
            
            var lastRole = RoleInfo.parseRole(args[0], "last", i, 1);
            
            ScriptActions.scriptData.last.Add(lastRole);
        }
    }
}