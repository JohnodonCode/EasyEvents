using System;
using System.Collections.Generic;
using System.Linq;
using EasyEvents.Types;

namespace EasyEvents.Commands
{
    public class TextCommand
    {
        public static void Run(List<string> args, int i, string cmd)
        {
            if(args.Count < 2) throw new InvalidArgumentLengthException("Expected 2 arguments but got "+args.Count+" for command \""+cmd+"\" at line "+i+".");
            
            RoleInfo role = null;
            if (cmd != "cassie")
            { 
                role = RoleInfo.parseRole(args[0], cmd, i, 1);
                args.RemoveAt(0);
            }

            var message = String.Join(" ", args);

            var delay = -1;
            if (int.TryParse(args.Last(), out delay))
            {
                args.pop();
            }
            
            switch (cmd)
            {
                case "cassie":
                    if (delay == -1)
                    {
                        ScriptActions.scriptData.cassie.Add(new TextData(message, null));
                    }
                    else
                    {
                        ScriptActions.GetDelay(delay).cassie.Add(new TextData(message, null));
                    }
                    break;
                
                case "broadcast":
                    if (delay == -1)
                    {
                        ScriptActions.scriptData.broadcast.Add(new TextData(message, role));
                    }
                    else
                    {
                        ScriptActions.GetDelay(delay).broadcast.Add(new TextData(message, role));
                    }
                    break;
                
                case "hint":
                    if (delay == -1)
                    {
                        ScriptActions.scriptData.hint.Add(new TextData(message, role));
                    }
                    else
                    {
                        ScriptActions.GetDelay(delay).hint.Add(new TextData(message, role));
                    }
                    break;
            }
        }
    }
}