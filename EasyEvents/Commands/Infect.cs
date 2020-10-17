using System.Collections.Generic;
using EasyEvents.Types;

namespace EasyEvents.Commands
{
    public class Infect
    {
        public static void Run(List<string> args, int i)
        {
            if(args.Count < 2) throw new InvalidArgumentLengthException("Expected 2 arguments but got "+args.Count+" for command \"infect\" at line "+i+".");

            var oldRole = RoleInfo.parseRole(args[0], "infect", i, 0);
            var newRole = RoleInfo.parseRole(args[1], "infect", i, 1);

            var soft = args.Count > 2 && args[2].Trim().ToLower() == "soft";

            ScriptActions.scriptData.infectData.Add(new InfectData(oldRole, newRole, soft));
        }
    }
}