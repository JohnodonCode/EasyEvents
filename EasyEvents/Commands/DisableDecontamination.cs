using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;

namespace EasyEvents.Commands
{
    public static class DisableDecontamination
    {
        public static void Run(List<string> args, int i)
        {
            ScriptActions.disableDecontamination = true;
        }
    }
}