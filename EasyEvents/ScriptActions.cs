using System;
using System.Collections.Generic;
using EasyEvents.Events;

namespace EasyEvents
{
    public static class ScriptActions
    {
        private static RoundStarted customSpawnEvent;
        public static void SetCustomSpawn(List<int[]> classIds, int finalClassId, int line)
        {
            if (customSpawnEvent != null) throw new CommandErrorException("Error running command \"spawn\" at line "+line+": Custom spawns have already been set. Only run the \"spawn\" command once.");
            
            customSpawnEvent = new RoundStarted(classIds, finalClassId);
            Exiled.Events.Handlers.Server.RoundStarted += customSpawnEvent.onRoundStarted;
        }

        public static void RemoveEvents()
        {
            if (customSpawnEvent != null)
            {
                Exiled.Events.Handlers.Server.RoundStarted -= customSpawnEvent.onRoundStarted;
                customSpawnEvent = null;
            }
        }
    }
}