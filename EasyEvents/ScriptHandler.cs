using System;
using System.Linq;
using System.Text.RegularExpressions;
using EasyEvents.Commands;

namespace EasyEvents
{
    public static class ScriptHandler
    {
        public static void RunScript(string inputText)
        {
            var arr = inputText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            
            for (var i = 0; i < arr.Length; i++)
            {
                var s = arr[i];

                var args = Regex
                    .Split(String.Join(" ", s), "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)")
                    .Where(item => !string.IsNullOrEmpty(item))
                    .ToList();
                
                if (args.Count < 1) continue;
                
                var cmd = args[0].Trim().ToLower();
                args.RemoveAt(0);
                
                if (cmd.StartsWith("#") || cmd.StartsWith("//") || cmd == string.Empty) continue;

                switch (cmd)
                {
                    case "spawn":
                        Spawn.Run(args, i);
                        break;
                    
                    case "roundlock":
                        RoundSummary.RoundLock = true;
                        break;
                    
                    case "detonate":
                        ScriptActions.scriptData.detonate = true;
                        break;
                    
                    case "teleport":
                        Teleport.Run(args, i);
                        break;
                    
                    case "createclass":
                        CreateClass.Run(args, i);
                        break;
                    
                    case "clearitems":
                        ClearItems.Run(args, i);
                        break;
                    
                    case "give":
                        Give.Run(args, i);
                        break;
                    
                    case "infect":
                        Infect.Run(args, i);
                        break;
                    
                    case "hp":
                        HP.Run(args, i);
                        break;
                    
                    case "scale":
                        Scale.Run(args, i);
                        break;
                    
                    case "disabledecontamination":
                        DisableDecontamination.Run(args, i);
                        break;
                    
                    case "last":
                        Last.Run(args, i);
                        break;
                    
                    case "cassie":
                        TextCommand.Run(args, i, "cassie");
                        break;
                    
                    case "broadcast":
                        TextCommand.Run(args, i, "broadcast");
                        break;
                    
                    case "hint":
                        TextCommand.Run(args, i, "hint");
                        break;
                    
                    case "lights":
                        Lights.Run(args, i);
                        break;
                    
                    case "disabledetonation":
                        DisableDetonation.Run(args, i);
                        break;
                    
                    case "escape":
                        Commands.Escape.Run(args, i);
                        break;
                    
                    default:
                        throw new InvalidCommandException("The command \""+cmd+"\" on line "+i+" was not found.");
                        break;
                }
            }
        }
    }
}