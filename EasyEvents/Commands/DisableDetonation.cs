using System.Collections.Generic;

namespace EasyEvents.Commands
{
    public static class DisableDetonation
    {
        public static void Run(List<string> args, int i)
        {
            ScriptActions.scriptData.disableNuke = true;
        }
    }
}