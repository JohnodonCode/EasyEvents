using System.Collections.Generic;
using EasyEvents.Types;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace EasyEvents.Commands
{
    public static class Lights
    {
        public static void Run(List<string> args, int i)
        {
            if(args.Count < 2) throw new InvalidArgumentLengthException("Expected 2 arguments but got "+args.Count+" for command \"lights\" at line "+i+".");

            var _fixedName = args[0].Trim().ToUpper();
            var HCZOnly = false;
            if (_fixedName == "HCZ") HCZOnly = true;
            else if (_fixedName != "LCZ") throw new InvalidArgumentException("Invalid argument for command \"lights\" on line "+i+", argument 0. Expected \"LCZ/HCZ\" but got \""+args[0]+"\".");
            
            if(!int.TryParse(args[1].Trim(), out var time)) throw new InvalidArgumentException("Invalid argument for command \"lights\" on line "+i+", argument 1. Expected \"INT\" but got \""+args[1]+"\".");

            if (args.Count == 3)
            {
                if(!int.TryParse(args[2].Trim(), out var delay)) throw new InvalidArgumentException("Invalid argument for command \"lights\" on line "+i+", argument 2. Expected \"INT\" but got \""+args[2]+"\".");
                    
                ScriptActions.GetDelay(delay).lights.Add(new LightData(HCZOnly, time));
            }
            else
            {
                ScriptActions.scriptData.lights.Add(new LightData(HCZOnly, time));
            }
        }
    }
}