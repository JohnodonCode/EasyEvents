using System.Collections.Generic;
using System.Linq;

namespace EasyEvents
{
    public static class ScriptHandler
    {
        public static void RunScript(string inputText)
        {
            var arr = inputText.Split('\n');
            
            for (var i = 0; i < arr.Length; i++)
            {
                var s = arr[i];
                
                var cmd = s.Split(' ')[0].ToLower();
                var args = s.Split(' ').ToList();
                if (args.Count > 0) args.RemoveAt(0);

                switch (cmd)
                {
                    case "spawn":
                        if (args.Count < 1) throw new InvalidArgumentLengthException("Expected 1 argument but got 0 for command \"spawn\" at line "+i+".");

                        var finalClassId = -1;
                        var sum = 0;
                        var classIds = new List<int[]>();

                        for (var y = 0; y < args.Count; y++)
                        {
                            var argEls = args[y].Split(',');
                            
                            if(argEls.Length < 1) throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17),(0-100)\" but got \""+args[y]+"\".");
                            
                            if (argEls.Length == 1)
                            {
                                if (y != args.Count - 1 || finalClassId != -1) throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17),(0-100)\" but got \""+args[y]+"\".");

                                    if(!int.TryParse(argEls[0], out var classId)) throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17)\" but got \""+args[y]+"\".");
                                if(classId < 0 || classId > 17) throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17),(0-100)\" but got \""+args[y]+"\".");

                                finalClassId = classId;
                            }
                            else if (argEls.Length == 2)
                            {
                                if(!int.TryParse(argEls[0], out var classId) || !int.TryParse(argEls[1], out var chance)) throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17),(0-100)\" but got \""+args[y]+"\".");
                                if(classId < 0 || classId > 17 || chance < 0 || chance > 100) throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17),(0-100)\" but got \""+args[y]+"\".");

                                sum += chance;
                                classIds.Add(new int[] {classId, chance});
                            }
                            else throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17),(0-100)\" but got \""+args[y]+"\".");
                        }
                        
                        if(sum > 100) throw new InvalidArgumentException("Invalid arguments for command \"spawn\" on line "+i+", argument. The sum of spawn chances should never exceed 100. Got "+sum+".");
                        if(sum < 100 && finalClassId == -1) throw new InvalidArgumentException("Invalid arguments for command \"spawn\" on line "+i+", argument. The sum of spawn chances shouldn't be less than 100 unless you have set a class to use for the remaining players. Got "+sum+".");
                        
                        ScriptActions.setCustomSpawn(classIds, finalClassId);
                        break;
                }
            }
        }
    }
}