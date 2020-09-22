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
            if(args.Count < 3 && cmd != "cassie") throw new InvalidArgumentLengthException("Expected 2 arguments but got "+args.Count+" for command \""+cmd+"\" at line "+i+".");
            if(cmd == "cassie" && args.Count < 1) throw new InvalidArgumentLengthException("Expected 1 argument but got "+args.Count+" for command \""+cmd+"\" at line "+i+".");

            RoleInfo role = null;
            var duration = 0;
            if (cmd != "cassie")
            { 
                role = RoleInfo.parseRole(args[0], cmd, i, 1);
                args.RemoveAt(0);
                
                if(!int.TryParse(args[0].Trim(), out duration)) throw new InvalidArgumentException("Invalid argument for command \""+cmd+"\" on line "+i+", argument 1. Expected \"INT\" but got \""+args[0]+"\".");
                args.RemoveAt(0);
            }

            if (int.TryParse(args.Last(), out var delay))
            {
                args.pop();
            }

            var data = new TextData(args[0].Trim('"'), role, duration);
            
            switch (cmd)
            {
                case "cassie":
                    if (delay == -1)
                    {
                        ScriptActions.scriptData.cassie.Add(data);
                    }
                    else
                    {
                        ScriptActions.GetDelay(delay).cassie.Add(data);
                    }
                    break;
                
                case "broadcast":
                    if (delay == -1)
                    {
                        ScriptActions.scriptData.broadcast.Add(data);
                    }
                    else
                    {
                        ScriptActions.GetDelay(delay).broadcast.Add(data);
                    }
                    break;
                
                case "hint":
                    if (delay == -1)
                    {
                        ScriptActions.scriptData.hint.Add(data);
                    }
                    else
                    {
                        ScriptActions.GetDelay(delay).hint.Add(data);
                    }
                    break;
            }
        }
    }
}