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
            
            if (args[0].Trim().ToLower().StartsWith("g:") && CustomRoles.roles.ContainsKey(args[0].Trim().ToLower()))
            {
                if(!CustomRoles.roles.TryGetValue(args[0].Trim().ToLower(), out var customRole)) throw new InvalidArgumentException("Invalid argument for command \"clearitems\" on line "+i+", argument 0. Expected \"(0-17)\" but got \""+args[0]+"\".");

                ScriptActions.clearItems.Add(new ClearItemsData(customRole, customRole.classId));
            }
            else
            {
                if(!int.TryParse(args[0].Trim(), out var classId)) throw new InvalidArgumentException("Invalid argument for command \"clearitems\" on line "+i+", argument 0. Expected \"(0-17)\" but got \""+args[0]+"\".");
                if(classId < 0 || classId > 17) throw new InvalidArgumentException("Invalid argument for command \"clearitems\" on line "+i+", argument 0. Expected \"(0-17)\" but got \""+args[0]+"\".");

                ScriptActions.clearItems.Add(new ClearItemsData(null, classId));
            }
        }
    }
}