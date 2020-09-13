using System;
using System.Collections.Generic;
using System.Linq;
using EasyEvents.Commands;
using EasyEvents.Types;
using Exiled.API.Features;
using MEC;

namespace EasyEvents
{
    public static class ScriptActions
    {
        public static bool eventRan = false;
        
        private static List<int[]> classIds = null;
        private static int finalClassId = -1;

        private static List<TeleportData> teleportIds = null;

        public static bool detonate = false;
        
        public static void SetCustomSpawn(List<int[]> _classIds, int _finalClassId, int line)
        {
            if (classIds != null) throw new CommandErrorException("Error running command \"spawn\" at line "+line+": Custom spawns have already been set. Only run the \"spawn\" command once.");

            classIds = _classIds;
            finalClassId = _finalClassId;
        }

        public static void SetTeleport(List<TeleportData> _teleportIds, int line)
        {
            if (teleportIds != null) throw new CommandErrorException("Error running command \"teleport\" at line "+line+": A teleport rule has already been set. Only run the \"teleport\" command once.");

            teleportIds = _teleportIds;
        }

        public static void AddEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public static void RemoveEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            classIds = null;
            finalClassId = -1;
            teleportIds = null;
            detonate = false;
            eventRan = false;
        }
        
        private static void OnRoundStarted()
        { 
            Timing.RunCoroutine(RoundStart());
        }
        
        private static IEnumerator<float> RoundStart()
        {
            yield return Timing.WaitForSeconds(1f);
            
            if (classIds != null)
            {
                SetRoles();
                yield return Timing.WaitForSeconds(1f);
            }

            if (teleportIds != null)
            {
                Teleport();
            }

            if (detonate)
            {
                AlphaWarheadController.Host.StartDetonation();
            }
        }

        private static void SetRoles()
        {
            var players = Player.List.ToList();
            players.Shuffle();
            
            foreach (var classId in classIds)
            {
                var role = (RoleType) classId[0];
                var percent = classId[1];
                
                for (var i = 0; i < players.Count; i++)
                {
                    if ((i != 0) && (i * percent / 100) <= ((i - 1) * percent / 100)) continue;
                    
                    players[i].SetRole(role);
                    players.RemoveAt(i);
                }
                
                players.Shuffle();
            }

            if (players.Count > 0 && finalClassId != -1)
            {
                var role = (RoleType) finalClassId;
                
                foreach (var player in players)
                {
                    player.SetRole(role);
                }
            }
        }

        private static void Teleport()
        {
            foreach (var data in teleportIds)
            {
                if (!PlayerMovementSync.FindSafePosition(data.door.transform.position, out var pos))
                {
                    throw new EventRunErrorException("No safe position could be found for door \""+data.door.DoorName+"\".");
                }

                var role = (RoleType) data.classId;
                var players = Player.List.Where(player => player.Role == role).ToList();
                
                foreach (var player in players)
                {
                    player.GameObject.GetComponent<PlayerMovementSync>().OverridePosition(pos, 0f, false);
                }
            }
        }
    }
}