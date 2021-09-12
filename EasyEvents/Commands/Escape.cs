using System.Collections.Generic;

namespace EasyEvents.Commands
{
    public class Escape
    {
        public static void Run(List<string> args, int i)
        {
            if(args.Count < 1) throw new InvalidArgumentLengthException("Expected 1 argument but got "+args.Count+" for command \"escape\" at line "+i+".");
            
            var escapeRole = RoleInfo.parseRole(args[0], "escape", i, 1);
            
            ScriptActions.scriptData.escape.Add(escapeRole);
        }
    }
}